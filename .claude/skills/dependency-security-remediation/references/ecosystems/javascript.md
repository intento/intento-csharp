# JavaScript / TypeScript — fix mechanics

The ecosystem file for `package.json` projects, covering yarn (Classic and Berry), npm and
pnpm. Two things live here: how to force re-resolution (Phase 2 rung 0) and how to write a
forced version override (rung 4).

Reach for the override half only after rung 0 (the finding is often just a stale lock entry)
and rung 1 (is the package even used?) — most overrides that get written were never needed.
And note the failure shape of this whole file: getting a key wrong doesn't error, it silently
does nothing, which is worse than an error because the audit still shows the finding and you
will think you already handled it.

## Forcing re-resolution without writing an override (rung 0)

Before any of the syntax below, the cheaper move is to make the package manager re-resolve
within the ranges already declared. The Yarn Berry mechanics have sharp edges worth knowing
in advance; all of these were confirmed on Yarn 4.9:

- `yarn up -R <pkg> [<pkg>...]` re-resolves **every** descriptor of those packages,
  transitive ones included. Pass the whole list of flagged package names in one invocation.
- **`-R` rejects ranges.** `yarn up -R webpack-dev-server@^5` fails with
  `Usage Error: Ranges aren't allowed when using --recursive`. So when a package needs both
  recursion *and* a ceiling, it takes two passes: `-R` for the transitive consumers, and a
  separate plain `yarn up <pkg>@<range>` for your own declared range.
- **Plain `yarn up <pkg>` rewrites `package.json`,** and it will happily cross a major —
  `yarn up -R webpack-dev-server` jumps to 6.x when you wanted the patched 5.x. Constrain it
  (`yarn up 'webpack-dev-server@^5'`) whenever a newer major exists.
- **It also widens what it rewrites.** `yarn up 'webpack-dev-server@^5'` leaves `"^5"` in
  `package.json`, discarding the more informative `"^5.2.1"` floor that was there. Restore a
  meaningful floor by hand afterwards — ideally the patched version, so the range documents
  which advisory it answers.
- **Yarn deletes an empty `resolutions: {}` on the next install.** Harmless in itself, but it
  means the empty-the-block experiment in Phase 3 changes the file again underneath you;
  re-read before your next edit rather than assuming your version is current.

Leave the ranges of unaffected direct dependencies alone. If an existing caret already
reaches the fix, re-resolution is enough, and the smaller diff is the more reviewable one.

## Yarn Berry (`resolutions` field, Yarn 2+)

```json
"resolutions": {
  "lodash": "^4.18.1",
  "ajv@npm:^8.0.0": "^8.20.0"
}
```

Two key forms, and they behave differently:

- **Plain package name** (`"lodash"`) — a *blanket* override. Every request for `lodash`
  anywhere in the tree, regardless of what range it asks for, gets rewritten to resolve
  against your target range instead. Yarn doesn't check whether your target actually
  satisfies the original requester's declared range — it just forces it. This is fine for
  simple, API-stable leaf utilities (semver, brace-expansion-style string/glob helpers)
  where every version in the observed range behaves the same at the call sites your
  project's dependencies use. It is *not* fine for packages with real API differences
  across majors (`ajv` 6.x vs 8.x behave differently enough that eslint's own ajv 6 usage
  and some loader's ajv 8 usage cannot share one forced version).

- **Scoped key** (`"ajv@npm:^8.0.0"`) — only rewrites requests whose descriptor string is
  *exactly* `ajv@npm:^8.0.0`. Find the real descriptor string first — don't guess it —
  with `yarn why <pkg>` (shows `via npm:^X.Y.Z` per dependent) or by checking the
  dependent's own `package.json` directly (`npm view <dependent>@<version>
  dependencies.<pkg>`), since a resolution's own effect on `yarn why`'s displayed range
  can be confusing once one is already active. If a dependent later bumps and starts
  requesting a *different* range string (e.g. moves from `^9.0.4` to `^10.2.2`), your old
  scoped key silently stops matching anything — it becomes a no-op, not an error. This is
  exactly the kind of dead entry Phase 3 in SKILL.md is about finding.

You can combine both a blanket key and per-scope keys for the same package name if
different major branches genuinely need different treatment (some consumers on 2.x,
others on 5.x, one of them lacking the security fix that only landed in the 5.x branch).

### Scoped keys break Snyk — check before you reach for one

**If the project is scanned by Snyk, a scoped key is not available to you.** Yarn honours it
and `yarn npm audit` is happy, but `snyk test` aborts **the whole scan** — not just that
package — with:

```
Dependency pacote@npm:^15.1.1 was not found in yarn.lock.
Your package.json and yarn.lock are probably out of sync. Please run "yarn install" and try again.
```

Snyk reads the requester's *declared* range (`pacote: ^15.1.1`, from
`rollup-plugin-filesize`'s own `package.json`) and looks for a matching `yarn.lock`
descriptor. The scoped resolution rewrote that descriptor to `pacote@npm:21.5.1`, so nothing
matches. The message blames the lockfile, so the natural response is to reinstall or
regenerate it, which never helps.

The blanket key `"pacote": "21.5.1"` scans cleanly and installs identically. When you are
pushed onto a blanket key for this reason, say so in the `_resolutionsNotes` entry along with
the single consumer it is really for — otherwise the next person reads its breadth as
intent and either widens it further or narrows it back and breaks the scan again.

Verified against Snyk CLI 1.1293.1 and Yarn 4.9.1.

## Yarn Classic (`resolutions` field, Yarn 1)

Same field name, same nested-path matching semantics as Berry for the common cases, but
Yarn 1 also supports (and is more commonly seen using) a slash-path form to scope by
dependency chain rather than by exact range string:

```json
"resolutions": {
  "webpack-dev-server/**/ws": "^7.5.10"
}
```

This targets `ws` specifically when required through `webpack-dev-server`'s tree, however
deep. Prefer this over guessing a version-range string when the goal is "this specific
dependent's copy," since Yarn 1's range-string scoping is less consistently documented
than Berry's.

## npm (`overrides` field, npm 8.3+)

```json
"overrides": {
  "lodash": "^4.18.1",
  "some-package": {
    "ajv": "^8.20.0"
  }
}
```

npm's plain top-level key is also a blanket override, same caveat as Yarn Berry's plain
key. To scope narrowly, nest it under the parent package name instead of using a range
string as the key — `overrides.some-package.ajv` only affects `ajv` when required by
`some-package`, regardless of what range `some-package` itself declares. npm does not
support Yarn Berry's `pkg@npm:^range` descriptor-matching syntax.

## pnpm (`pnpm.overrides` field in `package.json`, or a separate config)

```json
"pnpm": {
  "overrides": {
    "lodash": "^4.18.1",
    "foo>bar>lodash": "^4.18.1"
  }
}
```

pnpm supports the same blanket top-level form, plus a `>`-delimited chain-scoping syntax
(`parent>child>package`) analogous to Yarn Classic's slash-path form.

## Quick reference: which override should I write?

| Situation | What to write |
|---|---|
| Every consumer of `pkg` in the tree should get the same safe version, and `pkg`'s API is stable enough that this can't break anything | Blanket key (`"pkg"`) |
| Only one specific dependent's copy is vulnerable; others already resolve fine on their own | Scoped-by-range (Berry: `"pkg@npm:^range"`) or scoped-by-parent (npm/pnpm: nested/chain form) |
| …but the project is also scanned by **Snyk**, and the manager is **Yarn Berry** | Blanket key — the scoped key makes `snyk test` fail to scan at all. Record the intended consumer in the notes |
| Multiple major branches of `pkg` coexist and need *different* target versions (e.g. one consumer needs 6.x fixed, another needs 8.x fixed) | Multiple scoped keys, one per branch — never a single blanket key here |
| You're not sure which dependent's request a given key actually matches | Check with `yarn why <pkg>` / `npm ls <pkg>` *before* writing the key, not after |
