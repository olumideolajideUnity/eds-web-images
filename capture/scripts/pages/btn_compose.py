"""Components > Button: compose the dark specimens from unity/btn_spec.cs (captures/btn_<which>.png, rects in C:/tmp/btn_<which>.json)
with the original's labels and callouts (badges, pills, height and radius callouts), at 2x.
Measured in Unity 7: IMGUI miniButton 18pt, button 19pt, LargeButton 24pt tall; IMGUI button corners about 3pt."""
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
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, G, FILL, OUT as TOUT, BG as TBG, INK, TXT, PAGE as TPAGE, themed
F, FB, FM = "fonts/Inter-Regular.ttf", "fonts/Inter-SemiBold.ttf", _efds_font("consolab.ttf")
WHITE, OUT, BG = INK, TOUT, TBG
OUTDIR = "replacements/components/button"
def R(which): return {k: [v * 2 for v in r] for k, r in json.load(open(f"{_EFDS_TMP}/btn_{which}{SUF}.json")).items()}   # rects in canvas px (2x)
def run(n, out, canvas, src, ann, w=None):
    spec = {"out": f"{OUTDIR}/{out}", "canvas": canvas, "backdrop": BG,
            "windows": [{"src": f"captures/btn_{src}{SUF}.png", "chrome": "none", "x": 0, "y": 0, "w": w or canvas[0]}], "annotations": ann}
    lay = f"layouts/button_{n}{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
def badge(s, cx, cy, r=18): return [{"t": "dot", "x": cx, "y": cy, "r": r, "color": OUT}, {"t": "dot", "x": cx, "y": cy, "r": r - 2, "color": BG},
                                    {"t": "text", "s": s, "x": cx, "y": cy, "anchor": "mm", "size": 22, "color": WHITE, "font": F}]
def pill(s, x, y): return {"t": "pill", "s": s, "x": x, "y": y, "h": 44, "pad": 16, "size": 22, "bg": BG, "outline": OUT, "color": WHITE, "font": FB}
L = lambda a, b, c, d, col=G, w=2: {"t": "line", "x1": a, "y1": b, "x2": c, "y2": d, "color": col, "w": w}
os.makedirs(OUTDIR, exist_ok=True)

# 1 variations: badges 1 / 2a / 2b, divider, 'imGUI only' tag
ann = badge("1", 64, 64) + badge("2a", 864, 64) + badge("2b", 864, 364) + [L(800, 0, 800, 600, OUT, 2),
       {"t": "text", "s": "imGUI only", "x": 1480, "y": 541, "anchor": "mm", "size": 24, "color": [218, 33, 98], "font": FM, "bg": [250, 225, 232] if LIGHT else [25, 26, 27]}]
run("01", "buttons-variations.png", [1600, 600], "variations", ann)
# 3 basic, 5 toggleDe
run("03", "buttons-basic.png", [840, 200], "basic", [])
run("05", "buttons-toggleDe.png", [840, 200], "toggleDe", [])
# 8 sizes: dashed guides on each button's top/bottom, height brackets left and right
r = R("sizes"); ann = [pill("Button Sizes", 40, 62)]
for k, label in (("mini", "18 px"), ("default", "19 px"), ("large", "24 px")):
    x, y, w, h = r[k]; y0, y1 = y, y + h
    ann += [{"t": "hdash", "x1": 0, "x2": 1600, "y": y0, "color": OUT, "w": 1}, {"t": "hdash", "x1": 0, "x2": 1600, "y": y1, "color": OUT, "w": 1}]
    for bx, side in ((432, -1), (1166, 1)):
        ann += [L(bx, y0, bx, y1), L(bx - 6, y0, bx + 6, y0), L(bx - 6, y1, bx + 6, y1),
                {"t": "text", "s": label, "x": bx - 10 * (1 if side < 0 else -1), "y": (y0 + y1) / 2, "anchor": "rm" if side < 0 else "lm", "size": 22, "color": G, "font": F}]
run("08", "buttons-style-size.png", [1600, 600], "sizes", ann)
# 9 grouping: radius brackets at the corners of the separate buttons, '3 px round' labels outside the end ones
r = R("grouping"); ann = [pill("Button Grouping", 40, 62)]
for k in ("sl", "sm", "sr"):
    x, y, w, h = r[k]; x0, y0, x1, y1 = x - 5, y - 5, x + w + 5, y + h + 5
    for cx, cy, dx, dy in ((x0, y0, 1, 1), (x1, y0, -1, 1), (x0, y1, 1, -1), (x1, y1, -1, -1)):
        ann += [L(cx, cy, cx + 8 * dx, cy), L(cx, cy, cx, cy + 8 * dy)]
xs, ys, ws, hs = r["sl"]; xe, ye, we, he = r["sr"]
ann += [{"t": "text", "s": "3 px\nround", "x": xs - 14, "y": ys + hs / 2 - 12, "anchor": "rm", "size": 22, "color": G, "font": F},
        {"t": "text", "s": "3 px\nround", "x": xe + we + 14, "y": ye + he / 2 - 12, "anchor": "lm", "size": 22, "color": G, "font": F}]
run("09", "buttons-style-grouping.png", [1600, 600], "grouping", ann)

# ---- part 2: anatomy, colour diagrams, layout specs, Buttons.png ----
def callouts(points, by):          # numbered badges below, each with an elbow leader up to its target point
    ann = []
    for n, (tx, ty, bx) in enumerate(points, 1):
        ann += [{"t": "dot", "x": tx, "y": ty, "r": 5, "color": G}]
        if abs(tx - bx) < 1: ann += [L(bx, by - 20, bx, ty)]
        else: ey = max(ty, (ty + by) / 2); ann += [L(tx, ty, tx, ey), L(tx, ey, bx, ey), L(bx, ey, bx, by - 20)]
        ann += [{"t": "dot", "x": bx, "y": by, "r": 20, "color": G}, {"t": "dot", "x": bx, "y": by, "r": 18, "color": FILL},
                {"t": "text", "s": str(n), "x": bx, "y": by, "anchor": "mm", "size": 22, "color": G, "font": F}]
    return ann
r = R("anatomy"); lx, ly, lw, lh = r["large"]; ix, iy, iw, ih = r["icon"]
ann = callouts([(lx + 36, ly + lh * 0.55, lx + 36), (ix + iw * 0.5, iy + ih * 0.55, ix + iw * 0.5)], ly + lh + 80)
by = ly + lh + 30                                     # 3: container bracket joining both buttons' bottoms
ann += [L(lx + lw * 0.6, ly + lh, lx + lw * 0.6, by), L(ix + iw * 0.5 + 40, iy + ih, ix + iw * 0.5 + 40, by), L(lx + lw * 0.6, by, ix + iw * 0.5 + 40, by)]
mx = (lx + lw * 0.6 + ix + iw * 0.5 + 40) / 2
ann += [L(mx, by, mx, ly + lh + 60)] + [{"t": "dot", "x": mx, "y": ly + lh + 80, "r": 20, "color": G}, {"t": "dot", "x": mx, "y": ly + lh + 80, "r": 18, "color": FILL},
        {"t": "text", "s": "3", "x": mx, "y": ly + lh + 80, "anchor": "mm", "size": 22, "color": G, "font": F}]
run("11", "buttons-anatomy.png", [1600, 600], "anatomy", ann)
for n, which, out in (("15", "basic_theme", "basic_buttons-themes.png"), ("16", "toggle_theme", "toggle_buttons-themes.png"), ("17", "toolbar_theme", "toolbar_buttons-themes.png")):
    x, y, w, h = R(which)["b"]
    cx = x + w / 2; spread = max(w * 0.34, 70)
    pts = [(x + 6, y + h * 0.7, cx - spread), (x + w * 0.28, y + h - 4, cx), (x + w * 0.62, y + h * 0.62, cx + spread)]
    run(n, out, [1600, 600], which, callouts(pts, y + h + 70))
# layout specs: heights (green, left) and vertical padding from the styles (red, right)
r = R("layout"); RED = [255, 110, 64]; ann = []
pads = {"mini": "0 / 1 px", "default": "2 / 2 px", "large": "2 / 3 px"}
for k, label in (("mini", "18 px"), ("default", "19 px"), ("large", "24 px")):
    x, y, w, h = r[k]; y0, y1 = y, y + h; bx = 520
    ann += [{"t": "hdash", "x1": 0, "x2": 1600, "y": y0, "color": OUT, "w": 1}, {"t": "hdash", "x1": 0, "x2": 1600, "y": y1, "color": OUT, "w": 1},
            L(bx, y0, bx, y1), L(bx - 6, y0, bx + 6, y0), L(bx - 6, y1, bx + 6, y1),
            {"t": "text", "s": label, "x": bx - 10, "y": (y0 + y1) / 2, "anchor": "rm", "size": 22, "color": G, "font": F},
            L(1030, y0, 1030, y0 + 6, RED), L(1030, y1 - 6, 1030, y1, RED), L(1024, y0, 1036, y0, RED), L(1024, y1, 1036, y1, RED),
            {"t": "text", "s": pads[k], "x": 1046, "y": (y0 + y1) / 2, "anchor": "lm", "size": 22, "color": RED, "font": F}]
ann += [{"t": "pill", "s": "Height", "x": 40, "y": 538, "h": 40, "pad": 14, "size": 22, "bg": FILL, "outline": G, "color": G, "font": FB},
        {"t": "pill", "s": "Padding (top / bottom)", "x": 170, "y": 538, "h": 40, "pad": 14, "size": 22, "bg": [240, 216, 206] if LIGHT else [70, 40, 32], "outline": RED, "color": RED, "font": FB}]
run("12", "buttons-layout-specs.png", [1600, 600], "layout", ann)
# Buttons.png: the focused button and the real window it opens, with OS frame (title 'Export Coordinates')
spec = {"out": f"{OUTDIR}/Buttons.png", "canvas": [1600, 600], "backdrop": BG,
        "windows": [{"src": f"captures/btn_export{SUF}.png", "chrome": "none", "x": 0, "y": 0, "w": 1600},
                    {"src": f"captures/btn_empty{SUF}.png", "title": "Export Coordinates", "x": 592, "y": 248, "w": 826, "h": None}], "annotations": []}
json.dump(themed(spec), open(f"layouts/button_10{LAY}.json", "w"), indent=1)
subprocess.run([sys.executable, "scripts/compose.py", f"layouts/button_10{LAY}.json"], check=True, stdout=subprocess.DEVNULL)
