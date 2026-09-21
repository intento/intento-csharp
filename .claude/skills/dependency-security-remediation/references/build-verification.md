# Verifying a dependency change didn't break or alter the build

Read this at Phase 4, once the audit is clean and before you call the work done. Two things
live here: the specific ways a dependency bump breaks a toolchain, and how to prove what the
fix did to a shipped artifact.

Failure modes worth watching for specifically. The examples are JavaScript because that is
where they were observed, but each names a class that exists wherever a toolchain resolves its
own plugins — a moved peer dependency, a changed export shape, a dropped default, two copies of
one tool:

- **A package your build config uses but never declared disappears.** Rung 0 changes the hoisted
  layout, so anything a config file `require()`s without declaring it in `package.json` —
  available until now only because some dependency happened to hoist it into place — can vanish.
  This needs no major version: webpack moved `terser-webpack-plugin` out of its own dependencies
  in a *minor* release, and a `webpack.config.js` that had called
  `require('terser-webpack-plugin')` for years stopped loading. It fails loudly
  (`Cannot find module` at config load), which is the one merciful thing about it. Fix by
  declaring the package as a direct dependency — the project genuinely uses it, and the old
  arrangement was luck — never by pinning the parent back to the version that hoisted it. Same
  family as the `ts-jest` peer-dependency case below, but it does not need a strict linker: plain
  hoisting is enough, so it can bite any project. Worth grepping build configs for `require(`
  and `import` of anything absent from `package.json` before you start.

- **A peer dependency moved out of `dependencies`, in a bump that looks trivial.** `ts-jest`
  moved `jest-util` from `dependencies` to `peerDependencies` between 29.3.2 and 29.4.12 — a
  minor bump. Under a strict, non-hoisting linker (`nodeLinker: pnpm`, pnpm itself, Yarn PnP)
  the unmet peer simply isn't there and `require('jest-util')` throws at run time. The install
  exits 0; only running the suite finds it. Fix by adding the peer as an explicit devDependency
  at the version the rest of the tree uses. Diff `npm view <pkg>@<old> dependencies` against
  `@<new>` whenever a bump breaks a tool that used to work.
- **A tool's exported factory stops being constructible.** `new tsJest.createTransformer()`
  worked for years and threw `is not a constructor` after a minor bump, because the export became
  a shorthand method. Repo-local config and build-tools files are where this lands; they are
  typically neither typechecked nor linted.
- **A major silently drops something the previous major provided by default** — a builder's
  built-in transform loader, a formatter, a legacy-config shim. The config still parses; the
  command fails. Read the new major's changelog for what replaced the default rather than routing
  around the symptom.
- **Two copies of the same tool at different versions, with something bridging them.** If your
  config does `require('webpack')` to build plugin instances that get handed to a *different*
  compiler resolved internally by some other dependency, version drift breaks at an internal API
  boundary. Resolve the same instance the other tool uses
  (`require.resolve('webpack', { paths: [require.resolve('<the-other-tool>')] })`) rather than
  chasing matching patch versions.

When something breaks after a batch of bumps, **bisect rather than guess** — revert to a
confirmed-working baseline first (worth doing anyway, to confirm the bug is new) and reintroduce
changes one at a time. The obvious suspect is often innocent: in the `ts-jest` case above the
suspect was a jest major, and the culprit was a patch-level bump nobody looked at twice.


## If the project ships a build artifact, prove what changed in it

A dev-dependency-only fix is commonly justified with "none of this reaches the published
artifact." That is true of the *vulnerabilities* and not necessarily true of the *fix*. Check
rather than assert.

This applies to anything that ships a built output rather than resolving dependencies at run
time — a JS bundle, a compiled binary, a published NuGet package, a container image. The
commands below are JS; the two questions they answer are not. Is the build reproducible at
all? And what actually changed in the output?

**First establish whether the build is even deterministic**, before attributing any difference to
your change. Build twice on the *unchanged* tree and compare:

```bash
yarn build && cp -R dist /tmp/a && yarn build && diff -rq /tmp/a dist
```

Plenty of builds are not reproducible — a glob-import plugin that doesn't sort its results will
reorder modules run to run. Skip this and you will confidently attribute pre-existing noise to
your dependency bump. (Report the non-determinism as a separate finding; it's real, and it means
nobody can answer "did this change the bundle?" by diffing artifacts.)

**Then compare semantically, not bytewise.** Source maps make this exact: the `sources` list names
every module that went into the bundle, so a sorted diff of the two lists says precisely what was
added and removed, independent of ordering and minification.

The class that motivates all of this: **a relative `browserslist` query turns a lockfile refresh
into a product change.** A target like `last 4 versions` means nothing until it's resolved against
`caniuse-lite`, whose version is pinned in the lockfile — so the shipped browser-support floor is
a side effect of when someone last updated the lock, not a decision anyone made. On one repo,
deduplicating `browserslist` to close two CVEs moved the floor about ten months (Chrome 130 → 148,
Safari 18.0 → 26.3), and `useBuiltIns: 'usage'` responded by dropping 16 core-js polyfills;
`dist/index.js` shrank 9512 bytes while the project's own 83 modules stayed byte-identical. That
is a shipped behaviour change arriving through a "dev-only" security fix. Surface it as a decision
for the humans — and note that an explicit floor would have made it a non-event.


## Clear the cache before believing any negative result

The techniques above — bisecting, reverting a fix to see whether a test catches it, mutating a
config to prove an option is consumed — all rest on "I broke it and the suite noticed". A warm
cache silently removes that guarantee, and it fails in the reassuring direction: the suite passes
and you conclude the code is fine.

Observed on one repo: restoring a genuinely broken call — `new tsJest.createTransformer()`, which
*cannot* work on ts-jest 29.4 — **passed 55 suites on a warm cache** and failed immediately with
`TypeError: tsJest.createTransformer is not a constructor` on a cold one. Jest reuses cached
transform output, so the transformer's `process` was never called at all.

So: `jest --clearCache` (or your runner's equivalent, and the bundler's, and the package
manager's) before any run whose *meaning* depends on a failure appearing. A green baseline run
can be warm; a green run that is supposed to be evidence cannot.

**When a mutation still won't bite, the tool is answering from somewhere else — find out where
before concluding the code is fine.** In the same repo a bogus `tsconfig` path on one transform
entry killed all 55 suites, while the identical mutation on a neighbouring entry did nothing,
before *and* after the fix. The reason was a second cache: ts-jest's `_cachedConfigSets` is
static and keyed on the jest config *object*, so whichever transformer initialises first decides
the configuration every other one gets. The mutation was inert for a real reason that had nothing
to do with the change under test.

The escape is to prove the behaviour **out of band** — call the thing directly in a plain script
with a freshly constructed config object, where nothing is shared:

```js
tsJest.createTransformer(opts).process('export const x = 1', '<file>',
    { config: { rootDir, cwd, globals: {}, __tag: label }, cacheFS: new Map() })
// undefined                            -> compiles      (the broken call)
// { tsconfig: './tsconfig.NOPE.json' } -> File not found (the option is consumed)
```

An in-suite check that cannot be made to fail is not evidence of correctness; it is evidence that
the suite is not exercising the thing you are changing. Say which of the two you established.

## Check what the remediation added to the publishable surface

Scanners and test runs leave files behind — a SAST cache, a JSON report, coverage output — and
some publish configurations ship every new file by default. So the act of following this method
can add something to your released package. Check before publishing, and diff rather than eyeball:

```bash
npm pack --dry-run --ignore-scripts --json > /tmp/before.json   # on the base revision
npm pack --dry-run --ignore-scripts --json > /tmp/after.json    # after remediation
```

The case that motivates this: **`snyk code test` writes `.dccache`** — a JSON map with one entry
per scanned file, keyed by **absolute path**, so it carries the maintainer's username and full
directory layout. On one repo it was gitignored, which is exactly why nobody looked, and shipped
anyway: **once a `.npmignore` exists, npm never consults `.gitignore`.** A `npm pack --dry-run`
measured 747 files in the tarball, including `.dccache`, 109 files of `coverage/`, and 98KB of
internal review notes.

Two rules follow:

- **A denylist is the wrong default.** `.npmignore` means every new artefact has to be remembered
  twice, and the next tool cache lands in the tarball for free. A `files` allowlist in
  `package.json` inverts it, so new files are excluded until someone opts them in. Note this is
  not the same as deleting `.npmignore` and letting `.gitignore` apply — build output is commonly
  gitignored *and* published, so that swap ships nothing.
- **Publish-surface changes belong to whoever owns releases.** Adding one cache file to the
  denylist is in scope for a security changeset; switching to an allowlist changes what every
  future release contains, and that is a separate decision (Phase 5).
