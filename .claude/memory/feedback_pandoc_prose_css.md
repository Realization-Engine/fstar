---
name: Preserve prose-CSS in pandoc deploy workflow
description: In .github/workflows/deploy-pages.yml the `header-includes` inline `<style>` contains prose-spacing rules that MUST NOT be stripped when editing — without them the GitHub Pages docs render flush with no paragraph/list spacing.
type: feedback
---

When editing `.github/workflows/deploy-pages.yml`, preserve the full prose-CSS block inside the `--variable header-includes='<style>...</style>...'` argument of the `pandoc` invocation. The block must keep rules for: `body` (margin, max-width, line-height), `p` (bottom margin), `h1`–`h6` (top/bottom margin, line-height), `h1:first-child` (reset top margin), `ul, ol` (bottom margin + `padding-left: 2em`), `li`, `li > p`, nested `li > ul`/`li > ol`, `blockquote`, `hr`, and `pre.mermaid`.

**Why:** The pandoc standalone pages link to `src/FStarEquations.App/wwwroot/css/Site.css`, which opens with a universal reset: `*, *::before, *::after { margin: 0; padding: 0; }`. That wipes default spacing on every element. The Blazor app compensates with rules scoped to `.main-content` and `.nav-card`, but pandoc pages are not inside those wrappers, so nothing re-adds prose spacing. Without the injected `<style>`, paragraphs, ordered/unordered lists, and headings collapse flush left with no separation — exactly the rendering bug seen on the "What is Mirror" section of `The_Multiplier_and_the_Mirror.html` on GitHub Pages (2026-04-19).

**How to apply:**
- The fix lives in `header-includes`, not in `Site.css`, because touching `Site.css` would risk breaking the Blazor app that depends on the reset.
- `header-includes` is injected into `<head>` after the `<link>` to `Site.css`, so its element-selector rules override the reset by source order — specificity is equal, later wins.
- If you need to condense, reformat, or edit that YAML line, do not drop any of the prose selectors listed above. Minifying whitespace inside the `<style>` is fine; removing rules is not.
- If a future change adds another pandoc-standalone-docs pipeline that shares CSS with this global reset, inject the same prose defaults in its `header-includes` rather than weakening the reset.
