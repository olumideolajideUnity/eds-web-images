"""Components > Toggle: compose the dark images from unity/tg_spec.cs (captures/tg_<which>.png, rects C:/tmp/tg_<which>.json) at 2x,
plus the Physics Layer Collision Matrix (Project Settings > Physics > Settings, captures/tg_phys.png)."""
import os as _os; _EFDS_TMP = _os.environ.get("EFDS_TMP", "C:/tmp")
import json, os, subprocess, sys
from PIL import ImageFont
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, G, FILL, OUT, BG, INK, PAGE as TPAGE, themed
F, FB = "fonts/Inter-Regular.ttf", "fonts/Inter-SemiBold.ttf"; K = 2.5
OUTDIR = "replacements/components/toggle"; os.makedirs(OUTDIR, exist_ok=True)
def R(w): return {k: [v * 2 for v in r] for k, r in json.load(open(f"{_EFDS_TMP}/tg_{w}{SUF}.json")).items()}
L = lambda a, b, c, d, col=G, w=2: {"t": "line", "x1": a, "y1": b, "x2": c, "y2": d, "color": col, "w": w}
pill = lambda s, x, y: {"t": "pill", "s": s, "x": x, "y": y, "h": 40, "pad": 14, "size": 22, "bg": BG, "outline": OUT, "color": INK, "font": FB}
def badge(n, cx, cy): return [{"t": "dot", "x": cx, "y": cy, "r": 20, "color": G}, {"t": "dot", "x": cx, "y": cy, "r": 18, "color": FILL},
                              {"t": "text", "s": str(n), "x": cx, "y": cy, "anchor": "mm", "size": 22, "color": G, "font": F}]
num = lambda s, x, y: [{"t": "dot", "x": x, "y": y, "r": 17, "color": OUT}, {"t": "dot", "x": x, "y": y, "r": 15, "color": BG},
                       {"t": "text", "s": s, "x": x, "y": y, "anchor": "mm", "size": 20, "color": INK, "font": FB}]
fill = lambda x, y, w, h, col: {"t": "rect", "x": x, "y": y, "w_": w, "h": h, "color": col, "w": 0, "fill": 1.0, "dash": False}
def run(n, out, wins, ann, canvas=(1600, 600), backdrop=BG):
    spec = {"out": f"{OUTDIR}/{out}", "canvas": list(canvas), "backdrop": backdrop, "windows": wins, "annotations": ann}
    lay = f"layouts/toggle_{n}{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
full = lambda which, w=1600: {"src": f"captures/tg_{which}{SUF}.png", "chrome": "none", "x": 0, "y": 0, "w": w}

# 02 variations: 1 toggle (checkbox), 2 toolbar toggle
run("02", "toggle-variations.png", [full("variations")], [L(800, 0, 800, 600, OUT, 1)] + num("1", 46, 534) + num("2", 846, 534))
run("03", "toggle.png", [full("toggle", 840)], [], (840, 200))
run("04", "toolbar-toggle.png", [full("tbtoggle", 840)], [], (840, 200))

# 05 Layer Collision Matrix (Project Settings > Physics > Settings, Shared tab), cropped to the panel
run("05", "toggles-wtu.png", [{"src": f"captures/tg_phys{SUF}.png", "chrome": "none", "x": 335, "y": 8, "w": 930,
                               "crop": [int(205 * K), int(88 * K), 1750 - int(692 * K), 2250 - int(395 * K)]}], [])

# 07 toggle vs toggle button: two panels on a darker page, with the original's pills
PAGE = TPAGE if LIGHT else [40, 40, 40]   # panels (px): 120..760 and 840..1482, y 82..520
ann = [fill(0, 0, 1600, 82, PAGE), fill(0, 520, 1600, 80, PAGE), fill(0, 82, 120, 438, PAGE), fill(760, 82, 80, 438, PAGE), fill(1482, 82, 118, 438, PAGE)]
ann += [pill("Toggle", 176, 144), pill("Toggle Button (Independent)", 898, 144), pill("Toggle Button in toolbar", 898, 340)]
run("07", "toggle-vs-togglebutton.png", [full("vs")], ann)

# 08 / 09 labelling do / don't
run("08", "toggle-htu Y img.png", [full("allowhdr", 780)], [], (780, 480))
run("09", "toggle-htu N img.png", [full("hdr", 780)], [], (780, 480))

# 11 checked / mixed / unchecked in three columns
run("11", "toggle-style.png", [full("style")], [L(534, 0, 534, 600, OUT, 1), L(1068, 0, 1068, 600, OUT, 1)])

# 13 parent and child toggles, in a panel like the original
ann = [fill(0, 0, 1600, 120, TPAGE if LIGHT else [40, 40, 40]), fill(0, 492, 1600, 108, TPAGE if LIGHT else [40, 40, 40]), fill(0, 120, 384, 372, TPAGE if LIGHT else [40, 40, 40]), fill(1222, 120, 378, 372, TPAGE if LIGHT else [40, 40, 40])]
run("13", "toggle-behavior.png", [full("behavior")], ann)

# 14 anatomy: 1 label, 2 box, 3 checkmark
r = R("anatomy"); lx, ly, lw, lh = r["t:label"]; ix, iy, iw, ih = r["t:input"]; cx, cy, cw, ch = r["t:check"]; by = 430
ann = [L(lx, ly + lh + 4, lx + lw * 0.95, ly + lh + 4), L(lx + lw * 0.45, ly + lh + 4, lx + lw * 0.45, by - 20)] + badge(1, lx + lw * 0.45, by)
ann += [{"t": "rect", "x": ix - 6, "y": iy - 6, "w_": iw + 12, "h": ih + 12, "color": G, "w": 2, "dash": False, "solid": True, "radius": 8},
        L(ix + iw * 0.3, iy + ih + 6, ix + iw * 0.3, by - 20)] + badge(2, ix + iw * 0.3, by)
ann += [{"t": "dot", "x": cx + cw * 0.7, "y": cy + ch * 0.35, "r": 5, "color": G}, L(cx + cw * 0.7, cy + ch * 0.35, ix + iw + 40, cy + ch * 0.35),
        L(ix + iw + 40, cy + ch * 0.35, ix + iw + 40, by - 20)] + badge(3, ix + iw + 40, by)
run("14", "toggle-anatomy.png", [full("anatomy")], ann)

# 12 toggles in Preferences (General: three checkbox rows; cropped above the Compilation Cache Location row, which shows a local path)
#    and in the Inspector (component enable toggles in the headers; EventSystem from the Atrio scene)
run("12", "toggle-style-labels.png",
    [{"src": f"captures/tg_prefs{SUF}.png", "chrome": "none", "x": 70, "y": 210, "w": 570, "crop": [int(205 * K), int(88 * K), 1550 - int(490 * K), 1050 - int(148 * K)]},
     {"src": f"captures/tg_insp{SUF}.png", "chrome": "none", "x": 840, "y": 100, "w": 704, "crop": [0, 0, 0, 815 - int(188 * K)]}], [])


# Two-theme images (built on the light run; skipped until the dark half exists): light half left, dark half right
if LIGHT:
    DG, DF = [178, 255, 89], [48, 60, 36]          # the dark half keeps the dark callout colours
    import theme as T
    def two(n, out, which, ann, h=600):
        if not os.path.exists(f"captures/tg_{which}.png"): print("dark half missing:", which); return
        spec = {"out": f"{OUTDIR}/{out}", "canvas": [1600, h], "backdrop": [194, 194, 194],
                "windows": [{"src": f"captures/tg_{which}_l.png", "chrome": "none", "x": 0, "y": 0, "w": 800}, {"src": f"captures/tg_{which}.png", "chrome": "none", "x": 800, "y": 0, "w": 800}], "annotations": ann}
        json.dump(spec, open(f"layouts/toggle_{n}.json", "w"), indent=1)
        subprocess.run([sys.executable, "scripts/compose.py", f"layouts/toggle_{n}.json"], check=True, stdout=subprocess.DEVNULL)
    two("01", "toggle-overview.png", "overview", [])
    lab = lambda s_, x_, y_, col, a: {"t": "text", "s": s_, "x": x_, "y": y_, "anchor": a, "size": 22, "color": col, "font": F}
    ann = []
    for i, s_ in enumerate(("Unchecked", "Checked", "Indeterminate")):
        ann += [lab(s_, [300, 482, 660][i], 64, [110, 110, 110], "mm"), lab(s_, 800 + [126, 306, 488][i], 64, [190, 190, 190], "mm")]
    for i, s_ in enumerate(("Default", "Hover", "Focused", "Disabled")):
        y_ = [204, 384, 566, 748][i]; ann += [lab(s_, 64, y_, [110, 110, 110], "lm"), lab(s_, 1560, y_, [190, 190, 190], "rm")]
    two("15", "toggle-states.png", "states", ann, 848)
    r = {k: [v * 2 for v in vv] for k, vv in json.load(open(_EFDS_TMP + "/tg_themes_l.json")).items()}
    ix, iy, iw, ih = r["t:input"]; cx, cy, cw, ch = r["t:check"]; by = iy + ih + 70
    def bdg(n_, x_, y_, g, f): return [{"t": "dot", "x": x_, "y": y_, "r": 20, "color": g}, {"t": "dot", "x": x_, "y": y_, "r": 18, "color": f},
                                      {"t": "text", "s": str(n_), "x": x_, "y": y_, "anchor": "mm", "size": 22, "color": g, "font": F}]
    ln = lambda a, b, c, d, g: {"t": "line", "x1": a, "y1": b, "x2": c, "y2": d, "color": g, "w": 2}
    ann = [{"t": "rect", "x": ix - 6, "y": iy - 6, "w_": iw + 12, "h": ih + 12, "color": T.G, "w": 2, "dash": False, "solid": True, "radius": 8},
           ln(ix + iw * 0.2, iy + ih + 6, ix + iw * 0.2, by - 20, T.G)] + bdg(1, ix + iw * 0.2, by, T.G, T.FILL)
    ann += [{"t": "dot", "x": cx + cw * 0.7, "y": cy + ch * 0.35, "r": 5, "color": T.G}, ln(cx + cw * 0.7, cy + ch * 0.35, cx + cw * 0.7, by - 20, T.G)] + bdg(2, cx + cw * 0.7, by, T.G, T.FILL)
    ann += [{"t": "dot", "x": 800 + ix + iw * 0.5, "y": iy + ih / 2, "r": 5, "color": DG}, ln(800 + ix + iw * 0.5, iy + ih / 2, 800 + ix + iw + 40, iy + ih / 2, DG),
            ln(800 + ix + iw + 40, iy + ih / 2, 800 + ix + iw + 40, by - 20, DG)] + bdg(3, 800 + ix + iw + 40, by, DG, DF)
    two("16", "toggle-themes.png", "themes", ann)
