"""Components > Dropdown: compose the dark images from real pieces:
- UI Toolkit DropdownFields (unity/dd_spec.cs -> captures/dd_<which>.png, rects C:/tmp/dd_<which>.json);
- Unity 7 owner-drawn OS menus (scripts/pages/dd_menus.py -> captures/menus/dd_*.png, following MenuThemeWin.cpp);
- real Editor popups: the Gizmos window (AnnotationWindow) and the Scene Camera window (unity/dd_popups.cs).
At 2x with the original's callouts and pills."""
import os as _os; _EFDS_TMP = _os.environ.get("EFDS_TMP", "C:/tmp")
import json, os, subprocess, sys
from PIL import ImageFont
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, G, FILL, OUT as TOUT, BG as TBG, INK, TXT, PAGE as TPAGE, themed
F, FB = "fonts/Inter-Regular.ttf", "fonts/Inter-SemiBold.ttf"; WHITE, OUT, BG = INK, TOUT, TBG
OUTDIR = "replacements/components/dropdown"; os.makedirs(OUTDIR, exist_ok=True)
M = json.load(open(f"captures/menus/dd_menus{SUF}.json"))
def R(w): return {k: [v * 2 for v in r] for k, r in json.load(open(f"{_EFDS_TMP}/dd_{w}{SUF}.json")).items()}
def menu(name, x, y):            # place a menu image so its frame's top-left is at (x, y) canvas px
    m = M[name]; return {"t": "image", "src": f"captures/menus/dd_{name}{SUF}.png", "x": x - m["pad"], "y": y - m["pad"], "w": m["w"]}
L = lambda a, b, c, d, col=G, w=2: {"t": "line", "x1": a, "y1": b, "x2": c, "y2": d, "color": col, "w": w}
def badge(n, cx, cy): return [{"t": "dot", "x": cx, "y": cy, "r": 20, "color": G}, {"t": "dot", "x": cx, "y": cy, "r": 18, "color": FILL},
                              {"t": "text", "s": str(n), "x": cx, "y": cy, "anchor": "mm", "size": 22, "color": G, "font": F}]
def pill(s, x, y): return {"t": "pill", "s": s, "x": x, "y": y, "h": 44, "pad": 16, "size": 22, "bg": BG, "outline": OUT, "color": WHITE, "font": FB}
def run(n, out, canvas, wins, ann):
    spec = {"out": f"{OUTDIR}/{out}", "canvas": canvas, "backdrop": BG, "windows": wins, "annotations": ann}
    lay = f"layouts/dropdown_{n}{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
full = lambda src, w=1600: {"src": src, "chrome": "none", "x": 0, "y": 0, "w": w}

# anatomy: zoomed field with its menu below (1.8x); 1 field, 2 label/value, 3 menu
x, y, w, h = R("anatomy")["field"]; my = y + h + 4; mh = M["anatomy"]["h"] - 2 * M["anatomy"]["pad"]
ann = [menu("anatomy", x, my)]
ann += [L(x + w / 2, y - 6, x + w / 2, 126), {"t": "dot", "x": x + w / 2, "y": y - 6, "r": 5, "color": G}] + badge(1, x + w / 2, 106)
ann += [L(x + 14, y + h / 2, 450, y + h / 2), L(450, y + h / 2, 450, 310), {"t": "dot", "x": x + 14, "y": y + h / 2, "r": 5, "color": G}] + badge(2, 450, 330)
mw = M["anatomy"]["w"] - 2 * M["anatomy"]["pad"]
ann += [L(x + mw + 14, my, x + mw + 14, my + mh), L(x + mw + 14, my + mh / 2, 1162, my + mh / 2)] + badge(3, 1182, my + mh / 2)
run("05", "dd-anatomy.png", [1600, 600], [full(f"captures/dd_anatomy{SUF}.png")], ann)

# colour: 1 arrow, 2 background, 3 text
x, y, w, h = R("themes")["field"]; by = y + h + 54
ann = [L(x + 28, y + h - 10, x + 300, y + h - 10), L(x + 150, y + h - 10, x + 150, by - 20), {"t": "dot", "x": x + 150, "y": y + h - 10, "r": 5, "color": G}] + badge(3, x + 150, by)
ann += [L(x + w - 110, y + h - 4, x + w - 110, by - 20), {"t": "dot", "x": x + w - 110, "y": y + h - 4, "r": 5, "color": G}] + badge(2, x + w - 110, by)
ann += [L(x + w - 34, y + h / 2, x + w - 34, by - 20), {"t": "dot", "x": x + w - 34, "y": y + h / 2, "r": 5, "color": G}] + badge(1, x + w - 34, by)
run("08", "dd-themes.png", [1600, 600], [full(f"captures/dd_themes{SUF}.png")], ann)

# menu positioning (1.5x): closed; menu over the field with the checked item aligned to it; menu below the field
r = R("position"); k = M["position"]["k"]; ann = [pill("Menu Positioning", 40, 62)]
fx, fy, fw, fh = r["mid"]
for key in ("left", "mid", "right"):
    _, yy, _, hh = r[key]
ann += [{"t": "hdash", "x1": 0, "x2": 1600, "y": fy, "color": OUT, "w": 1}, {"t": "hdash", "x1": 0, "x2": 1600, "y": fy + fh, "color": OUT, "w": 1}]
row_cy = (3 + 22 * 1.5) * k                                 # centre of row 2 (Directional) below the menu frame's top, canvas px
ann += [menu("position", fx - 22 * k + 3 * k, fy + fh / 2 - row_cy)]
rx, ry, rw, rh = r["right"]; ann += [menu("position", rx, ry + rh + 2)]
run("04", "dd-menu position.png", [1600, 622], [full(f"captures/dd_position{SUF}.png")], ann)

# variations: OS menu (owner-drawn) vs custom menu (the real Gizmos window) under the focused fields
r = R("variations"); ann = [pill("OS Menu", 40, 82), pill("Custom Menu", 840, 82)]
ox, oy, ow, oh = r["os2"]; ann += [menu("os", ox, oy + oh + 2)]
cx, cy, cw, ch = r["cu2"]
wins = [full(f"captures/dd_variations{SUF}.png"), {"src": f"captures/dd_gizmos{SUF}.png", "chrome": "none", "x": cx, "y": cy + ch + 2, "w": 540}]
run("02", "dd-variations.png", [1600, 600], wins, ann)

# layout: OS menu, Gizmos menu, Scene Camera menu side by side (1x, as the original)
wins = [{"src": f"captures/dd_variations{SUF}.png", "chrome": "none", "x": 0, "y": 0, "w": 1600, "crop": [0, 0, 0, 0]},
        {"src": f"captures/dd_gizmos{SUF}.png", "chrome": "none", "x": 410, "y": 124, "w": 540},
        {"src": f"captures/dd_camera{SUF}.png", "chrome": "none", "x": 990, "y": 124, "w": 580}]
ann = [pill("OS Menu", 30, 62), pill("Gizmos Menu", 410, 62), pill("Scene Camera Menu", 990, 62), menu("os", 30, 124)]
run("06", "dd-layout.png", [1600, 600], wins[1:], ann)

# menus-dd: Hierarchy's Create (+) menu open on Visual Effects, submenu with Particle System Force Field hovered,
# over a floating Hierarchy and Scene view (SampleScene). Menus at the windows' scale: 2 canvas px per DIP (1x canvas = 2x the UI).
from native_menu import draw
K = 2.0
MENU = dict(bg=(235, 235, 235), text=(9, 9, 9), highlight=(214, 214, 214)) if LIGHT else {}
main, pad = draw("captures/menus/hier_create.txt", K * 2, hot="Visual Effects", hot_outline=True, **MENU)   # drawn at 2x for a crisp @2x output
sub, spad = draw("captures/menus/hier_vfx.txt", K * 2, hot="Particle System Force Field", **MENU)
main.save(f"captures/menus/hier_create{SUF}.png"); sub.save(f"captures/menus/hier_vfx{SUF}.png")
mx, my = 6, 40 * K + 4                                     # under the Hierarchy's + button
row_y = (main.info["rows"]["Visual Effects"] - pad) / 2
sx = mx + (main.width - 2 * pad) / 2 - 3 * K; sy = my + row_y - sub.info["frame"] / 2
ann = [{"t": "image", "src": f"captures/menus/hier_create{SUF}.png", "x": mx - pad / 2, "y": my - pad / 2, "w": main.width / 2},
       {"t": "image", "src": f"captures/menus/hier_vfx{SUF}.png", "x": sx - spad / 2, "y": sy - spad / 2, "w": sub.width / 2}]
wins = [{"src": f"captures/dd_hier{SUF}.png", "chrome": "none", "x": 0, "y": 0, "w": 312},
        {"src": f"captures/dd_scene{SUF}.png", "chrome": "none", "x": 312, "y": 0, "w": 1288}]
run("03", "menus-dd.png", [1600, 600], wins, ann)
