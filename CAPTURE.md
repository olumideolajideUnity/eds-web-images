# Retaking the images

This guide is for anyone, person or AI agent, who wants to **regenerate the Editor screenshots and GIFs in this repo** from a
live Unity Editor. Typical reasons: a newer Unity version, a design change, or a fix to one image. To put the finished images
on foundations.unity.com, follow [AGENTS.md](AGENTS.md) instead.

Everything you need is in [`capture/`](capture/):

| Path | What it is |
|---|---|
| `capture/recipes.json` | One recipe per image: what it shows, the original, the setup scripts, the capture commands, the layout, the outputs |
| `capture/efds.sh` | Shell helpers that drive the open Editor through the Unity CLI and grab windows from Unity's own view buffers |
| `capture/unity/` | C# setup scripts. Each one puts the Editor in the state a shot needs (opens and sizes a window, selects an object, expands a component) |
| `capture/layouts/` | Layout specs: where each capture sits on the canvas, its window frame, and its callouts |
| `capture/scripts/compose.py` | Turns captures and a layout into the finished image: macOS and Windows frames, 1x and @2x, dark or light |
| `capture/scripts/pages/` | Per-page compose scripts, for images with measured callouts, splits or recolouring |
| `capture/scripts/gif_build.py`, `rec_build.py` | GIFs from Editor frames, or from a screen recording |
| `capture/fixtures/` | Specimen components, editor windows and scenes that many shots use |
| `capture/doctor.py` | Reports what the open project can retake. Read-only |
| `capture/fixtures.py` | Installs or removes the fixtures in the open project |
| `capture/promote.py` | Copies regenerated images into `images/` and updates the manifests |

## Ground rules

1. **Never capture the screen.** Every capture comes from the Editor's own view buffers (`efds.sh` helpers, `unity command
   editor_window`). Screen grabs pick up other windows, notifications and personal information. A human screen recording or
   screenshot is only for the few images that need real pointer movement or an OS-drawn element. Those recipes are marked
   `manual`.
2. **Match the original.** Each recipe has `original_image_url`. Keep the same content, framing, crop and scale. Change only
   what the newer Editor changes, and record anything notable in the recipe's notes.
3. **Make dark and light.** Each image ships in the dark and light Editor skins, each with a macOS and a Windows window frame,
   at 1x and @2x. Light captures use the same name with an `_l` suffix.
4. **No personal information.** Check that no user names, home-folder paths, emails or account details are visible. Blur them if
   they are.
5. **Use a scratch project.** Many setup scripts replace the open scene or add assets (see each recipe's `changes_project`).
   Never run recipes in a project you care about.

## Setup

Requirements:
- A Unity Editor, 6.0 or later. The recipes were made on Unity 7000.0.0a20; see [Other projects and Unity versions](#other-projects-and-unity-versions).
- The [Unity CLI](https://docs.unity.com) (`unity --version`).
- Python 3 with Pillow (`pip install pillow`).
- `ffmpeg`, for GIFs only.
- A bash shell: Git Bash on Windows, or Terminal on macOS.

1. Create and open a scratch project, for example `unity projects create EFDS-Capture --path ~/UnityProjects`.
   Opening it from Unity Hub works too.
2. Install the Pipeline package so the CLI can drive it: `unity pipeline install` (run inside the project folder).
3. Set **Preferences > UI Scaling** to 250%. The layouts were measured at 2.5 px per point. Other scales still compose, because
   `compose.py` resizes every capture to its layout box, but they come out softer.
4. Check what you can retake:
   ```bash
   cd capture
   python doctor.py
   ```
5. Install the fixtures if the doctor asks for them. This writes `Assets/EFDS_Screenshots` into the open project:
   ```bash
   python fixtures.py install
   ```
   Some recipes also need sample content that isn't in this repo. The doctor lists them. Most are optional:

   | Requirement | How to get it |
   |---|---|
   | `sample:Timeline Gameplay Sequence Demo` | Package Manager > Timeline > Samples > Gameplay Sequence Demo > Import |
   | `sample:URP template ExampleAssets` | Create the scratch project from the 3D (URP) template |
   | `sample:SampleScene` | Comes with most templates (`Assets/Scenes/SampleScene.unity`) |
   | `sample:Atrio` | A large internal environment sample. Without it, use any detailed environment scene and change the scene path at the top of the setup script |

If more than one Editor is open, set `EFDS_PROJECT` to the project path you want to drive.

## Retake one image

1. **Find the recipe.** Search `capture/recipes.json` for the image's `id`. The id is the same as in `manifest.json`, for
   example `components/button/buttons-basic.png`. You can also run `python doctor.py --id buttons-basic`. Read:
   - `notes`: what the image shows and how it was made;
   - `original_image_url`: open it, because this is what you're matching;
   - `level`: how much of the recipe is recorded (see below);
   - `requires` and `changes_project`.
2. **Capture (dark).** Make sure the Editor is in the dark skin. In a bash shell in `capture/`, run `source efds.sh`, then run each
   dark entry in `captures` in order. Each command runs its setup script(s) and writes `captures/<name>.png`.
   Entries marked "derived from other captures by a compose script" are made in step 3; skip them here.
3. **Compose (dark).**
   - If the recipe lists `page_scripts`, run them: `python scripts/pages/<name>.py`.
   - Otherwise compose the dark layout: `python scripts/compose.py layouts/<name>.json`.

   The output goes to `capture/replacements/<page>/...`, as macOS and Windows frames at 1x and @2x.
4. **Light.** Switch the Editor to the light skin (Preferences > General > Editor Theme). Run the `_l` capture commands, then
   compose again with `THEME=light`:
   - `THEME=light python scripts/pages/<name>.py`, or
   - `python scripts/compose.py layouts/<name>_light.json`.

   If the light layout is missing or the light content sits a few points lower, `python scripts/lightlayout.py
   layouts/<name>.json` derives it, and `python scripts/vshift.py <capture>` measures the offset.
5. **Check it.** Put your 1x output next to the original. Check:
   - the same content and the same crop;
   - no clipped text;
   - no stray selection or hover;
   - no personal information.
6. **Promote and commit.**
   ```bash
   python promote.py "<recipe id>"     # copies into images/ and updates manifest.json / manifest.csv
   git diff --stat
   ```
   Commit, then hand over to the site update in [AGENTS.md](AGENTS.md), which only applies what changed.

## Recipe levels

| `level` | Meaning | What you do |
|---|---|---|
| `scripted` | Every capture has a recorded command | Run the commands, then compose |
| `partial` | The setup and compose scripts are known, but some capture commands weren't recorded | Rebuild the missing commands (below) |
| `notes-only` | Only the notes, the layout and the original | Rebuild the shot from those (below) |
| `manual` | Needs a screen recording or a human screenshot | Follow the notes; recordings go in `capture/incoming/gifs/<file>.mp4` for `rec_build.py` |

`source` on each capture entry tells you where its command came from:
- **recorded**: the command used when the image was made.
- **light twin of the recorded dark command**: the same steps with the `_l` name, run in the light skin.
- **derived from other captures by a compose script**: made in step 3, not by a capture command.

### Rebuilding a missing capture command

- The layout's `windows[].src` names each capture, and its `w`/`h` give the size it fills on the canvas.
- Each setup script starts with a comment saying what it sets up, followed by its parameters on one line, for example
  `string WHICH = "variations";` or `float W = 346, H = 230; string EXPAND = "";`. Patch them with `ueval`'s sed argument:
  `ueval unity/btn_spec.cs 's/string WHICH = "variations";/string WHICH = "basic";/'`.
- Then grab the window with the helper that fits:

| Helper | Captures |
|---|---|
| `capwin <window title> <name>` | A floating window, by its title (the specimen host is `EFDSPopupHost`) |
| `capdock <title or type> <name>` | A floating window's tab strip and content, for example `capdock InspectorWindow insp` |
| `capdock2 <type> <name>` | The same, for windows in a tabbed floating container |
| `capinsp`, `caphier`, `capprops`, `capsv <name>` | The floating Inspector, Hierarchy, Properties window or Scene view |
| `capview <GUIView type> <name>` | A view without a window, for example `UnityEditor.Toolbar` (the main toolbar) |
| `capscene <name>` | The Scene view as drawn |
| `ueval <file.cs> [sed]`, `ucode '<C#>'` | Run a setup script, or a line of C#, in the Editor |

- **GIF frames:** source `scripts/gif_cap.sh` after `efds.sh`. Its helpers send real mouse and key events and capture one frame
  at a time: `gm`, `gk`, `gf` and `gq`. Then run `python scripts/gif_build.py`.

Write rect data and temporary files under `$EFDS_TMP`. `efds.sh` points every `C:/tmp/` in the setup scripts there, so they run
unchanged on any machine.

## Other projects and Unity versions

The recipes aren't tied to one project or one Editor build:
- `efds.sh` and `doctor.py` drive whichever Editor is open.
- Setup scripts create what they need (temporary unsaved objects, specimen windows), or take it from the fixtures.
- Captures come from view buffers on any platform.

What changes between versions is the Editor itself. Some setup scripts reach into Editor internals by reflection (a window type
or private field), and those can be renamed between versions. When a setup script fails:

1. Read the error `ueval` prints. It names the missing type or member.
2. Find its equivalent in your version. For example, list window types with
   `ucode 'return string.Join(",", UnityEngine.Resources.FindObjectsOfTypeAll<UnityEditor.EditorWindow>().Select(w => w.GetType().FullName));'`.
3. Fix the script, keeping the old name working if you can, and note the change.

Expect the images to differ where the Editor differs: control styles, icons, menu items, the packages listed in a settings
page. That is the point of a retake. Keep the framing the same and let the UI be current.

If the fixtures don't compile in your version, run `python fixtures.py remove` and use the recipes that don't need them
(the doctor tells you which).

## For maintainers

`capture/` is generated from the audit workspace by `build_kit.py` and refreshed on every publish. If you improve a recipe by
hand, also record what you changed in its notes, so the next retake starts from your version.
