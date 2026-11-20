"""Color palette > Window Backgrounds: Unity 7 Light Explorer (captures/cp_lights.png; unity/cp_lights.cs,
cp_lights_order.cs, cp_lights2.cs, le_click.cs) at the original's 2x framing, cropped like the original, with its
four callouts on the measured rows."""
import json, os, subprocess, sys
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from ov_callouts import badge, G
X, Y, K = 140, 114, 2; P = lambda x, y: (X + x * K, Y + y * K)
L = lambda x1, y1, x2, y2: {"t": "line", "x1": x1, "y1": y1, "x2": x2, "y2": y2, "color": G, "w": 2}
D = lambda x, y: {"t": "dot", "x": x, "y": y, "r": 5, "color": G}
ann = []
# 1 Default Background: the tab strip beside the Light Explorer tab
x1, y1 = P(141, 8); ann += badge(x1, 72, 1) + [L(x1, 92, x1, y1), D(x1, y1)]
# 2 Highlight Background: the selected Point Light row (150-168 pt)
_, y2 = P(0, 159); ann += badge(76, y2, 2) + [L(96, y2, P(22, 0)[0], y2), D(P(22, 0)[0], y2)]
# 3 Window Background: the window margin and a plain (non-alternated) row (204-222 pt)
_, ya = P(0, 195); _, yb = P(0, 213); ex = 124
ann += badge(76, ya, 3) + [L(96, ya, P(7.5, 0)[0], ya), D(P(7.5, 0)[0], ya), L(ex, ya, ex, yb), L(ex, yb, P(22, 0)[0], yb), D(P(22, 0)[0], yb)]
# 4 Alternated Rows Background: an alternated row (222-240 pt)
_, y4 = P(0, 231); ann += badge(76, y4, 4) + [L(96, y4, P(22, 0)[0], y4), D(P(22, 0)[0], y4)]
p = 2.5
spec = {"out": "replacements/fundamentals/color-palette/Window Backgrounds.png", "canvas": [1600, 656], "backdrop": [25, 26, 27],
        "windows": [{"src": "captures/cp_lights.png", "chrome": "none", "x": X, "y": Y, "w": 730 * K,
                     "crop": [0, 0, round(50 * p), round(29 * p)]}], "annotations": ann}
json.dump(spec, open("layouts/color-palette_13.json", "w"), indent=1)
subprocess.run([sys.executable, "scripts/compose.py", "layouts/color-palette_13.json"], check=True, stdout=subprocess.DEVNULL)
