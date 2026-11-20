"""Components > Tree view: compose the dark images at 2x from a real floating one-column Project browser (unity/sv_proj.cs, 441pt wide,
Assets > Atrio > Character > Bird expanded; captures/tv_tree.png). Row geometry from Unity 7's TreeViewGUI constants (read live):
line height 16, top row margin 4, base indent 2, indent width 14, foldout width 15. #3 = Foldout #3, #4 = List view #4."""
import os as _os; _EFDS_TMP = _os.environ.get("EFDS_TMP", "C:/tmp")
import json, os, subprocess, sys
from PIL import ImageFont
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, G, FILL, OUT, BG, INK, themed
F, FB = "fonts/Inter-Regular.ttf", "fonts/Inter-SemiBold.ttf"; RED = [205, 80, 40] if LIGHT else [255, 110, 64]
OUTDIR = "replacements/components/tree-view"; os.makedirs(OUTDIR, exist_ok=True)
L = lambda a, b, c, d, col=G, w=2: {"t": "line", "x1": a, "y1": b, "x2": c, "y2": d, "color": col, "w": w}
GF = ImageFont.truetype(F, 20)
def gpill(s, x, y, right=False, col=G, fill=FILL):
    w = GF.getlength(s) + 24
    return {"t": "pill", "s": s, "x": x - w if right else x, "y": y, "h": 34, "pad": 12, "size": 20, "bg": fill, "outline": col, "color": col, "font": F}
pill = lambda s, x, y: {"t": "pill", "s": s, "x": x, "y": y, "h": 40, "pad": 14, "size": 22, "bg": BG, "outline": OUT, "color": INK, "font": FB}
def badge(n, cx, cy): return [{"t": "dot", "x": cx, "y": cy, "r": 20, "color": G}, {"t": "dot", "x": cx, "y": cy, "r": 18, "color": FILL},
                              {"t": "text", "s": str(n), "x": cx, "y": cy, "anchor": "mm", "size": 22, "color": G, "font": F}]
def run(n, out, wins, ann):
    spec = {"out": f"{OUTDIR}/{out}", "canvas": [1600, 600], "backdrop": BG, "windows": wins, "annotations": ann}
    lay = f"layouts/tree-view_{n}{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
def reuse(n, out, lay):
    lay = lay.replace(".json", LAY + ".json")
    if not os.path.exists(lay): print("no layout yet:", lay); return
    spec = json.load(open(lay)); spec["out"] = f"{OUTDIR}/{out}"; json.dump(spec, open(f"layouts/tree-view_{n}{LAY}.json", "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", f"layouts/tree-view_{n}{LAY}.json"], check=True, stdout=subprocess.DEVNULL)
WX, WW, PW, PH = 352, 882, 441, 276            # window x and width (canvas px), capture size (pt)
def proj(y): return {"src": f"captures/tv_tree{SUF}.png", "chrome": "none", "x": WX, "y": y, "w": WW}
X = lambda pt: WX + pt * 2
ROW0, LH, TREE_BOTTOM = 53, 16, 254            # first row top (pt, window coords incl. tab), row height, tree area bottom
row_y = lambda i, wy: wy + (ROW0 + i * LH) * 2

reuse("03", "f-lv-tv.png", "layouts/foldout_03.json")
reuse("04", "tv-lv_row-height.png", "layouts/list-view_04.json")

# 02 tree view and its items
wy = 88; r0, r1 = row_y(0, wy), row_y(1, wy)
ann = [L(X(0) - 12, r0, X(0) - 12, r1), L(X(0) - 12, r0, X(0) - 4, r0), L(X(0) - 12, r1, X(0) - 4, r1), L(X(0) - 12, (r0 + r1) / 2, X(0) - 40, (r0 + r1) / 2),
       gpill("TreeViewItem", X(0) - 40, (r0 + r1) / 2, True)]
t0, t1 = row_y(0, wy) - 8, wy + TREE_BOTTOM * 2
ann += [L(X(PW) + 12, t0, X(PW) + 12, t1), L(X(PW) + 4, t0, X(PW) + 12, t0), L(X(PW) + 4, t1, X(PW) + 12, t1), L(X(PW) + 12, (t0 + t1) / 2, X(PW) + 40, (t0 + t1) / 2),
        gpill("TreeView", X(PW) + 40, (t0 + t1) / 2)]
run("02", "tv-variations.png", [proj(wy)], ann)

# 06 indentation: each level is indented 14 px (k_IndentWidth); the foldout arrow sits 2 px in (k_BaseIndent) and is 15 px wide
wy = 162; fold = lambda d: X(2 + 14 * d)
ann = [{"t": "vdash", "x": fold(d), "y1": wy + 46 * 2, "y2": 600, "color": RED, "w": 1} for d in range(5)]
for i, d in ((1, 1), (3, 2), (9, 3), (10, 4)):          # _Recovery, Animations, Bird, Animation
    yy = row_y(i, wy) + LH
    ann += [L(fold(d - 1), yy, fold(d), yy, RED, 2), L(fold(d - 1), yy - 6, fold(d - 1), yy + 6, RED, 2), L(fold(d), yy - 6, fold(d), yy + 6, RED, 2),
            {"t": "text", "s": "14 px", "x": X(0) - 14, "y": yy, "anchor": "rm", "size": 20, "color": RED, "font": FB}]
ann += [gpill("Offset foldout arrow (2 px)", fold(0) - 10, 120, False, RED, [240, 216, 206] if LIGHT else [86, 64, 57]), L(fold(0), 137, fold(0), wy + 46 * 2, RED, 1), pill("Tree view indentation", 40, 540)]
run("06", "tv-indentation.png", [proj(wy)], ann)

# 10 anatomy: 1 tree view, 2 tree view item (row), 3 scrollbar
wy = 82; t0, t1 = row_y(0, wy) - 8, wy + TREE_BOTTOM * 2; r0, r1 = row_y(0, wy), row_y(1, wy); sbx = X(PW - 6)
ann = [L(X(0) - 12, t0, X(0) - 12, t1), L(X(0) - 12, t0, X(0) - 4, t0), L(X(0) - 12, t1, X(0) - 4, t1), L(X(0) - 12, (t0 + t1) / 2, X(0) - 44, (t0 + t1) / 2)] + badge(1, X(0) - 64, (t0 + t1) / 2)
ann += [L(X(PW) + 12, r0, X(PW) + 12, r1), L(X(PW) + 4, r0, X(PW) + 12, r0), L(X(PW) + 4, r1, X(PW) + 12, r1), L(X(PW) + 12, (r0 + r1) / 2, X(PW) + 44, (r0 + r1) / 2)] + badge(2, X(PW) + 64, (r0 + r1) / 2)
sy = wy + 120 * 2
ann += [{"t": "dot", "x": sbx, "y": sy, "r": 5, "color": G}, L(sbx, sy, X(PW) + 44, sy)] + badge(3, X(PW) + 64, sy)
run("10", "tv-anatomy.png", [proj(wy)], ann)

# 05 bold headers: the Assets and Packages roots are drawn bold (captures/tv_bold.png: only Assets expanded, 439pt wide)
wy, wx = 90, 358; bx = lambda pt: wx + pt * 2
ann = []
for i in (0, 10):                                   # Assets, Packages
    y0, y1 = wy + (ROW0 + i * LH) * 2, wy + (ROW0 + (i + 1) * LH) * 2; m = (y0 + y1) / 2
    ann += [L(bx(0) - 12, y0, bx(0) - 12, y1), L(bx(0) - 12, y0, bx(0) - 4, y0), L(bx(0) - 12, y1, bx(0) - 4, y1), L(bx(0) - 12, m, bx(0) - 40, m), gpill("Bold header", bx(0) - 40, m, True)]
run("05", "tv-bold-label.png", [{"src": f"captures/tv_bold{SUF}.png", "chrome": "none", "x": wx, "y": wy, "w": 878}], ann)


# Two-theme images (built on the light run; skipped until both halves exist): light half left, dark half right
if LIGHT:
    DG, DF = [178, 255, 89], [48, 60, 36]          # the dark half keeps the dark callout colours
    import theme as T
    def two(n, out, l, d, ann, h=600, wl=800):
        if not (os.path.exists(l) and os.path.exists(d)): print("half missing:", out); return
        spec = {"out": f"{OUTDIR}/{out}", "canvas": [1600, h], "backdrop": [194, 194, 194],
                "windows": [{"src": l, "chrome": "none", "x": (800 - wl) // 2, "y": 0 if wl == 800 else 40, "w": wl},
                            {"src": d, "chrome": "none", "x": 800 + (800 - wl) // 2, "y": 0 if wl == 800 else 40, "w": wl}], "annotations": ann}
        if wl != 800: spec["annotations"] = [{"t": "rect", "x": 800, "y": 0, "w_": 800, "h": h, "color": [56, 56, 56], "w": 0, "fill": 1.0, "dash": False}] + ann; spec["backdrop"] = [194, 194, 194]
        json.dump(spec, open(f"layouts/tree-view_{n}.json", "w"), indent=1)
        subprocess.run([sys.executable, "scripts/compose.py", f"layouts/tree-view_{n}.json"], check=True, stdout=subprocess.DEVNULL)
    from PIL import Image
    cv = Image.new("RGB", (1600, 600), (194, 194, 194)); cv.paste((56, 56, 56), (800, 0, 1600, 600))
    for src, ox in (("captures/sv_ov_l.png", 0), ("captures/sv_ov.png", 800)):
        im = Image.open(src).convert("RGB"); im = im.resize((600, round(im.height * 600 / im.width)), Image.LANCZOS); cv.paste(im.crop((0, 0, 600, min(560, im.height))), (ox + 100, 40))
    cv.save("captures/tv_overview_both.png")
    spec = {"out": f"{OUTDIR}/tv-overview.png", "canvas": [1600, 600], "backdrop": [194, 194, 194], "windows": [{"src": "captures/tv_overview_both.png", "chrome": "none", "x": 0, "y": 0, "w": 1600}], "annotations": []}
    json.dump(spec, open("layouts/tree-view_01.json", "w"), indent=1); subprocess.run([sys.executable, "scripts/compose.py", "layouts/tree-view_01.json"], check=True, stdout=subprocess.DEVNULL)
    lab = lambda s_, x_, y_, col, a: {"t": "text", "s": s_, "x": x_, "y": y_, "anchor": a, "size": 22, "color": col, "font": F}
    ann = [lab("Expanded", 2 * 166, 60, [110, 110, 110], "mm"), lab("Collapsed", 2 * 316, 60, [110, 110, 110], "mm"),
           lab("Expanded", 800 + 2 * 98, 60, [190, 190, 190], "mm"), lab("Collapsed", 800 + 2 * 249, 60, [190, 190, 190], "mm")]
    for i, s_ in enumerate(("Default", "Hover", "Selected", "Focused", "Disabled")):
        y_ = 2 * [80, 138, 196, 252, 309][i]; ann += [lab(s_, 54, y_, [110, 110, 110], "lm"), lab(s_, 1560, y_, [190, 190, 190], "rm")]
    two("11", "tv-states.png", "captures/tv_states_l.png", "captures/tv_states.png", ann, 740)
    ann = []
    for i, s_ in enumerate(("Default", "Hover", "Selected", "Focused")):
        y_ = 2 * [57, 121, 185, 249][i]; ann += [lab(s_, 56, y_, [110, 110, 110], "lm")]
    if os.path.exists(_EFDS_TMP + "/tv_color.json"):
        r = {k: [v * 2 for v in vv] for k, vv in json.load(open(_EFDS_TMP + "/tv_color.json")).items()}
        tgx, tgy, tgw, tgh = r["r0c0:toggle"]; icx, icy, icw, ich = r["r0c0:icon"]; lbx, lby, lbw, lbh = r["r0c0:label"]
        def bdg(n_, x_, y_): return [{"t": "dot", "x": x_, "y": y_, "r": 20, "color": DG}, {"t": "dot", "x": x_, "y": y_, "r": 18, "color": DF},
                                    {"t": "text", "s": str(n_), "x": x_, "y": y_, "anchor": "mm", "size": 22, "color": DG, "font": F}]
        ln = lambda a, b, c, d: {"t": "line", "x1": a, "y1": b, "x2": c, "y2": d, "color": DG, "w": 2}
        for n_, (x_, y_), by_ in ((1, (lbx + 30, lby), 2 * 57 - 60), (2, (icx + icw / 2, icy + ich / 2), 2 * 57), (3, (tgx + tgw / 2, tgy + tgh), 2 * 57 + 60)):
            ann += [{"t": "dot", "x": 800 + x_, "y": y_, "r": 4, "color": DG}, ln(800 + x_, y_, 800 + 160, by_), ln(800 + 160, by_, 800 + 110, by_)] + bdg(n_, 800 + 90, by_)
    two("12", "tv-color.png", "captures/tv_color_l.png", "captures/tv_color.png", ann)
