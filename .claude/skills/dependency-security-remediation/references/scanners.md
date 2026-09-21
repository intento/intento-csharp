# Scanners: what each one sees, and how to read its output

Read this at Phase 1, before you count anything as "all the findings".

## Snyk is the gate; the others are additional finders

**At Intento the Snyk CLI is the source of truth**, so `snyk test --dev` clean is what "done"
means and the Snyk run is what you quote when you report. That is a policy about which result
is authoritative — it is not a claim that Snyk sees the most, and the section below is why it
cannot be read that way. A finding only the GitHub-DB scanners report is still real and still
gets fixed; it just is not the release gate. Report which scanner found what, so nobody reads
"clean" as broader than the run behind it.

## No scanner is a superset of another

There are two advisory databases in common use and they do not agree:

- **GitHub Advisory Database** — read by `npm audit`, `yarn npm audit`, `pnpm audit`, and
  GitHub Dependabot. Running two of these adds nothing over running one; they return the
  same advisories.
- **Snyk's own database** — read by `snyk test`. It carries advisories GitHub has not
  published, and it lags or omits some GitHub has.

Observed on one real repo in a single pass: Snyk uniquely reported `js-yaml` and
`inflight`; Dependabot and `yarn npm audit` uniquely reported `@humanfs/node`; the two
agreed on `browserslist` and `pacote`. **If the project uses both, "zero findings" has to be
verified in each separately** — and the acceptance criteria you write should say so.

Also worth stating plainly when you report: **Dependabot alerts are not the same as "GitHub
is clean."** Code scanning and secret scanning are separate products and are often disabled.
Check rather than assume:

```bash
gh api repos/{owner}/{repo}/code-scanning/alerts       # 403 => Code Security not enabled
gh api repos/{owner}/{repo}/secret-scanning/alerts     # 404 => secret scanning disabled
```

**Dependabot scans the repo's default branch, not the branch you are working on.** Its open
count will not budge while you fix things on a topic branch, which is disconcerting the first
time you see the number you started with staring back after a successful remediation. Because
the count is meaningless there, compare *versions* instead — each alert's
`security_vulnerability.first_patched_version.identifier` against what your branch actually
resolves to — and confirm every alert is covered before claiming it is:

```bash
gh api repos/{owner}/{repo}/dependabot/alerts --paginate \
  -q '.[] | select(.state=="open") | "\(.security_advisory.severity)|\(.dependency.package.name)|\(.security_vulnerability.first_patched_version.identifier)"'
```

That comparison also answers "did anything land while I was away?" — a new advisory appears as
an alert demanding a *higher* version than the tree has.

## Commands

The gate, first — it covers every ecosystem, so on a multi-ecosystem repo it is the one tool
that can give a single answer:

| Scanner | Command | Verified |
|---|---|---|
| Snyk (all ecosystems) | `snyk test --dev --all-projects --json` | CLI 1.1293.1 |

`--all-projects` is what makes it find *every* manifest in the tree rather than just the one at
the root — essential given Phase 0, because an undetected project scans as clean and reports as
success. Confirm in the output that each manifest you found in Phase 0 actually appears.

Snyk **excludes devDependencies by default** — without `--dev` you get a falsely clean report on
a project whose findings are all build tooling, which is the common case.

Then the GitHub-Advisory-DB scanners, per ecosystem:

| Ecosystem | Command | Verified |
|---|---|---|
| JS — Yarn Berry (`yarn@2+`) | `yarn npm audit --all --recursive --json` (plain `yarn audit` doesn't exist in Berry) | yes, Yarn 4.9 |
| JS — Yarn Classic (`yarn@1`) | `yarn audit --json` | — |
| JS — npm | `npm audit --json` | — |
| JS — pnpm | `pnpm audit --json` | — |
| .NET | `dotnet list package --vulnerable --include-transitive --format json` | SDK 7.0.312 |
| Python | `pip-audit --format=json` | **not verified** |
| Go | `govulncheck -json ./...` | **not verified** |
| any, via GitHub | `gh api repos/{owner}/{repo}/dependabot/alerts?state=open --paginate` | yes |

`--all` in Yarn Berry includes devDependencies; `--recursive` includes transitive deps.

`dotnet list package --vulnerable` **omits transitive packages without `--include-transitive`**,
which is the same class of trap as Snyk's `--dev`: the default is the falsely reassuring one.
It also cannot be combined with `--outdated` or `--deprecated` — those are separate runs.

The Python and Go rows are the conventional tools but were **not run when this was written**, so
treat them as a starting point and confirm the flags before quoting a result. `govulncheck` is
additionally not a plain manifest scanner — it does reachability analysis from your code, so its
findings are a *subset* of what a manifest-level scan reports, and "govulncheck is clean" is a
different claim from "no vulnerable versions are present."

## `snyk test` is not `snyk code test`

They are different products and neither implies the other:

- **`snyk test`** — Snyk Open Source. Dependencies. This is what the table above is about.
- **`snyk code test`** — Snyk Code, a SAST engine over your own source. Separate findings,
  separate output format (SARIF), and it does **not** run as part of `snyk test`.

A remediation task phrased as "clear all the Snyk findings" means both. Run
`snyk code test --json` and triage it, or say explicitly that you only checked dependencies —
silently reporting "Snyk is green" after running only one of them is the easy mistake here.

Reading its SARIF: findings are in `runs[0].results`, rule metadata (title, CWE) in
`runs[0].tool.driver.rules` keyed by `ruleId`. SARIF `level` maps `note` = low, `warning` =
medium, `error` = high — a wall of `note` is usually noise, but triage per finding rather than
per count.

Expect a high false-positive rate on test files and on taint analysis: on one repo, 24 findings
were 12 cookies-without-`Secure` in jsdom tests, 5 "hardcoded secrets" that were fixtures named
`plainkey` and `per-request`, 2 path traversals in a developer CLI reading `process.argv`, and 5
postMessage findings of which 3 pointed at a listener that *did* validate the origin six lines
below the flagged line. **Two were real.** Open the flagged line every time — the cost of
triage is the point, and the two real ones were in shipped code while all 22 others were not.

## Output shapes that will trip you up

**Yarn Berry `--json` is NDJSON, not JSON.** One advisory per line, each wrapped in a
`{value, children}` envelope with Title-Case keys. `json.load()` on the whole file raises
`Extra data: line 2 column 1`. Parse line by line:

```python
rows = [json.loads(l) for l in open(path) if l.strip()]
for r in rows:
    pkg, c = r['value'], r['children']
    if c.get('URL'):                      # real advisory
        print(c['Severity'], pkg, c['Tree Versions'], c['URL'], c['Dependents'])
```

This differs from npm's shape, which is a single object with an `advisories` map.

**`snyk test --json` returns a different shape when it fails to scan.** A successful run has
`ok`, `uniqueCount`, `vulnerabilities`, `summary`. A failed one has only `{ok, error, path}` —
and `ok` is `false` in both cases, so a script that checks `ok` alone reports "vulnerabilities
found" when the truth is "the scan never ran". Always check for `error` first.

**Snyk's `vulnerabilities` array is one entry per path, not per advisory.** A repo showing
`uniqueCount: 5` had 18 array entries. Deduplicate on `(id, packageName, version)` to count
advisories, and collect `from` across entries to enumerate every path — you need all of them,
because closing one path leaves the finding open.

## Assert on the parsed entries, never on exit status

Every scanner here overloads its exit code, and each overloads it differently. Treat a non-zero
exit as "look at the output", never as "there are vulnerabilities", and write your acceptance
criteria against the parsed entry list.

**`yarn npm audit --all --recursive` exits 1 by design on a clean tree.** `--all` includes the
*deprecation* channel, which is separate from the advisory channel, so "0 advisories, 0
high/critical" and a non-zero exit are simultaneously true. One repo's five entries were four
deprecation notices (an eslint version-support notice, `glob` under `test-exclude`,
`whatwg-encoding` under `jsdom`, an unmaintained plugin) and one real advisory.

The trap this sets is worth naming, because the fix looks like a cleanup: **the obvious way to
make a permanently-red gate green is to drop `--all` — which silently removes devDependencies
from the scan**, where build-tooling advisories overwhelmingly live. A CI gate that checks `$?`
either stays red forever or gets "fixed" into uselessness. Parse `--json` and compare names.

The same discipline applies to Snyk for a different reason: `ok` is `false` both when the scan
found vulnerabilities and when it failed to run (see the previous section).

So when you record a verification command in a repo's docs, record **the expected residual
entry list beside it**, not the expected exit code. A later round then diffs names — which
tells it that a new advisory arrived, or that a known deprecation moved — instead of learning
nothing from a red exit that was always red.

## Deprecations are not vulnerabilities

`yarn npm audit` and `npm audit` mix genuine CVEs in with deprecation notices ("no longer
supported", "use @scope/replacement instead"). A real advisory has a numeric/GHSA ID **and a
URL**; a deprecation's `ID` is a string like `"foo (deprecation)"` with no URL. Filter on the
presence of `URL`. One repo's 16 reported items were 4 advisories and 12 deprecations.

Do not report deprecations as vulnerabilities and do not spend remediation effort on them.
Two things are still worth noticing as you pass:

- A deprecation notice that **names a drop-in replacement** (`babel-eslint` →
  `@babel/eslint-parser`) is a cheap fix, especially if a `grep` shows the old package isn't
  even wired up.
- A deprecation notice that **names a CWE or tells you to upgrade for safety** ("Potential
  CWE-502 — Update to 1.3.1 or higher") is a security finding wearing a deprecation's
  clothes. Treat it as real even though it has no URL.

## Snyk Code ignores are not Snyk Open Source ignores

Everything in the section below about `.snyk` `ignore:` applies to `snyk test` (Open Source)
and **not** to `snyk code test`. Tested against CLI 1.1293.1:

| mechanism | works on Snyk Code? |
|---|---|
| `exclude:` → `code:` path globs in `.snyk` | **yes — the only one that does** |
| `// deepcode ignore <rule>: reason` above the flagged line | **no** — honoured by the IDE plugins, not by the CLI. Tried both the full SARIF ruleId (`javascript/InsufficientPostmessageValidation`) and the bare rule name; the finding simply moved down by the line the comment added |
| `.snyk` `ignore:` keyed on the Code rule id + path | **no** — that section is Open Source only, and fails silently |
| platform-side ignore in the Snyk web UI | yes, but it is an account action, not a repo change — you cannot do it from a checkout |

Two consequences:

- **A failed suppression is worse than the finding.** An inline `// deepcode ignore` that the
  scanner does not honour still reads to the next developer as "this was reviewed and
  suppressed". If a mechanism does not work, revert it rather than leaving it in place, and
  verify the revert (`git diff` on the file) rather than assuming.
- **Path globs are the only lever, so the granularity you want may not exist.** If the false
  positives sit in a file that also contains code worth scanning, the narrowest available
  suppression excludes the whole file. That is usually the wrong trade — see the next section.

```yaml
# .snyk — the only Snyk Code suppression that works
exclude:
  code:
    - __tests__/**        # say here what this hides and why, per glob
    - build-tools/**
```

## Verifying an exception is actually dismissed

This applies **only once you have decided to dismiss** rather than accept — see "Suppressing a
finding is not the same as accepting it" in SKILL.md, because leaving a low-severity finding
visible is frequently the better call. When you have decided to dismiss, writing the exception in
the repo is half the job; the scanner has to be told too, or it re-reports every run and people
learn to skim past it. For Snyk Open Source that's a `.snyk` policy file:

```yaml
version: v1.25.0
ignore:
  SNYK-JS-INFLIGHT-6095116:
    - copyfiles > glob > inflight:      # scope to the path, so a NEW path re-alerts
        reason: >-
          ... why it is not exploitable here, and who owns it ...
        expires: 2027-03-07T00:00:00.000Z
```

Then prove both halves, because a policy that silences a finding is indistinguishable from a
fix unless you check:

```bash
snyk test --dev                          # => ok: true, uniqueCount: 0
snyk test --dev --policy-path=/dev/null  # => shows exactly the finding you excepted, and nothing else
```

The second command is the one that stops a policy file from quietly hiding a finding nobody
meant to accept.
