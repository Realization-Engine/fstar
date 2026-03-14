# Math Rendering for Markdown — Technique Reference

This document explains how display-mode LaTeX equations in
`The_Multiplier_and_the_Mirror.md` are rendered as SVG images so they display
correctly on GitHub and in offline Markdown viewers.

---

## The Problem

GitHub's Markdown renderer supports **inline** LaTeX (`$...$`) natively, but
**display-mode** equations (`$$...$$`) render inconsistently — they sometimes
break inside tables, after blockquotes, or when they contain complex
expressions (cases environments, `\frac`, stacked sub/superscripts, etc.).

Many desktop Markdown editors (VS Code, Visual Studio, Markdown Monster) have
partial or no LaTeX support. Pre-rendering display equations to SVG guarantees
they render identically everywhere images are supported.

---

## Two-Channel Strategy

The article uses two parallel rendering strategies:

| Channel | Syntax in Markdown | Rendered by | Count |
|---------|-------------------|-------------|-------|
| **Display equations** | `![alt text](math/display-NN.svg)` | Pre-rendered SVG image | ~50 |
| **Inline math** | `$M_p$`, `$F^*$`, `$\alpha$`, etc. | GitHub's native KaTeX renderer | ~200+ |

**Why not SVG everything?** Inline expressions appear mid-sentence. Replacing
each with an `<img>` tag would destroy readability in source, break text flow,
and produce ~200 additional files. Native `$...$` degrades gracefully — even
when a viewer can't render it, `$M_p$` is perfectly readable as raw LaTeX.

**Why not native `$$` everything?** GitHub's `$$` rendering is unreliable for
complex expressions and doesn't work in all block contexts. SVG images are
universally supported and pixel-identical across all viewers.

---

## File Structure

```
Docs/
├── The_Multiplier_and_the_Mirror.md   # The article (source of truth)
├── render-math-svg.mjs                # Node.js render script
├── README.md                          # This file
├── node_modules/                      # npm dependencies (not committed)
└── math/
    ├── display-01.svg                 # Equation (1)
    ├── display-02.svg                 # Equation (1a)
    ├── ...
    └── display-50.svg                 # Equation (32)
```

---

## How It Works

### The Render Script (`render-math-svg.mjs`)

A Node.js ESM script that uses **MathJax Full** (`mathjax-full`) to convert
LaTeX expressions to SVG server-side. It operates in two modes:

#### Mode 1 — First Run (convert `$$` blocks to SVG)

If the Markdown contains raw `$$...$$` display blocks, the script:

1. Finds every `$$...$$` block in the document.
2. Renders each to SVG via MathJax.
3. Writes the SVG to `math/display-NN.svg`.
4. Replaces the `$$...$$` block in the Markdown with:
   ```markdown
   ![escaped LaTeX as alt text](math/display-NN.svg)
   <!-- $$original LaTeX$$ -->
   ```

The HTML comment preserves the original LaTeX source for future regeneration.

#### Mode 2 — Regeneration (rebuild SVGs from comments)

If no `$$...$$` blocks remain (they've already been converted), the script
finds the `<!-- $$...$$ -->` comments and regenerates all SVG files from them.
This is the normal operating mode after first run.

### MathJax Configuration

```javascript
const tex = new TeX({ packages: AllPackages });
const svg = new SVG({ fontCache: 'local' });

html.convert(expression, {
  display: true,
  em: 16,
  ex: 8,
  containerWidth: 120 * 16,  // 1920px
});
```

- **`AllPackages`** — loads every TeX extension (ams, cases, boldsymbol, etc.)
- **`fontCache: 'local'`** — embeds font glyph `<path>` definitions inside
  each SVG, making them self-contained
- **`display: true`** — renders in display mode (centered, larger)
- **`em: 16, ex: 8`** — standard browser font metrics

### SVG Post-Processing

MathJax produces SVGs with dimensions in `ex` units and a `viewBox` whose
y-origin is typically **negative** (math content extends above the baseline).
For example:

```
viewBox="0 -950 17905.7 2142.2"
```

This means the coordinate space starts at y = −950 and extends 2142.2 units
tall. The script applies two post-processing steps:

#### 1. White Background Rectangle

A `<rect>` element is inserted as the first child of the `<svg>`, with
explicit coordinates matching the viewBox:

```xml
<rect id="equation-background"
      x="0" y="-950"
      width="17905.7" height="2142.2"
      fill="white" />
```

**Why explicit coordinates?** A naive `width="100%" height="100%"` rect
defaults to position `(0, 0)`, which misses everything above the baseline
(negative y-space). MathJax equations routinely have content above y = 0, so
the rect must use the viewBox's actual `minX` and `minY`.

#### 2. 3-Pixel Margin

The viewBox is expanded by 3 pixels on every side, and the display dimensions
are scaled proportionally:

```javascript
// Convert 3 CSS pixels to viewBox units
const pxScale = vbW / (widthEx * 8);   // viewBox units per pixel
const margin = 3 * pxScale;            // 3px in viewBox units

// Expand viewBox
const nMinX = minX - margin;
const nMinY = minY - margin;
const nW    = vbW + 2 * margin;
const nH    = vbH + 2 * margin;

// Scale display dimensions proportionally
const nWidthEx  = widthEx * nW / vbW;
const nHeightEx = heightEx * nH / vbH;
```

**Why `vbW / (widthEx * 8)`?** MathJax uses `ex` as its display unit, and we
configured `ex = 8` pixels. So `widthEx * 8` gives the display width in
pixels, and `vbW` is the same width in viewBox units. The ratio is the
conversion factor — approximately **55.25 viewBox units per CSS pixel**,
consistent across all MathJax output.

The background rect is then sized to fill the expanded viewBox, ensuring the
white padding extends to the edges.

### Resulting SVG Structure

```xml
<svg xmlns="http://www.w3.org/2000/svg"
     width="41.261ex" height="5.597ex"
     viewBox="-165.7 -1115.7 18237.2 2473.7"
     role="img" focusable="false">
  <rect id="equation-background"
        x="-165.7" y="-1115.7"
        width="18237.2" height="2473.7"
        fill="white" />
  <defs><!-- MathJax glyph paths --></defs>
  <g><!-- MathJax rendered equation --></g>
</svg>
```

---

## How to Use

### Prerequisites

- **Node.js** (v18+ recommended)
- **npm**

### Install Dependencies

```bash
cd Docs
npm install mathjax-full
```

There is no `package.json`; the single dependency is installed directly. If you
want to create one:

```bash
cd Docs
npm init -y
npm install mathjax-full
```

### Regenerate All SVGs

To rebuild every SVG from the LaTeX sources preserved in HTML comments:

```bash
cd Docs
node render-math-svg.mjs
```

Output: `Regenerated 50 SVG assets from preserved equation comments.`

This reads the `<!-- $$...$$ -->` comments in the Markdown and overwrites every
file in `math/`.

### Add a New Equation

1. Add the equation as a `$$...$$` block in the Markdown at the desired
   location:
   ```markdown
   Some text above.

   $$E = mc^2$$

   Some text below.
   ```

2. Run the script:
   ```bash
   node render-math-svg.mjs
   ```

3. The script replaces the `$$...$$` block with:
   ```markdown
   ![E = mc^2](math/display-51.svg)
   <!-- $$E = mc^2$$ -->
   ```

> **⚠ Important:** The script numbers SVGs sequentially based on the order of
> `$$` blocks found. If you add a new equation in the middle, it will
> renumber everything. In that case, it's safer to add new equations at the
> end, or manually create the SVG reference and comment, then use regeneration
> mode to rebuild the SVGs.

### Edit an Existing Equation

1. Edit the LaTeX inside the `<!-- $$...$$ -->` comment.
2. Run `node render-math-svg.mjs` to regenerate.

The SVG filenames are stable as long as the order of comments doesn't change.

---

## Inline Math (`$...$`)

Inline math is **not** handled by this script. It relies on GitHub's native
LaTeX renderer, which uses KaTeX under the hood.

**Behavior by viewer:**

| Viewer | Inline `$...$` rendering |
|--------|--------------------------|
| GitHub.com | ✅ Full KaTeX rendering, theme-aware |
| Markdown Monster | ✅ Renders via built-in MathJax |
| VS Code (Markdown Preview) | ✅ With math extension |
| Visual Studio 2026 | ❌ Displays raw LaTeX (e.g., `$M_p$`) |

The raw LaTeX is intentionally readable — `$M_p$` is clear enough even
unrendered. This was a deliberate trade-off: universal readability in source
over pixel-perfect rendering in every viewer.

---

## Troubleshooting

### SVGs have transparent background

The white `<rect>` may be missing or mispositioned. Regenerate:
```bash
node render-math-svg.mjs
```

### Equation content is clipped

The viewBox margin may have been lost. Regeneration restores the 3px margin
on all sides.

### Script says "No display math source found"

The Markdown has neither `$$...$$` blocks nor `<!-- $$...$$ -->` comments.
The script has nothing to process.

### New equation gets the wrong number

The script assigns numbers sequentially. If inserting between existing
equations, either:
- Add at the end and manually reorder the markdown references, or
- Manually write the `![alt](math/display-NN.svg)` + `<!-- $$...$$ -->`
  pattern and run regeneration mode.

---

## Key Technical Details

| Property | Value |
|----------|-------|
| MathJax package | `mathjax-full` (all TeX extensions) |
| Font caching | `local` (self-contained SVGs) |
| Display metrics | `em=16`, `ex=8`, `containerWidth=1920` |
| ViewBox scale | ~55.25 viewBox units per CSS pixel |
| Margin | 3 CSS pixels on all sides |
| Background | White `<rect>` matching full viewBox |
| Alt text | Escaped LaTeX (brackets escaped for Markdown) |
| Source preservation | `<!-- $$...$$ -->` HTML comments |
