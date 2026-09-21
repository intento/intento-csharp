---
name: dependency-security-remediation
description: Use whenever the user wants to find, fix, or clean up security vulnerabilities in a project's dependencies, in any language — phrases like "run a security audit", "check for vulnerabilities", "fix the audit findings", "address these Dependabot alerts", "get Snyk to zero", "update vulnerable packages", "patch this CVE", or any request touching dependency security in a manifest or lockfile (package.json, yarn.lock, pyproject.toml, requirements.txt, poetry.lock, go.mod, go.sum, *.csproj, packages.lock.json, and equivalents). Also trigger when the user asks to reduce or explain a block of version overrides — resolutions, overrides, constraints, replace directives, dependencyManagement — asks whether entries in it are still needed, wants a Snyk or Dependabot report brought to zero, or wants dependencies updated "for security" as opposed to general modernization. The method here is ecosystem-agnostic; per-ecosystem mechanics live in references/ecosystems/.
metadata:
  version: "1.1.0"
---

# Dependency Security Remediation

Fixing dependency CVEs is easy to do badly: pin every flagged package with a forced version
override and call it done. That accumulates silent debt (nobody remembers why half of them are
there six months later) and it skips the step that actually catches regressions — running the
project's own build. Prefer fixing at the source over patching around it, and never trust a
clean audit report on its own.

**This file is the method.** The detail sits in reference files, read on demand:

| Read | When |
|---|---|
| `references/scanners.md` | Phase 1 — which scanner sees what, commands per ecosystem, how to read the output |
| `references/ecosystems/<ecosystem>.md` | Phase 2 — how to re-resolve and how (or whether) to force a version |
| `references/build-verification.md` | Phase 4 — how bumps break toolchains, and proving what changed in a shipped artifact |
| `references/exceptions.md` | When a finding will not be fixed, or an override survives |

Resolve those paths from this skill's directory — `$CLAUDE_PLUGIN_ROOT/skills/dependency-security-remediation/`
when installed as a plugin, or the base directory announced when the skill loaded. A bare
relative path will not resolve from an arbitrary working directory.

**The method is ecosystem-agnostic.** The priority order, the verification and the scope
discipline are the same whether the manifest is `package.json`, `pyproject.toml`, `go.mod` or a
`.csproj`. Only the mechanics differ. Concrete examples below are labelled — read them as
instances of a class, not as the ecosystem this skill is about.

## Phase 0 — Identify what you are actually remediating

Find the manifests before running anything. This decides which scans apply and which fixes are
even available — and it is where "this is a JavaScript repo" gets assumed silently and wrongly.

```bash
find . -maxdepth 3 \( -name node_modules -o -name .git -o -name vendor \) -prune -o \
  \( -name 'package.json' -o -name 'pyproject.toml' -o -name 'requirements*.txt' \
     -o -name 'Pipfile' -o -name 'go.mod' -o -name '*.csproj' -o -name '*.sln' \
     -o -name 'Directory.Packages.props' -o -name 'pom.xml' -o -name 'build.gradle*' \
     -o -name 'Gemfile' -o -name 'Cargo.toml' \) -print
```

| Manifest | Ecosystem | Lockfile(s) to expect |
|---|---|---|
| `package.json` | JavaScript / TypeScript | `yarn.lock`, `package-lock.json`, `pnpm-lock.yaml` |
| `pyproject.toml`, `requirements*.txt`, `Pipfile` | Python | `poetry.lock`, `uv.lock`, `Pipfile.lock`, a pinned `requirements.txt` |
| `go.mod` | Go | `go.sum` |
| `*.csproj`, `*.sln`, `Directory.Packages.props` | .NET / C# | `packages.lock.json` (only if opted in) |

**A repo is often several of these at once** — the common case for a service with a bundled
frontend. Each needs its own scan *and* its own fix path; a clean JS audit says nothing about
the Python service beside it. Enumerate them all, and name which you covered when you report.

**Note the layout, not just the language.** Workspaces, Python dependency groups, Go
multi-module repos and .NET central package management all mean the file you edit is not the
file that resolves. If a lockfile sits above the manifest you are looking at, you are in a
workspace and the fix belongs at the root.

## Phase 1 — Gather signal from every source, then triage

**At Intento the Snyk CLI is the source of truth.** Start there and treat it as the gate:
`snyk test --dev --all-projects` clean is what "done" means and the run you quote. That is org
policy about which result is authoritative, not a claim that Snyk sees the most.

It cannot be read as seeing the most, because **no scanner is a superset of another** — the
GitHub Advisory Database and Snyk's own each carry advisories the other lacks, in both
directions. So run both: Snyk is the **gate**, the GitHub-DB scanners are **additional
finders**. A finding only they report is still real and still gets fixed; it just is not what
the release is judged on. Say which scanner found what, so nobody reads "clean" as broader than
the run behind it.

Two traps that make a scan lie, both defaulting to the reassuring answer: Snyk excludes
devDependencies without `--dev`, and scans only the root manifest without `--all-projects`. Per
ecosystem commands, the deprecation-vs-advisory filter, and how to read each tool's output are
in `references/scanners.md` — **read it before parsing anything**.

Before counting a finding, filter out deprecation notices. Most audit output is them, and a
deprecation is a hygiene issue, not a security one — and because they share a channel with real
advisories, **assert on the parsed entry list, never on exit status**. `yarn npm audit --all`
exits 1 on a clean tree by design, and the tempting fix (drop `--all`) silently stops scanning
devDependencies, where build-tooling advisories live.

**Dependency scanning is not the whole of "security findings".** If the task is "clear what the
tool reports" rather than "fix these CVEs", check what else the tool does — SAST, code scanning
and secret scanning are separate products, frequently disabled. Run them or say which you did
not.

## Phase 2 — Fix in this priority order

Work down the list per finding and stop at the first rung that applies. The first two cost
nothing and need no explanation later; most people skip straight to the last one.

**0. Refresh the lockfile entry.** A lockfile carries forward whatever version was resolved when
the entry was written, even when the declared range would allow a patched one today. So a large
share of "vulnerable transitive dependency" findings are stale lock entries, fixable with no
manifest change and no override — true of every ecosystem that resolves a range then records
the result.

Confirm first that the fix is inside the requester's existing declared range; if it is not, this
rung cannot help and you belong at rung 2 or 3. Then re-resolve, reinstall, re-scan. The command
is ecosystem-specific and several have traps — see `references/ecosystems/<ecosystem>.md` before
running one.

**Drive this from the finding list, one flagged package at a time.** A bulk refresh is not a
substitute and will quietly miss things: on one repo a change re-resolved 286 packages without
picking up `@ungap/structured-clone`, whose declared range had admitted the fix all along — a
targeted re-resolution of that one package got it immediately. The payoff is routinely large because nothing else changes: on one JS repo this
step alone closed four of six advisories; on another it took 127 advisories to 8 before a single
range was edited.

**1. Is the vulnerable package even used? Delete it.** A direct dev dependency that nothing
invokes is the cheapest possible fix, and it removes the finding permanently rather than moving
it. Grep the repo for the package *and its binary name*, and check CI configs and shell scripts,
not just the manifest. If the only hits are the manifest and the lockfile, nothing uses it. On
one repo `@babel/cli` had no script, no config and no CI reference — it existed only to drag in
`glob@7` and an unfixable `inflight`, and removing it closed a finding no override could have.

**2. Bump the direct dependency**, if the vulnerable package is itself in the manifest. Check
the newest version within the current major before considering a major jump. Nothing to
maintain, no explanation needed later.

**3. Bump the parent that pulls it in.** Ask whether some direct dependency has a newer release
that already requires a patched version, and check the range that candidate *declares* rather
than guessing. Build tooling gets its own dependencies fixed upstream constantly, faster than a
project remembers to check.

**Try this even when a note in the repo says it won't work** — those notes rot. One repo carried
a documented `ip-address` override reading "confirmed even socks's own latest release still
requests `^9.0.5`"; `socks@latest` had since moved to `^10.1.1`, and re-resolving retired the
override outright.

**Compare the ranges, not the version numbers.** The question is not whether the newer parent
declares something `>=` the fix, but whether its range is *better or worse than the one you
have*. A release can tighten a caret into an exact pin and drop you below a fix: a library
declaring `axios: "^1.7.9"` resolves to the newest 1.x, but its next release declaring
`axios: "1.12.2"` silently reintroduces everything fixed after 1.12.2. Same distrust as the
paragraph above, pointed the other way — so re-scan after taking *any* parent bump, including
ones somebody else raised.

An abandoned parent (no release in years) is a sign to look for its maintained fork rather than
an override (`yarn-run-all` → `npm-run-all2`). Check the fork's `engines` against the project's
declared floor and CI's runtime — a maintained fork often raises its floor faster than the
project does.

**4. Only if none of those work, force the version.** Legitimate when the fix landed in a major
the requester's range can't cross, when a pin's ceiling sits below the patched version, or when
the parent is unmaintained with no newer release.

Ecosystems are not equally capable here: JS has `resolutions`/`overrides`, .NET central package
management, Go `replace`, and Python no true transitive override at all — only install-time
constraints. **Check what your ecosystem can express before promising a fix at this rung.**
Where it cannot, the honest options are rung 3, the reachability argument below, or an accepted
finding. Syntax, scoping and per-ecosystem limits are in `references/ecosystems/<ecosystem>.md`,
along with a JS-specific trap worth knowing before you scope a key narrowly: a scoped Yarn Berry
resolution makes `snyk test` abort the entire scan.

Scope it as narrowly as the syntax allows, and expect the generic failure shape — a mis-keyed
override does nothing *quietly* rather than erroring, so the finding stays and you think you
handled it. An override that survives needs a note; see `references/exceptions.md`.

**A secondary option, when the only override would mean a large or awkward jump:** check whether
the vulnerable code path is *reachable* the way this project uses the package. If it provably is
not, leaving the version and documenting "verified unreachable, risk accepted" is legitimate —
but treat it as the exception, since it rests on an assumption (a config option, a call pattern)
that can silently stop holding, where an override does not. Worth noting when you find
unreachability: it is evidence the dependency is doing nothing for you, which often points back
at rung 1.

## Phase 3 — Verify empirically, not from semver math alone

Reasoning "the parent declares `^4.17.21` and the fix is `4.18.0`, so a caret reaches it" is
sometimes wrong in practice, for the rung 0 reason. The only way to know:

1. Make the candidate change (remove an override, bump a parent).
2. Reinstall, regenerating the affected part of the lockfile.
3. Re-run every scanner — with Snyk as the one that decides.

**Trim the override block in the same round you add to it, not as separate housekeeping.** A new
override changes what the old ones hold up, so entries load-bearing an hour ago can be dead now.
Empty the block, reinstall, re-scan: whatever comes back is genuinely needed. Do this even when
every entry has a convincing note. On one repo, forcing `pacote` from 15 to 21 pulled in a whole
modern tooling subtree, which made a documented `sigstore` override dead weight and exposed a
chain whose re-resolution retired the `tar` override too: three entries became one, and the
survivor was not among the original three. Expect the experiment to look alarming — that repo
went from 1 finding to 15 — and read it as a map of what each entry covers, not a reason to
restore them.

**Some entries will be worse than dead weight: they will be the finding.** An override is a
ceiling as well as a floor, so an entry written to force a package *up* to a fix will later hold
it *down*, below fixes released after it. One thirteen-entry block contained two: `tar: ^6.2.1`,
capping tar below the 7.5.21 fix its parent would have reached unaided, and `minimatch: ^5.1.6`,
pinning minimatch at a version with an open advisory. Both had been added to protect the project
and had become the reason it was exposed. An attached note is no defence — an actively harmful
entry reads exactly like a merely redundant one.

That overrides rot this way is the durability argument for the whole order: a declared range
absorbs the *next* advisory too, silently and for free, while an override freezes a version and
must be revisited by hand. One project re-audited three weeks after a rung-0-and-rung-3 fix and
needed no action at all — five packages had gained advisories, and the ranges had already
resolved past every one.

## Phase 4 — Run the full pipeline before calling anything done

A clean scan is necessary, not sufficient. Run everything that could be affected: typecheck,
lint, the real build (dev *and* prod if they differ), tests, and any secondary pipeline sharing
the same dependency tree. These are where a bump breaks something a diff would never show.

**Run that pipeline green *before* you change a single dependency.** It costs one pass and it is
the only thing that lets you say a later failure is yours. Without it every breakage costs a
bisect to establish something you could have had for free — and some of what you find will turn
out to predate you, which you will then be tempted to fix inside a security changeset.

**Compare the test count, not just the exit code.** A changed test runner can leave suites
failing to load in ways that are easy to skim past; "55 suites, 2393 tests" before and after is
the assertion that nothing was silently skipped. Which is a second reason to capture the
baseline first — the count is only evidence if you wrote it down beforehand.

**Clear the caches before believing any result that is supposed to be a failure.** Bisecting and
mutation checks both rest on "I broke it and the suite noticed", and a warm cache removes that
guarantee in the reassuring direction — on one repo, restoring a call that genuinely cannot work
passed 55 suites warm and threw immediately cold.

**Check what the work added to the publishable surface before releasing.** Scanners and test runs
leave files behind, and a denylist-based publish config ships each new one by default — `snyk code
test` writes a `.dccache` keyed by absolute path, which one repo published along with its coverage
directory and internal notes.

The specific ways toolchains break under a bump, how to bisect them without fooling yourself, how
to prove what the fix did to a shipped artifact, and the `npm pack --dry-run` diff are all in
`references/build-verification.md`. Read it if anything fails, before claiming a dev-only fix
cannot reach production, and before publishing.

## Phase 5 — Keep the security changeset's scope honest

Verification surfaces things that aren't the vulnerability you set out to fix: a linter major
enabling new rules on pre-existing code, a script that was already broken, a config gap that
"worked" only by lucky hoisting. Fix what is genuinely part of making the bump land safely — a
config incompatibility the bump caused, a peer it newly requires. For everything else, don't
fold silent fixes into the same changeset. Ask, or file it separately, so the changeset stays
reviewable as what it claims to be.

Say which is which when you report: this change *introduced* that incompatibility and had to fix
it; this change merely *exposed* that pre-existing breakage, filed elsewhere.

For findings you will not fix and overrides that survive — accepting versus suppressing, and the
notes both require — see `references/exceptions.md`.
