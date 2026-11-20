"""Components > Toolbar: compose the dark images at 2x from real Unity 7 captures:
captures/tl_scene.png (floating Scene view 820pt wide, view-option overlays only: unity/tl_overlays.cs; control rects C:/tmp/tl_controls.json),
captures/tl_timeline.png (Timeline with Scene and Game tabs, empty in-memory timeline: unity/tl_timeline.cs), captures/sv_ov.png (Project
toolbar), captures/cp_apptb.png (main toolbar). #4 is the original's constructed 'don't' example: a real UI Toolkit toolbar strip
(captures/tl_strip.png, unity/tl_strip.cs) set into the Timeline track toolbar. #8 = Color palette 'Toolbars' (layouts/color-palette_14.json)."""
import os as _os; _EFDS_TMP = _os.environ.get("EFDS_TMP", "C:/tmp")
import json, os, subprocess, sys
from PIL import Image
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, G, FILL, OUT, BG, INK, themed
F, FB = "fonts/Inter-Regular.ttf", "fonts/Inter-SemiBold.ttf"; RED = [205, 80, 40] if LIGHT else [255, 110, 64]; K = 2.5
DARKPAGE = [194, 194, 194] if LIGHT else [20, 20, 20]
OUTDIR = "replacements/components/toolbar"; os.makedirs(OUTDIR, exist_ok=True)
L = lambda a, b, c, d, col=G, w=2: {"t": "line", "x1": a, "y1": b, "x2": c, "y2": d, "color": col, "w": w}
pill = lambda s, x, y: {"t": "pill", "s": s, "x": x, "y": y, "h": 40, "pad": 14, "size": 22, "bg": BG, "outline": OUT, "color": INK, "font": FB}
def badge(n, cx, cy): return [{"t": "dot", "x": cx, "y": cy, "r": 20, "color": G}, {"t": "dot", "x": cx, "y": cy, "r": 18, "color": FILL},
                              {"t": "text", "s": str(n), "x": cx, "y": cy, "anchor": "mm", "size": 22, "color": G, "font": F}]
def run(n, out, wins, ann, canvas=(1600, 600), backdrop=BG):
    spec = {"out": f"{OUTDIR}/{out}", "canvas": list(canvas), "backdrop": backdrop, "windows": wins, "annotations": ann}
    lay = f"layouts/toolbar_{n}{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
def win(src, x, y, w, crop_pt=None, cap_size=None):   # crop_pt = (x0, y0, x1, y1) in pt of the capture
    d = {"src": src, "chrome": "none", "x": x, "y": y, "w": w}
    if crop_pt: cw, ch = cap_size; d["crop"] = [int(crop_pt[0] * K), int(crop_pt[1] * K), cw - int(crop_pt[2] * K), ch - int(crop_pt[3] * K)]
    return d

# 03 / 04 Timeline: two toolbar rows (rows: 26-48 and 49.2-71.2 pt), zoomed crop of the window's top-left
TL = Image.open(f"captures/tl_timeline{SUF}.png").convert("RGB"); TLS = TL.size
crop = (0, 0, 210, 102); X0, Y0, PX = 88, 143, 692 / 210            # canvas px per pt
ry = lambda pt: Y0 + pt * PX; rx = lambda pt: X0 + pt * PX
def bracket(x, y0, y1, t=14): return [L(x, y0, x, y1), L(x, y0, x + t, y0), L(x, y1, x + t, y1)]
run("03", "toolbar-multiple Y.png", [win(f"captures/tl_timeline{SUF}.png", X0, Y0, 692, crop, TLS)],
    bracket(64, ry(27), ry(47)) + bracket(64, ry(50.2), ry(70.2)), (780, 480), DARKPAGE)
strip = Image.open(f"captures/tl_strip{SUF}.png").convert("RGB").crop((0, int(10 * K), int(168 * K), int(32 * K)))
dont = TL.copy(); dont.paste(strip, (int(60 * K), int(49.2 * K))); dont.save(f"captures/tl_timeline_dont{SUF}.png")
run("04", "toolbar-multiple N.png", [win(f"captures/tl_timeline_dont{SUF}.png", X0, Y0, 692, crop, TLS)],
    [L(rx(60), ry(75), rx(205), ry(75), RED, 3), L(rx(60), ry(72.5), rx(60), ry(75), RED, 3), L(rx(205), ry(72.5), rx(205), ry(75), RED, 3)], (780, 480), DARKPAGE)

# Scene toolbar strip (tab 0-26, toolbar row 26-52 pt); 820pt capture shown at 1360 px wide
SC = Image.open(f"captures/tl_scene{SUF}.png").convert("RGB"); SCS = SC.size; ctl = json.load(open(_EFDS_TMP + "/tl_controls.json"))
def scene(x, y, w, with_tab=True): return win(f"captures/tl_scene{SUF}.png", x, y, w, (0, 0 if with_tab else 26, 820, 51), SCS)

# 05 grouping: related controls underlined (Scene view options, Timeline track toolbar, Project toolbar)
s = 1360 / 820; sx = lambda pt: 120 + pt * s
groups = [(96.8, 336), (352, 521.2), (540.4, 648), (651.2, 817)]
ann = [L(sx(a) + 2, 196, sx(b) - 2, 196) for a, b in groups]
tlr = (0, 49.2, 337, 71.2); t2 = 2.0; tx = lambda pt: 40 + pt * t2
ann += [L(tx(18), 448, tx(56), 448), L(tx(153), 448, tx(264), 448), L(tx(268), 448, tx(300), 448)]
PB = Image.open(f"captures/sv_ov{SUF}.png").convert("RGB"); px_ = lambda pt: 860 + pt * 2.3
ann += [L(px_(6), 448, px_(30), 448), L(px_(36), 448, px_(130), 448), L(px_(134), 448, px_(296), 448)]
run("05", "toolbar-elementGrouping.png",
    [scene(120, 150, 1360, False), win(f"captures/tl_timeline{SUF}.png", 40, 404, 337 * t2, tlr, TLS), win(f"captures/sv_ov{SUF}.png", 860, 394, 300 * 2.3, (0, 26, 300, 46), PB.size)], ann)

# 06 anatomy: 1 the window toolbar, 2 its controls
s = 1360 / 820; ax = lambda pt: 120 + pt * s; ay = lambda pt: 200 + pt * s
ann = [L(ax(0) - 18, ay(27), ax(0) - 18, ay(51)), L(ax(0) - 18, ay(27), ax(0) - 6, ay(27)), L(ax(0) - 18, ay(51), ax(0) - 6, ay(51)), L(ax(0) - 18, ay(39), ax(0) - 50, ay(39))] + badge(1, ax(0) - 70, ay(39))
leaf = [c for c in ctl if not any(o is not c and o[0] >= c[0] and o[0] + o[2] <= c[0] + c[2] + 0.1 and o[2] < c[2] for o in ctl)]
by = ay(66); cx = [ax(c[0] + c[2] / 2) for c in leaf]
ann += [L(cx[0], by, cx[-1], by)] + [x for c in cx for x in (L(c, ay(47), c, by), {"t": "dot", "x": c, "y": ay(47), "r": 4, "color": G})]
mid = (cx[0] + cx[-1]) / 2; ann += [L(mid, by, mid, by + 30)] + badge(2, mid, by + 50)
run("06", "toolbar-anatomy-windowToolbar.png", [scene(120, 200, 1360)], ann)

# 07 main toolbar: traffic falls off from left to right
O = [255, 110, 64]; ann = [pill("Drawing focus Left to Right \u2192", 40, 60)]
x0, x1, ly = 236, 1370, 358
for i in range(60):
    t = i / 60; c = [round(G[j] + (O[j] - G[j]) * t) for j in range(3)]
    ann.append(L(x0 + (x1 - x0) * t, ly, x0 + (x1 - x0) * (t + 1 / 60), ly, c, 3))
for (cx_, col, arrow, txt, anchor, tx_) in ((76, G, "\u2191", "High Traffic\nActions", "lm", 96), (1392, O, "\u2193", "Low Traffic\nActions", "lm", 1428)):
    ann += [{"t": "dot", "x": cx_, "y": ly, "r": 17, "color": col}, {"t": "dot", "x": cx_, "y": ly, "r": 15, "color": BG},
            {"t": "text", "s": arrow, "x": cx_, "y": ly, "anchor": "mm", "size": 22, "color": col, "font": FB},
            {"t": "text", "s": txt, "x": tx_, "y": ly - 12, "anchor": anchor, "size": 22, "color": col, "font": FB}]
run("07", "toolbar-layout.png", [{"src": f"captures/cp_apptb{SUF}.png", "chrome": "none", "x": 40, "y": 262, "w": 1520}], ann)

# 08 = Color palette 'Toolbars' (light: that page's _light layout)
spec = json.load(open(f"layouts/color-palette_14{LAY}.json")); spec["out"] = f"{OUTDIR}/Toolbars.png"
json.dump(spec, open(f"layouts/toolbar_08{LAY}.json", "w"), indent=1); subprocess.run([sys.executable, "scripts/compose.py", f"layouts/toolbar_08{LAY}.json"], check=True, stdout=subprocess.DEVNULL)

# 01 overview (two themes, built on the light run): one Scene toolbar crossing the theme boundary, light left of the canvas middle, dark right
if LIGHT:
    W_, X_, Y_ = 1360, 120, 262
    canvas = Image.new("RGB", (1600, 600), (194, 194, 194)); canvas.paste((56, 56, 56), (800, 0, 1600, 600))
    for src, x0, x1 in (("captures/tl_scene_l.png", 0, 800), ("captures/tl_scene.png", 800, 1600)):
        im = Image.open(src).convert("RGB"); im = im.crop((0, 0, int(820 * K), int(51 * K))); im = im.resize((W_, round(im.height * W_ / im.width)))
        a, b = max(x0 - X_, 0), min(x1 - X_, W_); canvas.paste(im.crop((a, 0, b, im.height)), (X_ + a, Y_))
    canvas.save("captures/tl_overview_both.png")
    spec = {"out": f"{OUTDIR}/toolbar-overview.png", "canvas": [1600, 600], "backdrop": [194, 194, 194],
            "windows": [{"src": "captures/tl_overview_both.png", "chrome": "none", "x": 0, "y": 0, "w": 1600}], "annotations": []}
    json.dump(spec, open("layouts/toolbar_01.json", "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", "layouts/toolbar_01.json"], check=True, stdout=subprocess.DEVNULL)
