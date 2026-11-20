"""Components > Slider: compose the dark images from unity/sl_spec.cs (captures/sl_<which>.png, rects C:/tmp/sl_<which>.json) at 2x.
Focus is real keyboard focus (pseudo-states don't stick on sliders); only one control can hold it, so the variations image splices
two captures (basic slider focused / min-max slider focused). Unity 7 sliders draw no hover style on the thumb (the Hover pseudo-state
is set, nothing changes). MinMaxSlider(minValue, maxValue, lowLimit, highLimit)."""
import os as _os; _EFDS_TMP = _os.environ.get("EFDS_TMP", "C:/tmp")
import json, os, subprocess, sys
from PIL import Image
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, G, FILL, OUT, BG, PAGE as TPAGE, themed
F, FB = "fonts/Inter-Regular.ttf", "fonts/Inter-SemiBold.ttf"; K = 2.5
OUTDIR = "replacements/components/slider"; os.makedirs(OUTDIR, exist_ok=True)
def R(w): return {k: [v * 2 for v in r] for k, r in json.load(open(f"{_EFDS_TMP}/sl_{w}{SUF}.json")).items()}
L = lambda a, b, c, d, col=G, w=2: {"t": "line", "x1": a, "y1": b, "x2": c, "y2": d, "color": col, "w": w}
def badge(n, cx, cy): return [{"t": "dot", "x": cx, "y": cy, "r": 20, "color": G}, {"t": "dot", "x": cx, "y": cy, "r": 18, "color": FILL},
                              {"t": "text", "s": str(n), "x": cx, "y": cy, "anchor": "mm", "size": 22, "color": G, "font": F}]
def run(n, out, src, ann, canvas=(1600, 600), backdrop=BG):
    spec = {"out": f"{OUTDIR}/{out}", "canvas": list(canvas), "backdrop": backdrop,
            "windows": [{"src": src, "chrome": "none", "x": 0, "y": 0, "w": canvas[0]}], "annotations": ann}
    lay = f"layouts/slider_{n}{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)

# 01 variations: left panel from the basic-slider-focused capture, right panel from the min/max-focused one; page darker than the panels
a = Image.open(f"captures/sl_variations_s2{SUF}.png").convert("RGB"); b = Image.open(f"captures/sl_variations_m2{SUF}.png").convert("RGB")
half = int(400 * K); a.paste(b.crop((half, 0, b.width, b.height)), (half, 0)); a.save(f"captures/sl_variations_d{SUF}.png")
PAGE = TPAGE if LIGHT else [40, 40, 40]; fill = lambda x, y, w, h: {"t": "rect", "x": x, "y": y, "w_": w, "h": h, "color": PAGE, "w": 0, "fill": 1.0, "dash": False}
panels = [(116, 136, 664, 328), (838, 136, 664, 328)]          # original's panels (2x px)
ann = [fill(0, 0, 1600, 136), fill(0, 464, 1600, 136), fill(0, 136, 116, 328), fill(780, 136, 58, 328), fill(1502, 136, 98, 328)]
run("01", "slider-variations.png", f"captures/sl_variations_d{SUF}.png", ann)

# 02 / 03 slider alone, min/max slider alone
run("02", "basic-slider.png", f"captures/sl_basic{SUF}.png", [], (840, 200))
run("03", "minmax-slider.png", f"captures/sl_minmax{SUF}.png", [], (840, 200))

# 06 sliders with numeric fields: same image as Numeric field (layouts/numeric-field_05.json), written to this page
spec = json.load(open(f"layouts/numeric-field_05{LAY}.json")); spec["out"] = f"{OUTDIR}/num_field-positioning.png"
json.dump(spec, open(f"layouts/slider_06{LAY}.json", "w"), indent=1)
subprocess.run([sys.executable, "scripts/compose.py", f"layouts/slider_06{LAY}.json"], check=True, stdout=subprocess.DEVNULL)

# 07 style: labelled slider with its value field (2.5x)
run("07", "slider-style.png", f"captures/sl_style{SUF}.png", [])

# 09 anatomy: 1 label, 2 thumb, 3 track, 4 value field
r = R("style"); lx, ly, lw, lh = r["s:label"]; dx, dy, dw, dh = r["s:dragger"]; tx, ty, tw, th = r["s:tracker"]; ix, iy, iw, ih = r["s:input"]
by = 420; ub = ly + lh + 6
ann = [L(lx, ub, lx + lw * 0.9, ub), L(lx + lw * 0.45, ub, lx + lw * 0.45, by - 20)] + badge(1, lx + lw * 0.45, by)
cx, cy = dx + dw / 2, dy + dh / 2; rr = dw / 2 + 8
ann += [{"t": "rect", "x": cx - rr, "y": cy - rr, "w_": 2 * rr, "h": 2 * rr, "color": G, "w": 2, "dash": False, "solid": True, "radius": rr},
        L(cx, cy + rr, cx, by - 20)] + badge(2, cx, by)
px = tx + tw * 0.68; py = ty + th / 2
ann += [{"t": "dot", "x": px, "y": py, "r": 5, "color": G}, L(px, py, px, by - 20)] + badge(3, px, by)
fx = ix + iw / 2; fy = iy + ih
ann += [{"t": "dot", "x": fx, "y": fy, "r": 5, "color": G}, L(fx, fy, fx, by - 20)] + badge(4, fx, by)
run("09", "slider-anatomy.png", f"captures/sl_style{SUF}.png", ann)


# Two-theme images (built on the light run): light half left, dark half right
if LIGHT:
    DG, DF = [178, 255, 89], [48, 60, 36]          # the dark half keeps the dark callout colours
    import theme as T
    def two(n, out, which, ann):
        spec = {"out": f"{OUTDIR}/{out}", "canvas": [1600, 600], "backdrop": [194, 194, 194],
                "windows": [{"src": f"captures/sl_{which}_l.png", "chrome": "none", "x": 0, "y": 0, "w": 800}, {"src": f"captures/sl_{which}.png", "chrome": "none", "x": 800, "y": 0, "w": 800}], "annotations": ann}
        json.dump(spec, open(f"layouts/slider_{n}.json", "w"), indent=1)
        subprocess.run([sys.executable, "scripts/compose.py", f"layouts/slider_{n}.json"], check=True, stdout=subprocess.DEVNULL)
    r = {k: [v * 2 for v in vv] for k, vv in json.load(open(_EFDS_TMP + "/sl_states.json")).items()}
    lab = lambda s_, x_, y_, col: {"t": "text", "s": s_, "x": x_, "y": y_, "anchor": "lm", "size": 24, "color": col, "font": F}
    ann = [{"t": "line", "x1": 800, "y1": 0, "x2": 800, "y2": 600, "color": [120, 120, 120], "w": 1}]
    for k_, s_ in (("default", "Default"), ("hover", "Hover"), ("focus", "Focused")):
        x, y, w, h = r[k_]; ann += [lab(s_, x + w + 40, y + h / 2, [110, 110, 110]), lab(s_, 800 + x + w + 40, y + h / 2, [200, 200, 200])]
    two("11", "slider-states.png", "states", ann)
    r = {k: [v * 2 for v in vv] for k, vv in json.load(open(_EFDS_TMP + "/sl_themes.json")).items()}
    dx, dy, dw, dh = r["s:dragger"]; tx, ty, tw, th = r["s:tracker"]; by = dy + dh + 70
    def bdg(n_, cx, cy, g, f): return [{"t": "dot", "x": cx, "y": cy, "r": 20, "color": g}, {"t": "dot", "x": cx, "y": cy, "r": 18, "color": f},
                                       {"t": "text", "s": str(n_), "x": cx, "y": cy, "anchor": "mm", "size": 22, "color": g, "font": F}]
    ln = lambda a, b, c, d, g: {"t": "line", "x1": a, "y1": b, "x2": c, "y2": d, "color": g, "w": 2}
    cx, cy = dx + dw / 2, dy + dh / 2; rr = dw / 2 + 8; px = tx + tw * 0.62
    ann = [{"t": "rect", "x": cx - rr, "y": cy - rr, "w_": 2 * rr, "h": 2 * rr, "color": T.G, "w": 2, "dash": False, "solid": True, "radius": rr},
           ln(cx, cy + rr, cx, by - 20, T.G)] + bdg(1, cx, by, T.G, T.FILL)
    ann += [{"t": "dot", "x": px, "y": ty + th / 2, "r": 5, "color": T.G}, ln(px, ty + th / 2, px, by - 20, T.G)] + bdg(2, px, by, T.G, T.FILL)
    ann += [{"t": "dot", "x": 800 + cx, "y": dy + dh - 1, "r": 5, "color": DG}, ln(800 + cx, dy + dh - 1, 800 + cx, by - 20, DG)] + bdg(3, 800 + cx, by, DG, DF)
    two("12", "slider-themes.png", "themes", ann)
