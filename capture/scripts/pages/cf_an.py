"""Components > Color field anatomy (colorField-an.png): a real UI Toolkit ColorField (captures/cf_field.png) and Unity's real
Color picker opened for the same colour (captures/cf_picker.png; unity/cf_an.cs), placed like the original: the picker's top half
with its window frame, and its lower half (RGB sliders, hexadecimal) cropped beside it; the original's labelled callouts."""
import os as _os; _EFDS_TMP = _os.environ.get("EFDS_TMP", "C:/tmp")
import json, os, subprocess, sys
from PIL import ImageFont
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from ov_callouts import G, F as FILL
P = 2.5; FB = "fonts/Inter-SemiBold.ttf"; f = ImageFont.truetype(FB, 22)
r = {k: [v * 2 for v in val] for k, val in json.load(open(_EFDS_TMP + "/cf_an.json")).items()}
fx, fy, fw, fh = r["field"]; cx_, cy_, cw_, ch_ = r["unity-color-field__color"]; ex, ey, ew, eh = r["unity-color-field__eyedropper"]
L = lambda a, b, c, d: {"t": "line", "x1": a, "y1": b, "x2": c, "y2": d, "color": G, "w": 2}
def pill(s, cx, cy):
    w = f.getlength(s) + 28
    return {"t": "pill", "s": s, "x": cx - w / 2, "y": cy, "h": 38, "pad": 14, "size": 22, "bg": FILL, "outline": G, "color": G, "font": FB}
px = 274; mid = cx_ + cw_ / 2
ann = [pill("Color swatch field", px, 222), L(px, 241, px, 262), L(px, 262, mid, 262), L(mid, 262, mid, cy_),
       pill("Alpha swatch field", px, 372), L(px, 353, px, 330), L(px, 330, mid, 330), L(mid, 330, mid, fy + fh),
       pill("Eyedropper button", px, 444), L(px + 104, 444, ex + ew / 2, 444), L(ex + ew / 2, 444, ex + ew / 2, ey + eh),
       pill("Color picker modal", 896, 38)]
CROP_TOP, CROP_BOT = 178.6, 449                         # lower half of the picker (pt): RGB dropdown lands at y=107pt
spec = {"out": "replacements/components/color-field/colorField-an.png", "canvas": [1600, 600], "backdrop": [50, 50, 50],
        "windows": [{"src": "captures/cf_field.png", "chrome": "none", "x": 0, "y": 0, "w": 1600},
                    {"src": "captures/cf_picker.png", "title": "Color", "x": 550, "y": 82, "w": 466, "h": None},
                    {"src": "captures/cf_picker.png", "chrome": "none", "x": 1056, "y": 0, "w": 466,
                     "crop": [0, round(CROP_TOP * P), 0, 1620 - round(CROP_BOT * P)]}], "annotations": ann}
os.makedirs("replacements/components/color-field", exist_ok=True)
json.dump(spec, open("layouts/color-field_05.json", "w"), indent=1)
subprocess.run([sys.executable, "scripts/compose.py", "layouts/color-field_05.json"], check=True, stdout=subprocess.DEVNULL)
