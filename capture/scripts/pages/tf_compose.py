"""Components > Text field: compose the dark images from unity/tf_spec.cs (captures/tf_<which>.png, rects + box model in C:/tmp/tf_<which>.json)
at 2x. Margin (red) and padding (green) strips are drawn from each field's resolved margins / input border+padding, not copied from the original.
#3 is the same image as Fundamentals > Color palette 'Accent Borders' (layouts/color-palette_02.json)."""
import os as _os; _EFDS_TMP = _os.environ.get("EFDS_TMP", "C:/tmp")
def _efds_font(n):
    # Windows fonts when present; elsewhere the bundled Inter (capture/fonts) or the system monospace font
    import os as _o
    w = "C:/Windows/Fonts/" + n
    if _o.path.exists(w): return w
    d = _o.path.dirname(_o.path.abspath(__file__))
    while not _o.path.isdir(_o.path.join(d, "fonts")) and _o.path.dirname(d) != d: d = _o.path.dirname(d)
    mono = [p for p in ("/System/Library/Fonts/Menlo.ttc", "/usr/share/fonts/truetype/dejavu/DejaVuSansMono-Bold.ttf") if _o.path.exists(p)]
    if n.startswith("consola") and mono: return mono[0]
    return _o.path.join(d, "fonts", "Inter-SemiBold.ttf" if n in ("segoeuib.ttf", "seguisb.ttf", "consolab.ttf") else "Inter-Regular.ttf")
import json, os, subprocess, sys
from PIL import ImageFont
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, G, FILL, OUT, BG, INK, themed
F, FB, MONO = "fonts/Inter-Regular.ttf", "fonts/Inter-SemiBold.ttf", _efds_font("consolab.ttf")
RED, GRN = ([205, 80, 40], [70, 150, 40]) if LIGHT else ([255, 110, 64], [178, 255, 89])
OUTDIR = "replacements/components/text-field"; os.makedirs(OUTDIR, exist_ok=True)
def R(w): return {k: [v * 2 for v in r] for k, r in json.load(open(f"{_EFDS_TMP}/tf_{w}{SUF}.json")).items()}
L = lambda a, b, c, d, col=G, w=2: {"t": "line", "x1": a, "y1": b, "x2": c, "y2": d, "color": col, "w": w}
PF = ImageFont.truetype(FB, 22)
def pill(s, x, y, bg=BG, outline=OUT, color=None, font=FB):
    color = color or INK
    return {"t": "pill", "s": s, "x": x, "y": y, "h": 40, "pad": 14, "size": 22, "bg": list(bg), "outline": list(outline), "color": list(color), "font": font}
def badge(n, cx, cy): return [{"t": "dot", "x": cx, "y": cy, "r": 20, "color": G}, {"t": "dot", "x": cx, "y": cy, "r": 18, "color": FILL},
                              {"t": "text", "s": str(n), "x": cx, "y": cy, "anchor": "mm", "size": 22, "color": G, "font": F}]
box = lambda x, y, w, h, col, a=0.45: {"t": "rect", "x": x, "y": y, "w_": max(w, 0), "h": max(h, 0), "color": col, "w": 0, "fill": a if w > 0 and h > 0 else 0, "dash": False}
def run(n, out, which, ann, canvas=(1600, 600)):
    spec = {"out": f"{OUTDIR}/{out}", "canvas": list(canvas), "backdrop": BG,
            "windows": [{"src": f"captures/tf_{which}{SUF}.png", "chrome": "none", "x": 0, "y": 0, "w": canvas[0]}], "annotations": ann}
    lay = f"layouts/text-field_{n}{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
def strips(r, k, vertical=False):   # red = the field's margins (+ label/input gap), green = the input's border+padding
    fx, fy, fw, fh = r[k]; ml, mt, mr, mb = r[k + ":margin"]; ix, iy, iw, ih = r[k + ":input"]; pl, pt, pr, pb = r[k + ":inputpad"]
    if not vertical:
        lx, ly, lw, lh = r[k + ":label"]
        return [box(fx - ml, iy, ml, ih, RED), box(lx + lw, iy, ix - lx - lw, ih, RED), box(fx + fw, iy, mr, ih, RED),
                box(ix, iy, pl, ih, GRN, 0.7), box(ix + iw - pr, iy, pr, ih, GRN, 0.7)]
    return [L(fx - ml - 24, fy - mt, fx + fw + mr, fy - mt, RED, 2), L(fx - ml - 24, fy + fh + mb, fx + fw + mr, fy + fh + mb, RED, 2),
            L(ix, iy + pt, ix + iw, iy + pt, GRN, 2), L(ix, iy + ih - pb, ix + iw, iy + ih - pb, GRN, 2)]

# 02 variations: 1 UI Toolkit TextField (single or multi-line), 2a IMGUI TextField, 2b IMGUI TextArea
num = lambda s, x, y: [{"t": "dot", "x": x, "y": y, "r": 17, "color": OUT}, {"t": "dot", "x": x, "y": y, "r": 15, "color": BG},
                       {"t": "text", "s": s, "x": x, "y": y, "anchor": "mm", "size": 20, "color": INK, "font": FB}]
ann = [L(800, 0, 800, 600, OUT, 1), L(800, 296, 1600, 296, OUT, 1)] + num("1", 40, 62) + num("2a", 830, 62) + num("2b", 830, 358)
ann += [pill("UIT", 24, 532, *(((235, 225, 250),) * 2 if LIGHT else ((25, 26, 27),) * 2), (128, 42, 215), MONO), pill("imGUI", 808, 532, *(((250, 225, 232),) * 2 if LIGHT else ((25, 26, 27),) * 2), (218, 33, 98), MONO)]
run("02", "textfield-variations.png", "variations", ann)

# 03 inset / outset borders: same image as Color palette 'Accent Borders' (light: that page's _light layout)
spec = json.load(open(f"layouts/color-palette_02{LAY}.json")); spec["out"] = f"{OUTDIR}/textfield-style.png"
json.dump(spec, open(f"layouts/text-field_03{LAY}.json", "w"), indent=1); subprocess.run([sys.executable, "scripts/compose.py", f"layouts/text-field_03{LAY}.json"], check=True, stdout=subprocess.DEVNULL)

# 04 / 05 behaviour: multi-line message (do) vs truncated single line (don't); panel on a darker page like the original
for n, w, out in (("04", "lapY", "textfield-behavior-Y img.png"), ("05", "lapN", "textfield-behavior-N img.png")):
    px, py, pw, ph = R(w)["panel"]; PAGE = [180, 180, 180] if LIGHT else [25, 26, 27]; x0, x1 = px - 8, px + pw + 8
    run(n, out, w, [box(0, 0, 780, py - 2, PAGE, 1), box(0, 0, x0, 480, PAGE, 1), box(x1, 0, 780 - x1, 480, PAGE, 1)], (780, 480))

# 06 anatomy: 1 label, 2 input text, 3 field
r = R("anatomy"); lx, ly, lw, lh = r["f:label"]; tx, ty, tw, th = r["f:text"]; ix, iy, iw, ih = r["f:input"]; by = 400
ann = [L(lx, ly + lh + 4, lx + lw, ly + lh + 4), L(lx + lw / 2, ly + lh + 4, lx + lw / 2, by - 20)] + badge(1, lx + lw / 2, by)
ann += [L(tx + 8, ty + th - 6, tx + tw * 0.62, ty + th - 6), L(tx + tw * 0.3, ty + th - 6, tx + tw * 0.3, by - 20)] + badge(2, tx + tw * 0.3, by)
ann += [{"t": "dot", "x": ix + iw * 0.82, "y": iy + ih - 1, "r": 5, "color": G}, L(ix + iw * 0.82, iy + ih - 1, ix + iw * 0.82, by - 20)] + badge(3, ix + iw * 0.82, by)
run("06", "textfield-anatomy.png", "anatomy", ann)

# 07 margins (red) and padding (green)
r = R("margins")
run("07", "textfield-margin-paddings.png", "margins", strips(r, "f") + [pill("Margins", 40, 498, (240, 216, 206) if LIGHT else (86, 64, 57), RED), pill("Padding", 180, 498, (205, 225, 190) if LIGHT else (74, 86, 61), GRN)])

# 08 / 09 horizontal and vertical scaling (second, disabled row of each box carries the guides)
r = R("hscale"); run("08", "textfield-horizontal scaling.png", "hscale", [pill("Horizontal Scaling \u2192", 40, 62)] + strips(r, "narrow_f2") + strips(r, "wide_f2"))
r = R("vscale"); run("09", "textfield-vertical scaling.png", "vscale", [pill("Vertical Scaling \u2193", 40, 62)] + strips(r, "short_f2", True) + strips(r, "tall_f2", True))


# Two-theme images (built on the light run): light half left, dark half right
if LIGHT:
    DG, DF = [178, 255, 89], [48, 60, 36]          # the dark half keeps the dark callout colours
    def two(n, out, which, ann):
        if not os.path.exists(f"captures/tf_{which}.png"): print("dark half missing:", which); return
        spec = {"out": f"{OUTDIR}/{out}", "canvas": [1600, 600], "backdrop": [194, 194, 194],
                "windows": [{"src": f"captures/tf_{which}_l.png", "chrome": "none", "x": 0, "y": 0, "w": 800}, {"src": f"captures/tf_{which}.png", "chrome": "none", "x": 800, "y": 0, "w": 800}], "annotations": ann}
        json.dump(spec, open(f"layouts/text-field_{n}.json", "w"), indent=1)
        subprocess.run([sys.executable, "scripts/compose.py", f"layouts/text-field_{n}.json"], check=True, stdout=subprocess.DEVNULL)
    two("01", "textfield-overview.png", "overview", [])
    two("10", "textfield-states.png", "states", [])
    r = {k: [v * 2 for v in vv] for k, vv in json.load(open(_EFDS_TMP + "/tf_themes.json")).items()}
    tx, ty, tw, th = r["f:text"]; ix, iy, iw, ih = r["f:input"]; by = iy + ih + 70; ox = 800
    def bdg(n_, cx, cy): return [{"t": "dot", "x": cx, "y": cy, "r": 20, "color": DG}, {"t": "dot", "x": cx, "y": cy, "r": 18, "color": DF},
                                {"t": "text", "s": str(n_), "x": cx, "y": cy, "anchor": "mm", "size": 22, "color": DG, "font": F}]
    ln = lambda a, b, c, d: {"t": "line", "x1": a, "y1": b, "x2": c, "y2": d, "color": DG, "w": 2}
    ann = [ln(ox + tx + 6, ty + th - 4, ox + tx + tw * 0.55, ty + th - 4)] + [ln(ox + tx + tw * 0.25, ty + th - 4, ox + tx + tw * 0.25, by - 20)] + bdg(1, ox + tx + tw * 0.25, by)
    for n_, (x_, y_) in ((2, (ix + iw * 0.6, iy + ih / 2)), (3, (ix + iw * 0.82, iy + ih - 1))):
        ann += [{"t": "dot", "x": ox + x_, "y": y_, "r": 5, "color": DG}, ln(ox + x_, y_, ox + x_, by - 20)] + bdg(n_, ox + x_, by)
    two("11", "textfield-themes.png", "themes", ann)
