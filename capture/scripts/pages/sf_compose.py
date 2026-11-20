"""Components > Search field: compose the dark images from unity/sf_spec.cs (captures/sf_<which>.png, rects C:/tmp/sf_<which>.json) at 2x.
The popup search field's menu opens as an owner-drawn native menu (Unity can't capture it), drawn with scripts/pages/native_menu.py
import os as _os; _EFDS_TMP = _os.environ.get("EFDS_TMP", "C:/tmp")
from its real items (captures/menus/sf_popup.txt). No mouse pointers."""
import json, os, subprocess, sys
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from native_menu import draw
from theme import LIGHT, SUF, LAY, G, FILL, OUT, BG, INK, TXT, themed
F, FB = "fonts/Inter-Regular.ttf", "fonts/Inter-SemiBold.ttf"; RED = [255, 110, 64]
MENU = dict(bg=(235, 235, 235), text=(9, 9, 9), highlight=(214, 214, 214)) if LIGHT else {}   # light owner-drawn menu colours (as the home hero)
OUTDIR = "replacements/components/search-field"; os.makedirs(OUTDIR, exist_ok=True)
def R(w): return {k: [v * 2 for v in r] for k, r in json.load(open(f"{_EFDS_TMP}/sf_{w}{SUF}.json")).items()}
L = lambda a, b, c, d, col=OUT, w=2: {"t": "line", "x1": a, "y1": b, "x2": c, "y2": d, "color": col, "w": w}
pill = lambda s, x, y: {"t": "pill", "s": s, "x": x, "y": y, "h": 40, "pad": 14, "size": 22, "bg": BG, "outline": OUT, "color": INK, "font": FB}
label = lambda s, x, y: {"t": "text", "s": s, "x": x, "y": y, "size": 24, "color": TXT, "font": F}
def run(n, out, which, ann, canvas=(1600, 600)):
    spec = {"out": f"{OUTDIR}/{out}", "canvas": list(canvas), "backdrop": BG,
            "windows": [{"src": f"captures/sf_{which}{SUF}.png", "chrome": "none", "x": 0, "y": 0, "w": canvas[0]}], "annotations": ann}
    lay = f"layouts/search-field_{n}{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)

# 01 / 02 overview (02 is the page's near-duplicate of 01): regular and popup toolbar search fields, empty and in use; popup menu open
r = R("overview"); _, iy, _, ih = r["p2:input"]; ix = r["p2:search"][0] - 6; iw = r["p2:cancel"][0] + r["p2:cancel"][2] - ix   # whole field (icon to cancel)
img, pad = draw("captures/menus/sf_popup.txt", 4, min_width=iw / 2, **MENU); img.save(f"captures/menus/sf_popup{SUF}.png")
menu = {"t": "image", "src": f"captures/menus/sf_popup{SUF}.png", "x": ix - pad / 2, "y": iy + ih + 2 - pad / 2, "w": img.width / 2}
ann = [L(800, 0, 800, 600, OUT, 1), pill("Basic Search Field Variations", 40, 62), label("Toolbar - Regular Search Field", 56, 136),
       label("Toolbar - Popup Search Field", 848, 136), menu]
run("01", "ext-overview.png", "overview", ann)
run("02", "Basic Search Field Variations.png", "overview", ann)

# 03 / 04 single toolbar search field, popup search field
run("03", "ToolbarSearchField.png", "single", [], (840, 200))
run("04", "ToolbarPopupSearchField.png", "popup", [], (840, 200))

# 05 recommended minimum width: 150px (dashed guides at the field's edges, dimension line below)
r = R("length"); x, y, w, h = r["f"]; dy = y + h + 22
ann = [pill("Recommended min: 150px", 40, 62),
       {"t": "vdash", "x": x, "y1": 0, "y2": 600, "color": RED, "w": 1}, {"t": "vdash", "x": x + w, "y1": 0, "y2": 600, "color": RED, "w": 1},
       L(x, dy, x + w, dy, RED, 2), L(x, dy - 8, x, dy + 8, RED, 2), L(x + w, dy - 8, x + w, dy + 8, RED, 2),
       {"t": "text", "s": "150 px", "x": x + w / 2, "y": dy + 32, "anchor": "mm", "size": 22, "color": RED, "font": FB}]
run("05", "searchfield-length.png", "length", ann)


# Two-theme images (built on the light run): light half left, dark half right
if LIGHT:
    DG, DF = [178, 255, 89], [48, 60, 36]          # the dark half keeps the dark callout colours
    def two(n, out, which, ann):
        spec = {"out": f"{OUTDIR}/{out}", "canvas": [1600, 600], "backdrop": [194, 194, 194],
                "windows": [{"src": f"captures/sf_{which}_l.png", "chrome": "none", "x": 0, "y": 0, "w": 800}, {"src": f"captures/sf_{which}.png", "chrome": "none", "x": 800, "y": 0, "w": 800}], "annotations": ann}
        json.dump(spec, open(f"layouts/search-field_{n}.json", "w"), indent=1)
        subprocess.run([sys.executable, "scripts/compose.py", f"layouts/search-field_{n}.json"], check=True, stdout=subprocess.DEVNULL)
    two("10", "searchfield-states.png", "states", [])
    # colour: 1 icon, 2 text, 3 field border, 4 field background, on the dark half (as the original)
    r = {k: [v * 2 for v in vv] for k, vv in json.load(open(_EFDS_TMP + "/sf_color.json")).items()}
    ox = 800; sx, sy, sw, sh = r["f:search"]; tx, ty, tw, th = r["f:text"]; ix, iy, iw, ih = r["f:input"]; by = iy + ih + 70
    def dbadge(n_, cx, cy): return [{"t": "dot", "x": cx, "y": cy, "r": 20, "color": DG}, {"t": "dot", "x": cx, "y": cy, "r": 18, "color": DF},
                                   {"t": "text", "s": str(n_), "x": cx, "y": cy, "anchor": "mm", "size": 22, "color": DG, "font": F}]
    DL = lambda a, b, c, d: {"t": "line", "x1": a, "y1": b, "x2": c, "y2": d, "color": DG, "w": 2}
    ann = []
    for n_, (x_, y_) in enumerate(((sx + sw / 2, sy + sh / 2), (tx + tw * 0.25, ty + th - 4), (ix + iw * 0.68, iy + ih - 1), (ix + iw * 0.86, iy + ih / 2)), 1):
        ann += [{"t": "dot", "x": ox + x_, "y": y_, "r": 5, "color": DG}, DL(ox + x_, y_, ox + x_, by - 20)] + dbadge(n_, ox + x_, by)
    two("12", "searchfield-color.png", "color", ann)
