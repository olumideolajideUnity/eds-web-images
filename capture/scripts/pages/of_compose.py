"""Components > Object field: anatomy and variations from real UI Toolkit ObjectFields (unity/of_spec.cs; captures/of_<which>.png,
rects C:/tmp/of_<which>.json) at 2x, with the original's callouts."""
import os as _os; _EFDS_TMP = _os.environ.get("EFDS_TMP", "C:/tmp")
import json, os, subprocess, sys
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, G, FILL, OUT as TOUT, BG as TBG, INK, TXT, PAGE as TPAGE, themed
F = "fonts/Inter-Regular.ttf"; BG = TBG
def R(w): return {k: [v * 2 for v in r] for k, r in json.load(open(f"{_EFDS_TMP}/of_{w}{SUF}.json")).items()}
L = lambda a, b, c, d: {"t": "line", "x1": a, "y1": b, "x2": c, "y2": d, "color": G, "w": 2}
def badge(n, cx, cy): return [{"t": "dot", "x": cx, "y": cy, "r": 20, "color": G}, {"t": "dot", "x": cx, "y": cy, "r": 18, "color": FILL},
                              {"t": "text", "s": str(n), "x": cx, "y": cy, "anchor": "mm", "size": 22, "color": G, "font": F}]
def run(n, out, which, ann):
    spec = {"out": f"replacements/components/object-field/{out}", "canvas": [1600, 600], "backdrop": BG,
            "windows": [{"src": f"captures/of_{which}{SUF}.png", "chrome": "none", "x": 0, "y": 0, "w": 1600}], "annotations": ann}
    lay = f"layouts/object-field_{n}{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
r = R("anatomy"); lx, ly, lw, lh = r["f:base-field__label"]; ix, iy, iw, ih = r["f:object-field__input"]
cx_, cy_, cw_, ch_ = r["f:object-field-display__icon"]; tx, ty, tw, th = r["f:object-field-display__label"]; sx, sy, sw, sh = r["f:object-field__selector"]
by = iy + ih + 90; b = iy + ih
ann = [L(lx + 4, b + 8, lx + 100, b + 8), L(lx + 52, b + 8, lx + 52, by - 20)] + badge(1, lx + 52, by)
ann += [{"t": "dot", "x": cx_ + cw_ / 2, "y": cy_ + ch_ - 4, "r": 5, "color": G}, L(cx_ + cw_ / 2, cy_ + ch_ - 4, cx_ + cw_ / 2, by - 20)] + badge(2, cx_ + cw_ / 2, by)
ann += [L(tx + 2, b + 8, tx + 100, b + 8), L(tx + 52, b + 8, tx + 52, by - 20)] + badge(3, tx + 52, by)
ann += [{"t": "dot", "x": sx + sw / 2, "y": sy + sh - 4, "r": 5, "color": G}, L(sx + sw / 2, sy + sh - 4, sx + sw / 2, by - 20)] + badge(4, sx + sw / 2, by)
ex = ix + iw + 24; ann += [L(ex, iy, ex, iy + ih), L(ex - 8, iy, ex, iy), L(ex - 8, iy + ih, ex, iy + ih), L(ex, iy + ih / 2, ex + 34, iy + ih / 2)] + badge(5, ex + 54, iy + ih / 2)
run("07", "object_fields-anatomy.png", "anatomy", ann)
run("02", "object_fields-variations.png", "variations", [])
