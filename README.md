# Editor screenshots for foundations.unity.com

Replacement images for the Editor screenshots on [foundations.unity.com](https://www.foundations.unity.com), retaken in a live
Unity 7000.0.0a20 Editor so they match the current UI.

- **332 images** are ready, across 39 pages (32 of 39 reviewed pages are complete).
- More images are added as they're retaken; pull the repo to get the latest.
- Last updated: 2026-11-20T17:37:08.

## Using this repo

- **With an AI agent:** point the agent at this repo and ask it to follow [`AGENTS.md`](AGENTS.md). It explains how to
  find each image on the site, which file to upload, and how to report back.
- **By hand:** open [`manifest.csv`](manifest.csv) in a spreadsheet. Each row tells you the page, the section, the
  original image URL and the new files.
- **To retake or update images yourself:** follow [`CAPTURE.md`](CAPTURE.md). [`capture/`](capture/) holds the capture
  toolkit and one recipe per image, and works with whichever Unity Editor and project you have open.

## Layout

```
images/<page path>/<variant>/<original file name>.png      1x, same width as the image on the site today
images/<page path>/<variant>/<original file name>@2x.png   2x, for high-density screens (preferred upload)
manifest.json / manifest.csv                                one row per image and variant
CAPTURE.md, capture/                                        how to retake the images, and the toolkit and recipes to do it
```

- `<page path>` matches the page's URL path (`home` is the home page), for example `images/patterns/overlays/`.
- `<variant>` is the window frame and Editor theme: `macos-dark` (the site's current look), `windows-dark`,
  `macos-light` and `windows-light`.
- File names match the image they replace, without the CDN's ID prefix.
