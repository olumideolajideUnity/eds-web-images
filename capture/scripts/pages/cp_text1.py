"""Color palette > Text Colors Base 1: Unity 7 Console (captures/cp_console.png; unity/cp_console.cs logs the
original's messages from EFDS.Example in BadFile.cs) over the main window's status bar (captures/cp_statusbar.png,
unity/em_statusbar.cs), at the original's 2x framing. Callouts: 1 Default Text, 2 Error Text, 3 Link Text."""
import json, os, subprocess, sys
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from ov_callouts import badge, G
X, Y, K, PW, PH, SBH = 180, 78, 2, 670, 322, 20
P = lambda x, y: (X + x * K, Y + y * K)
L = lambda x1, y1, x2, y2: {"t": "line", "x1": x1, "y1": y1, "x2": x2, "y2": y2, "color": G, "w": 2}
def R(x0, y0, x1, y1, px=4.5, py=4):          # box around measured text bounds (pt), padded like the original
    (a, b), (c, d) = P(x0 - px, y0 - py), P(x1 + px, y1 + py)
    return {"t": "rect", "x": a, "y": b, "w_": c - a, "h": d - b, "color": G, "w": 2, "radius": 3, "dash": False, "solid": True}, (a, b, c, d)
ann = []
# 1 Default Text: first line of the message detail (measured 3.2-331.2 x 191.6-202.4 pt)
r, (a, b, c, d) = R(3.2, 191.6, 331.2, 202.4, py=1.6); my = (b + d) / 2
ann += [r, L(124, my, a, my)] + badge(104, my, 1)
# 3 Link Text: the stack trace's file link (188.4-518 x 307.6-318.4 pt), leader to the right margin
r, (a, b, c, d) = R(188.4, 307.6, 518, 318.4, py=1.6); my = (b + d) / 2
ann += [r, L(c, my, 1540, my)] + badge(1560, my, 3)
# 2 Error Text: the status bar's last-error message (20.4-333.6 x 5.2-16 pt within the bar)
r, (a, b, c, d) = R(20.4, PH + 5.2, 333.6, PH + 16, py=3); mx = (a + c) / 2
ann += [r, L(mx, d, mx, 822)] + badge(mx, 842, 2)
p = 2.5
spec = {"out": "replacements/fundamentals/color-palette/Text Colors Base 1.png", "canvas": [1600, 912], "backdrop": [25, 26, 27],
        "windows": [{"src": "captures/cp_console.png", "chrome": "none", "x": X, "y": Y, "w": PW * K},
                    {"src": "captures/cp_statusbar.png", "chrome": "none", "x": X, "y": Y + PH * K, "w": PW * K,
                     "crop": [0, 0, round((1536 - PW) * p), 0]}], "annotations": ann}
json.dump(spec, open("layouts/color-palette_15.json", "w"), indent=1)
subprocess.run([sys.executable, "scripts/compose.py", "layouts/color-palette_15.json"], check=True, stdout=subprocess.DEVNULL)
