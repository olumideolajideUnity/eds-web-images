"""Empty states #7 (es-alignment.png): centering guides over the #5 pair, measured from the Unity 7 captures.
For each window: dashed outline of the content area, dashed box around the empty-state content block (detected from
pixels), and solid guides from the block to each content edge, in the original's orange.
"""
import json, os, subprocess, sys
from PIL import Image

ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, themed
ORANGE = [205, 80, 40] if LIGHT else [255, 110, 64]
base = {"canvas": [1600, 600], "backdrop": [226, 226, 226] if LIGHT else [56, 56, 56],
        "windows": [{"src": f"captures/es07_tl{SUF}.png", "chrome": "none", "x": 40, "y": 103, "w": 855},
                    {"src": f"captures/es07_uvcs{SUF}.png", "chrome": "none", "x": 919, "y": 103, "w": 641}]}
ann = []
results = []
for win, top_pt in zip(base["windows"], (72, 26)):          # content starts below tab (+ Timeline toolbars)
    im = Image.open(win["src"]).convert("RGB"); ppp = 2.5
    k = win["w"] / (im.width / ppp)                                    # canvas px per pt
    W, H = im.width, im.height
    t = int(top_pt * ppp); bot = H - int(2 * ppp)
    bg = im.getpixel((int(W * 0.95), int(H * (0.6 if LIGHT else 0.9))))   # light: 0.9 lands on the lighter bottom bar
    xs, ys = [], []
    for y in range(t + 4, bot, 2):
        row = [x for x in range(4, W - 4, 2) if sum(abs(c - d) for c, d in zip(im.getpixel((x, y)), bg)) > 60]
        if len(row) > 0.8 * len(range(4, W - 4, 2)): continue      # full-width bands (light skin: header strip, bottom bar)
        xs += row; ys += [y] * len(row)
    bx1, by1, bx2, by2 = min(xs), min(ys), max(xs), max(ys)
    m = 6 * ppp
    bx1, by1, bx2, by2 = bx1 - m, by1 - m, bx2 + m, by2 + m
    cx0, cy0 = win["x"], win["y"] + t / ppp * k
    cx1, cy1 = win["x"] + win["w"], win["y"] + bot / ppp * k
    X = lambda px: win["x"] + px / ppp * k
    Y = lambda py: win["y"] + py / ppp * k
    b = (X(bx1), Y(by1), X(bx2), Y(by2))
    ann += [{"t": "rect", "x": cx0, "y": cy0, "w_": cx1 - cx0, "h": cy1 - cy0, "color": ORANGE, "w": 1},
            {"t": "rect", "x": b[0], "y": b[1], "w_": b[2] - b[0], "h": b[3] - b[1], "color": ORANGE, "w": 1}]
    mx, my = (b[0] + b[2]) / 2, (b[1] + b[3]) / 2
    for (x1, y1, x2, y2) in ((mx, cy0, mx, b[1]), (mx, b[3], mx, cy1), (cx0, my, b[0], my), (b[2], my, cx1, my)):
        ann.append({"t": "line", "x1": x1, "y1": y1, "x2": x2, "y2": y2, "color": ORANGE, "w": 1.5})
        if x1 == x2: ann += [{"t": "line", "x1": x1 - 7, "y1": y1, "x2": x1 + 7, "y2": y1, "color": ORANGE, "w": 1.5}, {"t": "line", "x1": x1 - 7, "y1": y2, "x2": x1 + 7, "y2": y2, "color": ORANGE, "w": 1.5}]
        else: ann += [{"t": "line", "x1": x1, "y1": y1 - 7, "x2": x1, "y2": y1 + 7, "color": ORANGE, "w": 1.5}, {"t": "line", "x1": x2, "y1": y1 - 7, "x2": x2, "y2": y1 + 7, "color": ORANGE, "w": 1.5}]
    results.append({"left": round(b[0] - cx0), "right": round(cx1 - b[2]), "top": round(b[1] - cy0), "bottom": round(cy1 - b[3])})

spec = dict(base); spec["out"] = "replacements/patterns/empty-states/es-alignment.png"; spec["annotations"] = ann
lay = f"layouts/empty-states_07{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
print("margins (canvas px) Timeline / UVCS:", results)
