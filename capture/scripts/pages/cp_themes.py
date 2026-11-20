"""Color palette > Themes: one Inspector (Main Camera, Transform + Camera expanded; unity/acc_insp_cam.cs at 637 x 270 pt) shown in
both skins, split down the middle like the original: light skin left of x=800, dark skin right, each on its own page colour.
Needs captures/cp_themes_l.png (light skin) and captures/cp_themes.png (dark skin); skips (and says so) until both exist."""
import json, os, subprocess, sys
from PIL import Image
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
L, D = "captures/cp_themes_l.png", "captures/cp_themes.png"
if not (os.path.exists(L) and os.path.exists(D)): sys.exit("cp_themes: waiting for " + " and ".join(p for p in (L, D) if not os.path.exists(p)))
S, X, Y, W = 2, 165, 112, 1273                     # 1x canvas: window left / top / width (original's placement)
out = Image.new("RGB", (1600 * S, 600 * S), (194, 194, 194)); out.paste((30, 30, 31), (800 * S, 0, 1600 * S, 600 * S))
for src, x0, x1 in ((L, 0, 800), (D, 800, 1600)):
    im = Image.open(src).convert("RGB"); im = im.resize((W * S, round(im.height * W * S / im.width)), Image.LANCZOS)
    half = Image.new("RGB", out.size); half.paste(out, (0, 0)); half.paste(im, (X * S, Y * S))
    out.paste(half.crop((x0 * S, 0, x1 * S, 600 * S)), (x0 * S, 0))
out.save("captures/cp_themes_split.png")
spec = {"out": "replacements/fundamentals/color-palette/Themes.png", "canvas": [1600, 600], "backdrop": [194, 194, 194],
        "windows": [{"src": "captures/cp_themes_split.png", "chrome": "none", "x": 0, "y": 0, "w": 1600}], "annotations": []}
json.dump(spec, open("layouts/color-palette_01.json", "w"), indent=1)
subprocess.run([sys.executable, "scripts/compose.py", "layouts/color-palette_01.json"], check=True, stdout=subprocess.DEVNULL)
print("composed layouts/color-palette_01.json")
