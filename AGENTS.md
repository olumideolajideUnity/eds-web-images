# Instructions for AI agents: updating foundations.unity.com images

You are updating the Editor screenshots on **https://www.foundations.unity.com** (the Editor Foundations Design System site) with the
retaken images in this repository. Follow these steps exactly. Do not change any page content other than the images
listed in `manifest.json`.

## 1. Get the latest images

- Clone or pull `https://github.com/olumideolajideUnity/eds-web-images` (branch `main`).
- Read `manifest.json`. It is the only source of truth; don't infer images from folder names.
- If you have applied this repo before, note the commit you applied last time and only process what changed since:
  `git diff --name-status <last-applied-commit>..HEAD -- images/ manifest.json`. Record the new `HEAD` commit in your
  report so the next run can do the same.

## 2. Choose the variant

- Use `"macos-dark"` (the manifest's `default_variant`) unless the person you're working for asks for another.
  It matches the site's current look (macOS window frames, dark theme).
- Each image appears once per variant in `images`. Filter on `variant` first, then process one row per `id`.

## 3. For each image row

Fields you'll use: `page_url`, `section_heading`, `position_on_page`, `original_image_url`, `original_file`,
`original_size`, `replacement_1x` / `replacement_2x` (repo paths), `replacement_1x_url` / `replacement_2x_url` (direct
download links), `replacement_*_size`, `replacement_*_sha256`, `size_changed`, `alt_text`, `caption`, `notes`.

1. **Find the image on the page.** Open `page_url`. The image to replace is the `<img>` whose `src` or `srcset`
   contains `original_image_url` (match on the file name after the last `/`, URL-decoded, if the host or ID prefix
   differs). If no element matches, fall back to the content image under `section_heading` at `position_on_page`
   (content images counted from the top of the page, ignoring icons and navigation cards). If you still can't find it
   with confidence, **skip it and report it**; never guess.
2. **Download the new files** from `replacement_2x_url` and `replacement_1x_url` (or the repo paths). Check each
   file's SHA-256 against `replacement_2x_sha256` / `replacement_1x_sha256`.
3. **Replace the image.**
   - The site is built on Webflow (assets are served from `cdn.prod.website-files.com`). The safest way to swap an
     image everywhere it's used is to **replace the existing asset** with the new file (Webflow Designer: Assets panel,
     the image's settings, *Replace image*). If you're working through the Webflow Data API or an MCP server, upload
     the new file as an asset to the same site and point the image element (or CMS image field) at it.
   - Upload the **@2x file** and display it at the **1x width** (`replacement_1x_size`), so it stays sharp on
     high-density screens. If the platform supports `srcset`, use the 1x file as `1x` and the @2x file as `2x`.
   - If `size_changed` is `true`, the new image's aspect ratio differs from the old one (the note explains why);
     update any fixed width/height on the element so it isn't stretched or cropped.
   - Keep the existing **alt text and caption** (`alt_text`, `caption`) unless told otherwise.
   - Don't delete the original asset until the page has been checked.
4. **Check the result.** Reload the published page (or the staging preview), confirm the new image loads, is the right
   one for that section, isn't distorted, and the layout around it hasn't shifted.

## 4. Things you must not do

- Don't edit page text, headings, captions or alt text on your own. Some `notes` contain **FINDING** lines: they
  describe where the page's text no longer matches Unity 7 (for example a token value or a size the page quotes).
  Collect these and pass them to the content owners in your report instead of acting on them.
- Don't replace images that aren't in `manifest.json`. Images not listed are either not Editor screenshots or haven't
  been retaken yet (see `pages` in the manifest for progress per page).
- Don't publish the site without approval if the person you're working for hasn't asked you to; stage the changes and
  report.

## 5. Report back

Give a short report with:
- the repo commit you applied and the variant you used;
- a table of images replaced (page, section, original file);
- any images skipped and why;
- the FINDING notes, grouped by page, for the content owners;
- pages from `pages` where `complete` is `false` (more images will arrive for those).
