"""Color palette > Text Colors Base 3: Unity 7 floating Scene view with the Default overlay preset (captures/cp_scene.png;
unity/cp_scene.cs), at the original's 2x framing. Callouts: 1 Tab Text ("Scene"), 2 Toolbar Button Text ("Global")."""
import json, os, subprocess, sys
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from ov_callouts import badge, G
X, Y, K, PW = 344, 150, 2, 456
P = lambda x, y: (X + x * K, Y + y * K)
L = lambda x1, y1, x2, y2: {"t": "line", "x1": x1, "y1": y1, "x2": x2, "y2": y2, "color": G, "w": 2}
def R(x0, y0, x1, y1, px=4.5, py=4):
    (a, b), (c, d) = P(x0 - px, y0 - py), P(x1 + px, y1 + py)
    return {"t": "rect", "x": a, "y": b, "w_": c - a, "h": d - b, "color": G, "w": 2, "radius": 3, "dash": False, "solid": True}, (a, b, c, d)
ann = []
# 1 Tab Text: the tab's "Scene" label (measured 26-59.6 x 10.4-18.8 pt)
r, (a, b, c, d) = R(26, 10.4, 59.6, 18.8); my = (b + d) / 2
ann += [r, L(244, my, a, my)] + badge(224, my, 1)
# 2 Toolbar Button Text: "Global" on the Tool Settings toolbar's handle-orientation button (114.8-148.8 x 32.8-41.2 pt)
r, (a, b, c, d) = R(114.8, 32.8, 148.8, 41.2, px=3.5); mx = (a + c) / 2
ann += [r, L(mx, 126, mx, b)] + badge(mx, 106, 2)
spec = {"out": "replacements/fundamentals/color-palette/Text Colors Base 3.png", "canvas": [1600, 744], "backdrop": [25, 26, 27],
        "windows": [{"src": "captures/cp_scene.png", "chrome": "none", "x": X, "y": Y, "w": PW * K}], "annotations": ann}
json.dump(spec, open("layouts/color-palette_17.json", "w"), indent=1)
subprocess.run([sys.executable, "scripts/compose.py", "layouts/color-palette_17.json"], check=True, stdout=subprocess.DEVNULL)
