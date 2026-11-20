"""Components > Reorderable list: compose the dark images from unity/rl_spec.cs (captures/rl_<which>.png, rects C:/tmp/rl_<which>.json)
at 2x with the original's green bracket callouts / purple spacing callouts.
Unity 7 lists: the +/- footer sits inside the list's frame (no hanging tab); rows are 22pt (UI Toolkit) / 21pt element height (IMGUI).
Callout numbers follow the page text (IMGUI: 1 header, 2 no border, 3 add/remove, 4 reorder)."""
import os as _os; _EFDS_TMP = _os.environ.get("EFDS_TMP", "C:/tmp")
import json, os, subprocess, sys
from PIL import Image
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, G, FILL, OUT, BG, themed
F, FB = "fonts/Inter-Regular.ttf", "fonts/Inter-SemiBold.ttf"
OUTDIR = "replacements/components/reorderable-list"; os.makedirs(OUTDIR, exist_ok=True)
def R(w): return {k: [v * 2 for v in r] for k, r in json.load(open(f"{_EFDS_TMP}/rl_{w}{SUF}.json")).items()}
L = lambda a, b, c, d, col=G, w=2: {"t": "line", "x1": a, "y1": b, "x2": c, "y2": d, "color": col, "w": w}
def badge(n, cx, cy): return [{"t": "dot", "x": cx, "y": cy, "r": 20, "color": G}, {"t": "dot", "x": cx, "y": cy, "r": 18, "color": FILL},
                              {"t": "text", "s": str(n), "x": cx, "y": cy, "anchor": "mm", "size": 22, "color": G, "font": F}]
def bracket(x, y0, y1, side):   # side -1: on the left of the target (opens right); +1: on the right (opens left)
    t = 14 * -side; return [L(x, y0, x, y1), L(x, y0, x + t, y0), L(x, y1, x + t, y1)]
def callout(n, x, y0, y1, side, reach=36):   # bracket + leader + badge
    m = (y0 + y1) / 2; bx = x + side * (reach + 20)
    return bracket(x, y0, y1, side) + [L(x, m, x + side * reach, m)] + badge(n, bx, m)
def run(n, out, which, ann, canvas=(1600, 600), src=None):
    spec = {"out": f"{OUTDIR}/{out}", "canvas": list(canvas), "backdrop": BG,
            "windows": [{"src": src or f"captures/rl_{which}{SUF}.png", "chrome": "none", "x": 0, "y": 0, "w": canvas[0]}], "annotations": ann}
    lay = f"layouts/reorderable-list_{n}{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)

def imgui_parts(r, k):   # header / body / footer y-ranges (2x px) of an IMGUI ReorderableList from its rect + metrics
    x, y, w, _ = r[k]; hh, fh, eh, tot = [v / 2 for v in r[k + ":metrics"]]; s = 2
    return x, w, (y, y + hh * s), (y + hh * s, y + (tot - fh) * s), (y + (tot - fh) * s, y + tot * s)

# 02 IMGUI anatomy: 1 header, 2 list without border/background, 3 add/remove footer, 4 reorderable items
r = R("anatomy"); x, w, hd, body, ft = imgui_parts(r, "bg"); x2, w2, hd2, _, _ = imgui_parts(r, "nobg")
ann = callout(1, x - 20, hd[0] + 2, hd[1] - 2, -1) + callout(4, x - 20, body[0] + 4, body[1] - 4, -1)
ann += callout(3, x + w + 20, ft[0] + 2, ft[1] - 2, 1) + callout(2, x2 - 20, hd2[0] + 2, hd2[1] - 2, -1)
run("02", "087e8d079c3cb6ac6d6969e1a02a32d5_rlv-anatomy.png", "anatomy", ann)

# 03 UI Toolkit anatomy: 1 foldout header, 2 alternating rows, 3 add/remove footer, 4 no border, 5 reorder without handles
r = R("uitk"); tl, tr, bl, br = r["tl"], r["tr"], r["bl"], r["br"]
ann = callout(1, tl[0] - 20, tl[1] + 2, r["tl:scroll"][1] - 4, -1)
ann += callout(2, tr[0] - 20, r["tr:row0"][1], r["tr:row3"][1] + r["tr:row3"][3], -1)
ann += callout(3, tl[0] + tl[2] + 20, r["tl:footer"][1] + 2, r["tl:footer"][1] + r["tl:footer"][3] - 2, 1)
ann += callout(4, bl[0] - 20, r["bl:row0"][1], r["bl:row2"][1] + r["bl:row2"][3], -1)
ann += callout(5, br[0] - 20, r["br:row0"][1], r["br:row2"][1] + r["br:row2"][3], -1)
run("03", "1a01db89a1fcb1bf01bd3f46d03b0b12_rlv-UITK-anatomy.png", "uitk", ann)

# 05 states, dark half: unfocused capture (Selected = grey) with the focused rows pasted in (IMGUI Item 3, UI Toolkit Item 5 = blue)
r = json.load(open(f"{_EFDS_TMP}/rl_states_false{SUF}.json")); k = 2.5
a = Image.open(f"captures/rl_states_false{SUF}.png").convert("RGB"); f = Image.open(f"captures/rl_states_true{SUF}.png").convert("RGB")
x, y, w, h = r["imgui"]; hh, fh, eh, tot = r["imgui:metrics"]
iy = y + hh + (tot - hh - fh - 3 * eh) / 2 + 2 * eh          # top of the 3rd IMGUI element (list padding split top/bottom)
for (bx, by, bw, bh) in [(x + 1, iy, w - 2, eh), tuple(r["uitk:row4"])]:
    box = tuple(int(round(v * k)) for v in (bx, by, bx + bw, by + bh)); a.paste(f.crop(box), box[:2])
a.save(f"captures/rl_states_d{SUF}.png")

# 04 spacing (lists zoomed 1.75x in the specimen): measured Unity 7 values, not the original's.
# IMGUI ReorderableList: 23px per element (elementHeight 21 + elementPadding 2), 3px list padding to the right of the elements.
# UI Toolkit ListView: 1px border, 2px top/bottom padding on the item container (resolvedStyle of the collection's scroll view).
P = [112, 82, 200] if LIGHT else [175, 145, 244]; raw = json.load(open(f"{_EFDS_TMP}/rl_spacing{SUF}.json"))
def T(s, x, y, anchor="lm"): return {"t": "text", "s": s, "x": x, "y": y, "size": 20, "color": P, "anchor": anchor}
def pin(x, y0, y1): return [L(x, y0, x, y1, P, 1.5), {"t": "dot", "x": x, "y": y0, "r": 4, "color": P}]
ix, iy, iw, _ = [v * 2 for v in raw["imgui"]]; z = raw["imgui:scale"][0] * 2          # local pt -> canvas
ann = []
els = [raw[f"imgui:el{i}"] for i in range(4)]; inner_l = ix + z; ex1 = ix + (els[0][0] + els[0][2]) * z; inner_r = ix + iw - z
for e in els:
    ann.append({"t": "rect", "x": inner_l, "y": iy + e[1] * z, "w_": ex1 - inner_l, "h": e[3] * z, "color": P, "w": 1.5})
bx = ix - 40; ann += [L(bx, iy + els[0][1] * z, bx, iy + (els[-1][1] + els[-1][3]) * z, P, 1.5)]
for e in els + [None]:
    yy = iy + (e[1] if e else els[-1][1] + els[-1][3]) * z; ann.append(L(bx, yy, bx + 14, yy, P, 1.5))
ann += [T("23 px", bx, iy + (els[-1][1] + els[-1][3]) * z + 26, "mm")]
ann += [{"t": "rect", "x": ex1, "y": iy + els[0][1] * z, "w_": inner_r - ex1, "h": (els[-1][1] + els[-1][3] - els[0][1]) * z, "color": P, "w": 0, "fill": 0.35, "dash": False}]
ann += pin(ex1 + (inner_r - ex1) / 2, 40, iy + els[0][1] * z) + [T("3 px", ex1 + (inner_r - ex1) / 2 - 10, 40, "rm")]
r = R("spacing"); ux, uy, uw, uh = r["uitk"]; sx, sy, sw, sh = r["uitk:scroll"]; cx, cy, cw, ch = r["uitk:content"]; b = z    # 1px border
ann += [{"t": "rect", "x": sx + b, "y": sy + b, "w_": sw - 2 * b, "h": cy - sy - b, "color": P, "w": 0, "fill": 0.35, "dash": False},
        {"t": "rect", "x": sx + b, "y": cy + ch, "w_": sw - 2 * b, "h": sy + sh - cy - ch, "color": P, "w": 0, "fill": 0.35, "dash": False},
        {"t": "rect", "x": sx + b, "y": sy + b, "w_": sw - 2 * b, "h": sh - b, "color": P, "w": 1.5}]
ann += pin(sx + b / 2, 40, sy) + [T("1 px", sx + b / 2 + 12, 40)] + pin(sx + sw - b / 2, 40, sy) + [T("1 px", sx + sw - b / 2 - 12, 40, "rm")]
mx = sx + 200; my = sy + sh + 42; ann += [L(mx, sy + sh, mx, my - 14, P, 1.5), {"t": "dot", "x": mx, "y": sy + sh, "r": 4, "color": P},
                           T("2 px (top and bottom)", mx, my, "mm")]
run("04", "9bbed00275a5957ac4abccf213e20992_rlv-spacing.png", "spacing", ann)


# Two-theme images (built on the light run, from both skins' captures): light half left, dark half right, like the originals
if LIGHT:
    import theme as T
    DG, DF = [178, 255, 89], [48, 60, 36]
    def two(n, out, l, d, canvas, ann):
        spec = {"out": f"{OUTDIR}/{out}", "canvas": list(canvas), "backdrop": [194, 194, 194],
                "windows": [{"src": l, "chrome": "none", "x": 0, "y": 0, "w": canvas[0] // 2}, {"src": d, "chrome": "none", "x": canvas[0] // 2, "y": 0, "w": canvas[0] // 2}],
                "annotations": ann}
        json.dump(spec, open(f"layouts/reorderable-list_{n}.json", "w"), indent=1)
        subprocess.run([sys.executable, "scripts/compose.py", f"layouts/reorderable-list_{n}.json"], check=True, stdout=subprocess.DEVNULL)
    two("01", "373013f16ccfd7aacd96f68873709729_rlv-overview.png", "captures/rl_overview_l.png", "captures/rl_overview.png", (1600, 600), [])
    # states: labels left of the light lists, right of the dark lists; IMGUI / UITK pills; divider between the two list kinds
    r = {k: [v * 2 for v in vv] for k, vv in json.load(open(_EFDS_TMP + "/rl_states_false_l.json")).items()}
    x, y, w, h = r["imgui"]; hh, fh, eh, tot = r["imgui:metrics"]
    rows_i = [y + hh + (tot - hh - fh - 3 * eh) / 2 + i * eh + eh / 2 for i in range(3)]
    rows_u = [r[f"uitk:row{i}"][1] + r[f"uitk:row{i}"][3] / 2 for i in range(5)]
    lab = lambda s_, x_, y_, col, anchor: {"t": "text", "s": s_, "x": x_, "y": y_, "anchor": anchor, "size": 26, "color": col, "font": F}
    gp = lambda s_, x_, y_, g, f: {"t": "pill", "s": s_, "x": x_, "y": y_, "h": 40, "pad": 14, "size": 22, "bg": f, "outline": g, "color": g, "font": F}
    ann = [{"t": "line", "x1": 0, "y1": 355, "x2": 1600, "y2": 355, "color": [170, 170, 170], "w": 1}, {"t": "line", "x1": 800, "y1": 355, "x2": 1600, "y2": 355, "color": [86, 87, 88], "w": 1},
           gp("IMGUI", 37, 40, T.G, T.FILL), gp("IMGUI", 837, 40, DG, DF), gp("UITK", 37, 400, T.G, T.FILL), gp("UITK", 837, 400, DG, DF)]
    for names, ys in ((("Default", "Selected", "Focused"), rows_i), (("Default", "Alternate", "Selected", "Hover", "Focused"), rows_u)):
        for s_, yy in zip(names, ys): ann += [lab(s_, x - 20, yy, [120, 120, 120], "rm"), lab(s_, 800 + x + w + 20, yy, [200, 200, 200], "lm")]
    two("05", "c02440186b4ffe0f0772bcd67b83b103_rlv-states.png", "captures/rl_states_d_l.png", "captures/rl_states_d.png", (1600, 740), ann)
