"""Components > List view: compose the dark images from unity/lv_spec.cs (captures/lv_<which>.png, rects C:/tmp/lv_<which>.json) at 2x
with the original's pills and callouts. Unity 7 ListView / TreeView default rows are 22pt tall (measured)."""
import os as _os; _EFDS_TMP = _os.environ.get("EFDS_TMP", "C:/tmp")
import json, os, subprocess, sys
from PIL import ImageFont
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, G, FILL, OUT as TOUT, BG as TBG, INK, TXT, PAGE as TPAGE, themed
F, FB = "fonts/Inter-Regular.ttf", "fonts/Inter-SemiBold.ttf"; OUT, BG = TOUT, TBG
OUTDIR = "replacements/components/list-view"; os.makedirs(OUTDIR, exist_ok=True)
def R(w): return {k: [v * 2 for v in r] for k, r in json.load(open(f"{_EFDS_TMP}/lv_{w}{SUF}.json")).items()}
L = lambda a, b, c, d: {"t": "line", "x1": a, "y1": b, "x2": c, "y2": d, "color": G, "w": 2}
pill = lambda s, x, y: {"t": "pill", "s": s, "x": x, "y": y, "h": 40, "pad": 14, "size": 22, "bg": BG, "outline": OUT, "color": INK, "font": FB}
gpill = lambda s, x, y: {"t": "pill", "s": s, "x": x, "y": y, "h": 36, "pad": 12, "size": 20, "bg": FILL, "outline": G, "color": G, "font": F}
def badge(n, cx, cy): return [{"t": "dot", "x": cx, "y": cy, "r": 20, "color": G}, {"t": "dot", "x": cx, "y": cy, "r": 18, "color": FILL},
                              {"t": "text", "s": str(n), "x": cx, "y": cy, "anchor": "mm", "size": 22, "color": G, "font": F}]
def bracket(x, y0, y1, side):   # side -1: bracket opens to the right (on the left of the target); +1: opens to the left
    t = 8 * -side; return [L(x, y0, x, y1), L(x, y0, x + t, y0), L(x, y1, x + t, y1)]
def run(n, out, which, ann):
    spec = {"out": f"{OUTDIR}/{out}", "canvas": [1600, 600], "backdrop": BG,
            "windows": [{"src": f"captures/lv_{which}{SUF}.png", "chrome": "none", "x": 0, "y": 0, "w": 1600}], "annotations": ann}
    lay = f"layouts/list-view_{n}{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)

# anatomy: 1 foldout header, 2 list, 3 row, 4 scrollbar
r = R("anatomy"); x, y, w, h = r["list"]; r1 = r["list:row1"]; sc = r["list:scroller"]
hy = y + 22; body0 = r["list:row0"][1]
ann = bracket(x - 14, y + 2, body0 - 4, -1) + [L(x - 14, (y + body0) / 2, x - 50, (y + body0) / 2)] + badge(1, x - 70, (y + body0) / 2)
ann += bracket(x - 14, body0, 600, -1) + [L(x - 14, 300, x - 50, 300)] + badge(2, x - 70, 300)
rx = r1[0] + r1[2] + 8
ann += bracket(sc[0] - 6, r1[1], r1[1] + r1[3], 1) + [L(sc[0] - 6, r1[1] + r1[3] / 2, x + w + 20, r1[1] + r1[3] / 2)] + badge(3, x + w + 40, r1[1] + r1[3] / 2)
sy = sc[1] + 200; ann += [{"t": "dot", "x": sc[0] + sc[2] / 2, "y": sy, "r": 5, "color": G}, L(sc[0] + sc[2] / 2, sy, x + w + 20, sy)] + badge(4, x + w + 40, sy)
run("05", "lv-anatomy.png", "anatomy", ann)

# foldout: default list vs list with a foldout header
r = R("foldout")
run("03", "lv-foldout.png", "foldout", [pill("Default list view", r["plain"][0] - 4, r["plain"][1] - 60), pill("List view with a foldout", r["fold"][0] - 4, r["fold"][1] - 40)])

# row height: 22px bracket on the first row of the tree view and the list view
r = R("rowheight"); ann = [pill("Tree view", r["tree"][0], r["tree"][1] - 60), pill("List view", r["list"][0], r["list"][1] - 60)]
for k in ("tree", "list"):
    rx, ry, rw, rh = r[k + ":row0"]; bx = rx - 14
    ann += bracket(bx, ry, ry + rh, -1) + [L(bx - 16, ry + rh / 2, bx, ry + rh / 2), gpill("22px", bx - 16 - 74, ry + rh / 2)]
run("04", "tv-lv_row-height.png", "rowheight", ann)
