"""Components > Numeric field: compose the dark images from unity/nf_spec.cs (captures/nf_<which>.png, rects C:/tmp/nf_<which>.json) at 2x
with the original's pills, dividers and callouts."""
import os as _os; _EFDS_TMP = _os.environ.get("EFDS_TMP", "C:/tmp")
import json, os, subprocess, sys
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from ov_callouts import G, F as FILL
F, FB = "fonts/Inter-Regular.ttf", "fonts/Inter-SemiBold.ttf"; OUT, BG, RED = [86, 87, 88], [50, 50, 50], [255, 110, 64]
OUTDIR = "replacements/components/numeric-field"; os.makedirs(OUTDIR, exist_ok=True)
LIGHT = os.environ.get("THEME") == "light"            # THEME=light: light-skin captures (nf_<which>_l.png) and light layouts
SUF = "_l" if LIGHT else ""
if LIGHT: OUT, BG, G, FILL = [190, 190, 190], [200, 200, 200], [77, 140, 80], [182, 191, 182]   # callout green / fill from the originals' light halves
def R(w): return {k: [v * 2 for v in r] for k, r in json.load(open(f"{_EFDS_TMP}/nf_{w}{SUF}.json")).items()}
L = lambda a, b, c, d, col=G, w=2: {"t": "line", "x1": a, "y1": b, "x2": c, "y2": d, "color": col, "w": w}
pill = lambda s, x, y: {"t": "pill", "s": s, "x": x, "y": y, "h": 40, "pad": 14, "size": 22, "bg": BG, "outline": OUT, "color": [9, 9, 9] if LIGHT else [255, 255, 255], "font": FB}
def badge(n, cx, cy): return [{"t": "dot", "x": cx, "y": cy, "r": 20, "color": G}, {"t": "dot", "x": cx, "y": cy, "r": 18, "color": FILL},
                              {"t": "text", "s": str(n), "x": cx, "y": cy, "anchor": "mm", "size": 22, "color": G, "font": F}]
def run(n, out, which, canvas, ann):
    spec = {"out": f"{OUTDIR}/{out}", "canvas": canvas, "backdrop": BG,
            "windows": [{"src": f"captures/nf_{which}{SUF}.png", "chrome": "none", "x": 0, "y": 0, "w": canvas[0]}], "annotations": ann}
    if LIGHT: spec["theme"] = "light"
    lay = f"layouts/numeric-field_{n}{'_light' if LIGHT else ''}.json"
    json.dump(spec, open(lay, "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)

run("03", "SingleField.png", "single", [840, 200], [])
run("04", "MultiField.png", "multi", [840, 200], [])
run("02", "num_field-variations.png", "variations", [1600, 600], [L(800, 0, 800, 600, OUT)])
run("05", "num_field-positioning.png", "positioning", [1600, 600],
    [L(800, 0, 800, 600, OUT), pill("Basic Slider with a Numeric Field", 40, 60), pill("Min/Max Slider with 2 Numeric Fields", 840, 60)])

# anatomy: 1 label, 2 value, 3 field
r = R("anatomy"); lx, ly, lw, lh = r["f:label"]; ix, iy, iw, ih = r["f:input"]; tx, ty, tw, th = r["f:text"]
by = iy + ih + 70; ub = ly + lh - 6
ann = [L(lx + 16, ub, lx + 120, ub), L(lx + 60, ub, lx + 60, by - 20)] + badge(1, lx + 60, by)
ann += [L(tx + 6, ty + th - 2, tx + tw * 0.75, ty + th - 2), L(tx + tw * 0.3, ty + th - 2, tx + tw * 0.3, by - 20)] + badge(2, tx + tw * 0.3, by)
ann += [{"t": "dot", "x": ix + iw - 8, "y": iy + ih - 2, "r": 5, "color": G}, L(ix + iw - 8, iy + ih - 2, ix + iw - 8, by - 20)] + badge(3, ix + iw - 8, by)
run("06", "num_field-anatomy.png", "anatomy", [1600, 600], ann)

# layout: horizontal scaling - shade the label column and mark the field's edges on each box's second row
r = R("layout"); ann = [pill("Horizontal Scaling \u2192", 40, 60)]
for k in ("narrow", "wide"):
    bx, by_, bw, bh = r[k]; lx, ly, lw, lh = r[k + "_f2:label"]; ix, iy, iw, ih = r[k + "_f2:input"]
    ann += [{"t": "rect", "x": bx, "y": ly, "w_": ix - bx, "h": lh, "color": RED, "w": 0, "fill": 0.45, "dash": False},
            L(ix, ly - 6, ix, ly + lh + 6), L(ix + iw - 2, ly - 6, ix + iw - 2, ly + lh + 6)]
run("07", "num_field-layout.png", "layout", [1600, 600], ann)
