"""Components > Foldout: compose the dark images from unity/fo_spec.cs (captures/fo_<which>.png, rects C:/tmp/fo_<which>.json) at 2x
with the original's pills and callouts. Indentation measured from the UI Toolkit Foldouts: content labels step 15.2pt per level
(237.6 -> 252.8pt) and the arrow sits 15.2pt left of its level's labels."""
import os as _os; _EFDS_TMP = _os.environ.get("EFDS_TMP", "C:/tmp")
import json, os, subprocess, sys
from PIL import ImageFont
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, G, FILL, OUT as TOUT, BG as TBG, INK, TXT, PAGE as TPAGE, themed
F, FB = "fonts/Inter-Regular.ttf", "fonts/Inter-SemiBold.ttf"; WHITE, OUT, BG, RED = INK, TOUT, TBG, ([205, 80, 40] if LIGHT else [255, 110, 64])
OUTDIR = "replacements/components/foldout"; os.makedirs(OUTDIR, exist_ok=True)
fnt = ImageFont.truetype(FB, 22)
def R(w): return {k: [v * 2 for v in r] for k, r in json.load(open(f"{_EFDS_TMP}/fo_{w}{SUF}.json")).items()}
L = lambda a, b, c, d, col=G, w=2: {"t": "line", "x1": a, "y1": b, "x2": c, "y2": d, "color": col, "w": w}
def pill(s, x, y, col=None, fill=None):
    return {"t": "pill", "s": s, "x": x, "y": y, "h": 40, "pad": 14, "size": 22, "bg": fill or BG, "outline": col or OUT, "color": col or WHITE, "font": FB}
def pw(s): return fnt.getlength(s) + 28
def badge(n, cx, cy): return [{"t": "dot", "x": cx, "y": cy, "r": 20, "color": G}, {"t": "dot", "x": cx, "y": cy, "r": 18, "color": FILL},
                              {"t": "text", "s": str(n), "x": cx, "y": cy, "anchor": "mm", "size": 22, "color": G, "font": F}]
def run(n, out, which, ann):
    spec = {"out": f"{OUTDIR}/{out}", "canvas": [1600, 600], "backdrop": BG,
            "windows": [{"src": f"captures/fo_{which}{SUF}.png", "chrome": "none", "x": 0, "y": 0, "w": 1600}], "annotations": ann}
    lay = f"layouts/foldout_{n}{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)

run("02", "foldout-variations.png", "variations", [])
run("03", "f-lv-tv.png", "flvtv", [pill("Tree view", 80, 176), pill("List view", 570, 176), pill("Foldout", 1060, 176)])

# content: header bracket + pill on the left, content bracket + pill on the right
r = R("content"); tx, ty, tw, th = r["foldout:Probes:text"]; ax = r["foldout:Probes:arrow"][0]; cx, cy, cw, ch = r["foldout:Probes:content"]
hy = ty + th / 2; bx = ax - 10
ann = [L(bx, ty, bx, ty + th), L(bx, ty, bx + 8, ty), L(bx, ty + th, bx + 8, ty + th), L(bx - 22, hy, bx, hy), pill("Foldout header", bx - 22 - pw("Foldout header"), hy, G, FILL)]
rx = cx + cw + 10; ann += [L(rx, cy, rx, cy + ch), L(rx - 8, cy, rx, cy), L(rx - 8, cy + ch, rx, cy + ch), L(rx, cy + ch / 2, rx + 22, cy + ch / 2), pill("Foldout content", rx + 22, cy + ch / 2, G, FILL)]
run("04", "foldout-content.png", "content", ann)

# anatomy: 1 header row, 2 arrow, 3 icon, 4 content
r = R("anatomy"); k = "foldout:Foldout Label"
ax, ay, aw, ah = r[k + ":arrow"]; ix, iy, iw, ih = r[k + ":icon"]; tx, ty, tw, th = r[k + ":text"]; cx, cy, cw, ch = r[k + ":content"]
BX = ax - 160
row_mid = ty + th / 2
ann = badge(1, BX, ay - 96) + [L(BX + 20, ay - 96, tx + 60, ay - 96), L(tx + 60, ay - 96, tx + 60, ty), {"t": "dot", "x": tx + 60, "y": ty, "r": 5, "color": G}]
ann += badge(2, BX, row_mid) + [L(BX + 20, row_mid, ax + 2, row_mid), {"t": "dot", "x": ax + 2, "y": row_mid, "r": 5, "color": G}]
ann += badge(3, BX, ay - 40) + [L(BX + 20, ay - 40, ix + iw / 2, ay - 40), L(ix + iw / 2, ay - 40, ix + iw / 2, iy), {"t": "dot", "x": ix + iw / 2, "y": iy, "r": 5, "color": G}]   # from above: the content labels sit right under the icon
rx = cx + cw + 14; ann += [L(rx, cy, rx, cy + ch), L(rx - 8, cy, rx, cy), L(rx - 8, cy + ch, rx, cy + ch), L(rx, cy + ch / 2, rx + 22, cy + ch / 2)] + badge(4, rx + 42, cy + ch / 2)
run("07", "foldout-anatomy.png", "anatomy", ann)

# indentation: arrow-offset strip, dashed guides at each level's labels, '15 px' brackets
r = R("indentation"); a1 = r["f1:Foldout Level 1:arrow"]; l1 = r["f1:Foldout Level 1:label0"]; l2 = r["f1:Foldout Level 2:label0"]; a2 = r["f1:Foldout Level 2:arrow"]
x0, x1, x2 = a1[0], l1[0], l2[0]; top = a1[1] - 20
ann = [{"t": "rect", "x": x0, "y": top, "w_": x1 - x0, "h": 600 - top, "color": RED, "w": 0, "fill": 0.28, "dash": False},
       {"t": "vdash", "x": x0, "y1": top, "y2": 600, "color": RED, "w": 1}, {"t": "vdash", "x": x1, "y1": top, "y2": 600, "color": RED, "w": 1}, {"t": "vdash", "x": x2, "y1": top, "y2": 600, "color": RED, "w": 1},
       pill("Foldout indentation", 40, 62), pill("Offset foldout arrow", (x0 + x1) / 2 - pw("Offset foldout arrow") / 2, 120, RED, [70, 40, 32]), L((x0 + x1) / 2, 140, (x0 + x1) / 2, top, RED)]
for (ya, xa, xb) in ((l1[1] + l1[3] / 2, x0, x1), (l2[1] + l2[3] / 2, x1, x2)):
    ann += [L(xa, ya, xb, ya, RED), L(xa, ya - 7, xa, ya + 7, RED), L(xb, ya - 7, xb, ya + 7, RED),
            {"t": "text", "s": "15 px", "x": xa - 8, "y": ya, "anchor": "rm", "size": 20, "color": RED, "font": F}]
run("05", "foldout-indentation.png", "indentation", ann)
