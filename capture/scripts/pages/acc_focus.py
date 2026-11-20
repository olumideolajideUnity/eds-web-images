"""Accessibility a11y-8Y/8N (focus styling): a real Unity 7 Inspector Camera component (URP) with its Field of View slider focused
by a click sent through the Inspector's own event handling (captures/acc_insp8.png; unity/acc_insp_cam.cs EXPAND=Camera,
unity/acc_insp_click.cs). 8Y shows Unity 7's real focus state (blue label, thumb ring and field border). 8N ('don't') is the
same capture with those blue focus highlights recoloured to white, like the original's mock."""
import json, os, subprocess, sys
from PIL import Image
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, themed
DY = 4.0 if LIGHT else 0   # light skin: GameObject header 4pt taller (rows measured 10px lower)
P = 2.5
y = Image.open(f"captures/acc_insp8{SUF}.png").convert("RGB"); n = y.copy()
# Field of View row (pt): label, slider thumb and number field; turn bluish focus pixels into neutral greys of the same brightness
x0, y0, x1, y1 = 40, 230 + DY, 346, 248 + DY
for X in range(round(x0 * P), round(x1 * P)):
    for Y in range(round(y0 * P), round(y1 * P)):
        r, g, b = n.getpixel((X, Y))
        if b - r > 25: v = min(r, g, b) if LIGHT else max(r, g, b); n.putpixel((X, Y), (v, v, v))
n.save(f"captures/acc_insp8N{SUF}.png")
TOP, BOT = 116.8 + DY, 314.8 + DY                                   # Camera component: header top to the Projection section's end
for key, src in (("8Y", f"captures/acc_insp8{SUF}.png"), ("8N", f"captures/acc_insp8N{SUF}.png")):
    spec = {"out": f"replacements/fundamentals/accessibility/a11y-{key}.png", "canvas": [780, 480], "backdrop": [203, 203, 203] if LIGHT else [25, 26, 27],
            "windows": [{"src": src, "chrome": "none", "x": 46, "y": 82, "w": 692, "crop": [0, round(TOP * P), 0, y.height - round(BOT * P)]}],
            "annotations": []}
    lay = f"layouts/accessibility_{key}{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
