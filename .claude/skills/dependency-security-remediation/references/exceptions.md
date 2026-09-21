# Findings you are not going to fix, and notes that outlive them

Read this when a scanner reports something that will not be fixed in this round, or when an
override survives to the end of Phase 2. Two decisions live here: whether to suppress a finding
or merely accept it, and how to write down what survives so the next person doesn't re-derive
it.

## Suppressing a finding is not the same as accepting it

When a scanner reports something you are not going to fix, there are two different endings, and
the quieter one is not automatically the better one:

- **Accept it.** Leave the finding reporting, write down that it was reviewed and why it is
  tolerable. The scanner stays noisy; the decision stays visible and is trivially reversible.
- **Suppress it.** Add an ignore or an exclusion. The scanner goes quiet — and stays quiet about
  anything else that lands in the same scope, for as long as the rule survives.

Suppression is a permanent, silent change to what you will be told in future. That is the cost,
and it is easy to miss because it is paid later and by someone else. An `exclude: __tests__/**`
does not hide 17 findings; it hides every finding in that directory from now on. So weigh it
against what the noise is actually costing:

- **If the findings are low-severity, understood, and not blocking anything** — the honest answer
  is often to leave them reporting and record the decision. This is especially true when the
  tool's own severity rating agrees they are minor. "We looked, they are all `note`, we accept
  them for now" is a complete and defensible outcome, and it costs nothing to revisit.
- **If the noise is actively preventing the tool from being used** — failing CI, burying real
  findings in a wall of chaff, training people to skim past the report — then suppress, scope it
  as narrowly as the tool allows, and document each rule with what it hides.
- **Never suppress a finding you have not read.** The triage is the valuable part and it is not
  optional; suppressing by rule name or directory without opening the flagged lines is how a real
  finding gets filed under noise.
- **Never suppress a *real* finding to reach a round number.** If two of twenty-four are genuine,
  the answer is twenty-two suppressed or zero — not twenty-four.

Whichever you choose, write the triage down. It is the expensive half and it is identical either
way; the difference is only whether a rule also gets added. And if you suppress, re-run the
scanner afterwards to confirm the rule did what you think — see `references/scanners.md`, where
two of the three plausible-looking Snyk Code mechanisms turn out to do nothing at all.

## Documenting what survives

Two different things need writing down, and they have different homes.

**An override that survives to the end** (Phase 2 rung 4) needs a note saying *why*: the specific
advisory, what package actually requests the vulnerable range, why the cheaper rungs didn't work,
any non-obvious constraint on the pinned version (a Node engines floor, a scanner quirk), and what
would have to change for the entry to become removable. Put the note where the entry is, in
whatever the manifest format allows — a comment if it has them (`pyproject.toml`, `go.mod`,
`.csproj` all do), and for JSON manifests with no comment syntax, a sibling object keyed the
same way as the block it documents, one note per package, so each entry's reasoning sits next
to the version it explains:

```json
"_resolutionsNotes": { "pacote": "GHSA-… . Only requester is … . Removable once …" },
"resolutions":       { "pacote": "21.5.1" }
```

A single `"//resolutions"` string is fine for one or two entries and stops being readable once
several have unrelated reasons.

**A finding you are not going to fix** needs a written record either way: what it is, why it is
tolerable here, a **named owner** and a **review date**. Whether it *also* gets a scanner
dismissal is the judgement in the previous section — accept-and-leave-reporting for low-severity
findings that block nothing, dismiss when the noise is stopping the tool being used.

If you do dismiss it, the two halves have to match. A scanner dismissal with no written reasoning
is indistinguishable from a fix; written reasoning with no dismissal gets re-reported every run
until people skim past the whole report. So write both, scope the dismissal to the specific path
so a *new* path re-alerts, give it an expiry that matches the review date, and then verify it:
`references/scanners.md` has the `.snyk` policy format and the two commands that prove a dismissal
is hiding exactly what you meant and nothing else.

Pair every such exception with a follow-up that would retire it — the exception explains why
waiting is safe, the follow-up is the thing that makes waiting end.

**Both kinds of note rot.** They describe the tree on the day they were written, and the bump that
lands two rounds later turns them into confident lies — the `socks` example in Phase 2 rung 3 is
one that would have cost a real session an unnecessary override. So: re-read the existing notes at
the start of a round, and when a change touches an entry, give that entry its outcome in the same
change.
