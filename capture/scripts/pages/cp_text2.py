"""Color palette > Text Colors Base 2: Unity 7 Project Settings > Editor (captures/cp_settings.png; unity/cp_settings.cs,
cp_settings_scroll.cs Y=1045, cp_settings_click.cs focuses the Asynchronous Shader Compilation toggle through the window's
own click handling), at the original's 2x framing. Callouts: 1 Helpbox Text, 2 Label Text, 3 Label Text Focus."""
import json, os, subprocess, sys
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from ov_callouts import badge, G
X, Y, K, PW = 196, 110, 2, 604
P = lambda x, y: (X + x * K, Y + y * K)
L = lambda x1, y1, x2, y2: {"t": "line", "x1": x1, "y1": y1, "x2": x2, "y2": y2, "color": G, "w": 2}
def R(x0, y0, x1, y1, px=4.5, py=3):
    (a, b), (c, d) = P(x0 - px, y0 - py), P(x1 + px, y1 + py)
    return {"t": "rect", "x": a, "y": b, "w_": c - a, "h": d - b, "color": G, "w": 2, "radius": 3, "dash": False, "solid": True}, (a, b, c, d)
ann = []
# 1 Helpbox Text: the Numbering Scheme help box message (measured 241.2-564.8 x 245.2-266.8 pt), leader to the right margin
r, (a, b, c, d) = R(241.2, 245.2, 564.8, 266.8); my = (b + d) / 2
ann += [r, L(c, my, 1480, my)] + badge(1500, my, 1)
# 2 Label Text: "When entering Play Mode" (211.6-355.6 x 152.8-164 pt)
r, (a, b, c, d) = R(211.6, 152.8, 355.6, 164); my = (b + d) / 2
ann += [r, L(124, my, a, my)] + badge(104, my, 2)
# 3 Label Text Focus: the focused "Asynchronous Shader Compilation" toggle label (211.6-405.2 x 106-116.8 pt)
r, (a, b, c, d) = R(211.6, 106, 405.2, 116.8); my = (b + d) / 2
ann += [r, L(124, my, a, my)] + badge(104, my, 3)
spec = {"out": "replacements/fundamentals/color-palette/Text Colors Base 2.png", "canvas": [1600, 912], "backdrop": [25, 26, 27],
        "windows": [{"src": "captures/cp_settings.png", "chrome": "none", "x": X, "y": Y, "w": PW * K}], "annotations": ann}
json.dump(spec, open("layouts/color-palette_16.json", "w"), indent=1)
subprocess.run([sys.executable, "scripts/compose.py", "layouts/color-palette_16.json"], check=True, stdout=subprocess.DEVNULL)
