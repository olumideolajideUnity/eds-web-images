"""Color palette > Window borders: Unity 7 two-column Project window (captures/cp_proj.png, unity/cp_proj.cs +
cp_proj_size.cs) at the original's 2x framing, with the original's three callouts on the measured lines."""
import json, os, subprocess, sys
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from ov_callouts import badge, G
X, Y, K = 154, 122, 2; P = lambda x, y: (X + x * K, Y + y * K)
W, H = 647.2, 273.2
L = lambda x1, y1, x2, y2: {"t": "line", "x1": x1, "y1": y1, "x2": x2, "y2": y2, "color": G, "w": 2}
ann = []
# 1 toolbar border: the #141414 line under the toolbar (y 48-48.8 pt)
_, ty = P(0, 48.4); rx = P(W, 0)[0]
ann += [L(rx - 20, ty, 1484, ty)] + badge(1504, ty, 1)
# 2 window border: bracket along the right pane's content area (below breadcrumbs, above the zoom bar)
_, y0 = P(0, 68); _, y1 = P(0, 252); bx = 1465
ann += [L(rx - 20, y0, bx, y0), L(bx, y0, bx, y1), L(rx - 20, y1, bx, y1), L(bx, (y0 + y1) / 2, 1484, (y0 + y1) / 2)] + badge(1504, (y0 + y1) / 2, 2)
# 3 default border: the column splitter (x 194.8-195.2 pt), leader down from the window's bottom edge
sx, by = P(195, H)
ann += [L(sx, by - 20, sx, 700)] + badge(sx, 720, 3)
spec = {"out": "replacements/fundamentals/color-palette/Window borders.png", "canvas": [1600, 798], "backdrop": [25, 26, 27],
        "windows": [{"src": "captures/cp_proj.png", "chrome": "none", "x": X, "y": Y, "w": round(W * K)}], "annotations": ann}
json.dump(spec, open("layouts/color-palette_12.json", "w"), indent=1)
subprocess.run([sys.executable, "scripts/compose.py", "layouts/color-palette_12.json"], check=True, stdout=subprocess.DEVNULL)
