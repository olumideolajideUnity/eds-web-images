"""Accessibility a11y-9Y/9N (click-target size): Unity 7's real Play / Pause / Step buttons from the main toolbar
(captures/acc_toolbar.png, the UnityEditor.Toolbar view grabbed from its own buffer), with the original's pink click-target callout:
9Y marks the whole Pause button (28 x 19.6pt), 9N only its icon (7.6 x 9.2pt), measured from the capture."""
import json, os, subprocess, sys
from PIL import ImageFont
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, themed
P, K = 2.5, 2
G0, G1, BT = 723.6, 812.4, 0                     # play group x range (pt) and toolbar band top
X = 389 - (G1 - G0) * K / 2 - (G0 - 700) * K     # canvas x of toolbar pt 700 (crop starts at 700pt)
Y = 182 - 8.0 * K                                 # button top (8pt) lands at y=182
cx = lambda pt: X + (pt - 700) * K; cy = lambda pt: Y + pt * K
PINK, FILL = ([200, 30, 90], [246, 214, 226]) if LIGHT else ([218, 33, 98], [54, 26, 36])
F = "fonts/Inter-Regular.ttf"; f = ImageFont.truetype(F, 26); tw = f.getlength("Click target")
for key, (a, b, c, d) in (("9Y", (754.0, 8.0, 782.0, 27.6)), ("9N", (762.4, 11.6, 773.2, 24.0))):
    x0, y0, x1, y1 = cx(a), cy(b), cx(c), cy(d); mx = (x0 + x1) / 2
    ann = [{"t": "rect", "x": x0, "y": y0, "w_": x1 - x0, "h": y1 - y0, "color": PINK, "w": 2, "fill": 0.22, "dash": False, "solid": True},
           {"t": "line", "x1": mx, "y1": y1, "x2": mx, "y2": 314, "color": PINK, "w": 2},
           {"t": "pill", "s": "Click target", "x": mx - tw / 2 - 18, "y": 337, "h": 46, "pad": 18, "size": 26, "bg": FILL, "outline": PINK, "color": PINK, "font": F}]
    spec = {"out": f"replacements/fundamentals/accessibility/a11y-{key}.png", "canvas": [780, 480], "backdrop": [235, 235, 235] if LIGHT else [20, 20, 20],
            "windows": [{"src": f"captures/acc_toolbar{SUF}.png", "chrome": "none", "x": X, "y": Y, "w": 140 * K,
                         "crop": [round(700 * P), 0, 3840 - round(840 * P), 0]}], "annotations": ann}
    lay = f"layouts/accessibility_{key}{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
