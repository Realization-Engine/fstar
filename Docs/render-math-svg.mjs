import fs from 'node:fs/promises';
import path from 'node:path';
import {mathjax} from 'mathjax-full/js/mathjax.js';
import {TeX} from 'mathjax-full/js/input/tex.js';
import {SVG} from 'mathjax-full/js/output/svg.js';
import {liteAdaptor} from 'mathjax-full/js/adaptors/liteAdaptor.js';
import {RegisterHTMLHandler} from 'mathjax-full/js/handlers/html.js';
import {AllPackages} from 'mathjax-full/js/input/tex/AllPackages.js';

const documentPath = path.resolve('The_Multiplier_and_the_Mirror.md');
const assetsDir = path.resolve('math');

const adaptor = liteAdaptor();
RegisterHTMLHandler(adaptor);

const tex = new TeX({ packages: AllPackages });
const svg = new SVG({ fontCache: 'local' });
const html = mathjax.document('', { InputJax: tex, OutputJax: svg });

const renderSvg = (expression) => {
  const node = html.convert(expression, {
    display: true,
    em: 16,
    ex: 8,
    containerWidth: 120 * 16,
  });

  const svgNode = adaptor.firstChild(node);
  let svgMarkup = adaptor.outerHTML(svgNode);

  if (!svgMarkup.includes('xmlns="http://www.w3.org/2000/svg"')) {
    svgMarkup = svgMarkup.replace('<svg ', '<svg xmlns="http://www.w3.org/2000/svg" ');
  }

  const vbMatch = svgMarkup.match(/viewBox="([^"]+)"/);
  const widthMatch = svgMarkup.match(/width="([\d.]+)ex"/);
  const heightMatch = svgMarkup.match(/height="([\d.]+)ex"/);
  if (vbMatch && widthMatch && heightMatch) {
    const [minX, minY, vbW, vbH] = vbMatch[1].split(/\s+/).map(Number);
    const widthEx = parseFloat(widthMatch[1]);
    const heightEx = parseFloat(heightMatch[1]);
    const pxScale = vbW / (widthEx * 8);
    const margin = 3 * pxScale;
    const nMinX = +(minX - margin).toFixed(1);
    const nMinY = +(minY - margin).toFixed(1);
    const nW = +(vbW + 2 * margin).toFixed(1);
    const nH = +(vbH + 2 * margin).toFixed(1);
    const nWidthEx = +(widthEx * nW / vbW).toFixed(3);
    const nHeightEx = +(heightEx * nH / vbH).toFixed(3);
    svgMarkup = svgMarkup.replace(/viewBox="[^"]+"/, `viewBox="${nMinX} ${nMinY} ${nW} ${nH}"`);
    svgMarkup = svgMarkup.replace(/width="[\d.]+ex"/, `width="${nWidthEx}ex"`);
    svgMarkup = svgMarkup.replace(/height="[\d.]+ex"/, `height="${nHeightEx}ex"`);
    svgMarkup = svgMarkup.replace(/<rect id="equation-background"[^/]*\/>/, '');
    const rectEl = `<rect id="equation-background" x="${nMinX}" y="${nMinY}" width="${nW}" height="${nH}" fill="white" />`;
    svgMarkup = svgMarkup.replace(/<svg\b([^>]*)>/, `<svg$1>${rectEl}`);
  }

  return svgMarkup;
};

const escapeAltText = (value) => value.replace(/\s+/g, ' ').trim().replace(/\[/g, '\\[').replace(/\]/g, '\\]');

const main = async () => {
  const original = await fs.readFile(documentPath, 'utf8');
  const displayBlockPattern = /^\$\$(.+?)\$\$$/gm;
  const commentPattern = /^<!-- \$\$?(.+?)\$\$? -->$/gm;
  const matches = [...original.matchAll(displayBlockPattern)];
  const commentMatches = [...original.matchAll(commentPattern)];

  if (matches.length === 0 && commentMatches.length === 0) {
    console.log('No display math source found.');
    return;
  }

  await fs.mkdir(assetsDir, { recursive: true });

  if (matches.length === 0) {
    for (let index = 0; index < commentMatches.length; index += 1) {
      const expression = commentMatches[index][1].trim();
      const assetName = `display-${String(index + 1).padStart(2, '0')}.svg`;
      const assetPath = path.join(assetsDir, assetName);
      const svgMarkup = renderSvg(expression);

      await fs.writeFile(assetPath, svgMarkup, 'utf8');
    }

    console.log(`Regenerated ${commentMatches.length} SVG assets from preserved equation comments.`);
    return;
  }

  let updated = original;

  for (let index = 0; index < matches.length; index += 1) {
    const fullMatch = matches[index][0];
    const expression = matches[index][1].trim();
    const assetName = `display-${String(index + 1).padStart(2, '0')}.svg`;
    const assetPath = path.join(assetsDir, assetName);
    const relativeAssetPath = `math/${assetName}`;
    const svgMarkup = renderSvg(expression);
    const altText = escapeAltText(expression);
    const replacement = `![${altText}](${relativeAssetPath})\n<!-- $$${expression}$$ -->`;

    await fs.writeFile(assetPath, svgMarkup, 'utf8');
    updated = updated.replace(fullMatch, replacement);
  }

  await fs.writeFile(documentPath, updated, 'utf8');
  console.log(`Generated ${matches.length} SVG assets and updated markdown.`);
};

main().catch((error) => {
  console.error(error);
  process.exitCode = 1;
});
