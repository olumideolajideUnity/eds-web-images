"""Components > Scroll view: compose the dark images from unity/sv_spec.cs (captures/sv_*.png, rects C:/tmp/sv_*.json) at 2x.
Unity 7's Project and Hierarchy trees never scroll horizontally (TreeViewGUI m_UseHorizontalScroll = false) and IMGUI hover needs the
real pointer, so the scroll-view figures use a real UI Toolkit TreeView of the project's Atrio-Packages folder (horizontalScrollingEnabled).
Viewport / container figures: the scrolled tree over a faded full-size render of the same tree (the content container), as in the original."""
import os as _os; _EFDS_TMP = _os.environ.get("EFDS_TMP", "C:/tmp")
import json, os, subprocess, sys
from PIL import Image, ImageFont
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, G, FILL, OUT, BG, themed
F, FB = "fonts/Inter-Regular.ttf", "fonts/Inter-SemiBold.ttf"; K = 2.5
OUTDIR = "replacements/components/scroll-view"; os.makedirs(OUTDIR, exist_ok=True)
def J(n): return json.load(open(f"{_EFDS_TMP}/{n}{SUF}.json"))
def R(n): return {k: [v * 2 for v in r] for k, r in J(n).items()}
L = lambda a, b, c, d, col=G, w=2: {"t": "line", "x1": a, "y1": b, "x2": c, "y2": d, "color": col, "w": w}
PF = ImageFont.truetype(F, 22)
def gpill(s, x, y, right=False):   # green pill; right=True: x is the pill's right edge
    w = PF.getlength(s) + 28
    return {"t": "pill", "s": s, "x": x - w if right else x, "y": y, "h": 40, "pad": 14, "size": 22, "bg": FILL, "outline": G, "color": G, "font": F}, (x - w if right else x + w)
def bracket(x, y0, y1, side):
    t = 14 * -side; return [L(x, y0, x, y1), L(x, y0, x + t, y0), L(x, y1, x + t, y1)]
def run(n, out, src, ann):
    spec = {"out": f"{OUTDIR}/{out}", "canvas": [1600, 600], "backdrop": BG,
            "windows": [{"src": src, "chrome": "none", "x": 0, "y": 0, "w": 1600}], "annotations": ann}
    lay = f"layouts/scroll-view_{n}{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
def box(r): return tuple(int(round(v * K)) for v in (r[0], r[1], r[0] + r[2], r[1] + r[3]))
def fade(im, keep=0.35):
    bg = Image.new("RGB", im.size, tuple(BG)); return Image.blend(bg, im, keep)

# 02 vertical-only and horizontal-only ScrollViews, divided like the original
run("02", "sv-variations.png", f"captures/sv_variations{SUF}.png", [L(800, 0, 800, 600, OUT, 1)])

# viewport over the faded content container
small, full = J("sv_tree_false"), J("sv_tree_true")
a = Image.open(f"captures/sv_tree_false{SUF}.png").convert("RGB"); f = Image.open(f"captures/sv_tree_true{SUF}.png").convert("RGB")
base = Image.new("RGB", a.size, tuple(BG)); cb = box(small["tree:content"])
base.paste(fade(f.crop(cb)), cb[:2]); tb = box(small["tree"]); base.paste(a.crop(tb), tb[:2]); base.save(f"captures/sv_cv{SUF}.png")
r = R("sv_tree_false"); tx, ty, tw, th = r["tree"]; cx, cy, cw, ch = r["tree:content"]
cont = [{"t": "rect", "x": cx - 2, "y": cy - 2, "w_": cw + 2, "h": ch + 2, "color": [140, 140, 140] if LIGHT else [26, 26, 26], "w": 1, "dash": False, "solid": True}]
vp_y = ty + th / 2; p, _ = gpill("Content viewport", tx - 50, vp_y, right=True)
viewport = bracket(tx - 20, ty, ty + th, -1) + [L(tx - 20, vp_y, tx - 50, vp_y), p]
cr = cx + cw + 20; cm = cy + ch / 2; p2, _ = gpill("Content container", cr + 36, cm)
run("03", "sv-c-and-v.png", f"captures/sv_cv{SUF}.png", cont + viewport + bracket(cr, cy, cy + ch, 1) + [L(cr, cm, cr + 36, cm), p2])

# 05 anatomy: viewport, container, scrollbar button / thumb / groove
ux, uy, uw, uh = r["tree:vup"]; mx, my, mw, mh = r["tree:vthumb"]; gx, gy, gw, gh = r["tree:vgroove"]
px = cx + cw + 60; ann = cont + viewport
for label, (x, y) in (("Scrollbar button", (ux + uw / 2, uy + uh / 2)), ("Scrollbar thumb", (mx + mw / 2, my + mh / 2)), ("Scrollbar groove", (gx + gw / 2, my + mh + (gy + gh - my - mh) / 2))):
    pp, _ = gpill(label, px, y); ann += [{"t": "dot", "x": x, "y": y, "r": 5, "color": G}, L(x, y, px, y), pp]
ly = cy + ch - 40; p3, _ = gpill("Content container", tx - 50, ly, right=True); ann += [L(tx - 50, ly, cx - 2, ly), p3]
run("05", "sv-anatomy.png", f"captures/sv_cv{SUF}.png", ann)

# 04 tree view with both scrollbars
run("04", "sv-h-and-v.png", f"captures/sv_big_tree{SUF}.png", [])

# 06 colour: everything faded except the hovered scroller down button (real :hover state)
h = J("sv_big_hover"); im = Image.open(f"captures/sv_big_hover{SUF}.png").convert("RGB"); out = fade(im, 0.55); db = box(h["tree:vdown"])
out.paste(im.crop(db), db[:2]); out.save(f"captures/sv_hover_d{SUF}.png")
run("06", "sv-color.png", f"captures/sv_hover_d{SUF}.png", [])


# 01 overview (two themes, built on the light run): one-column Project window in each skin, faded like the original
if LIGHT:
    def faded(src, bg, keep=0.5):
        im = Image.open(src).convert("RGB"); out = Image.blend(Image.new("RGB", im.size, bg), im, keep); out.save(src.replace(".png", "_fade.png")); return src.replace(".png", "_fade.png")
    spec = {"out": f"{OUTDIR}/sv-overview.png", "canvas": [1600, 600], "backdrop": [194, 194, 194],
            "windows": [{"src": faded("captures/sv_ov_l.png", (194, 194, 194)), "chrome": "none", "x": 100, "y": 40, "w": 600},
                        {"src": faded("captures/sv_ov.png", (56, 56, 56)), "chrome": "none", "x": 900, "y": 40, "w": 600}],
            "annotations": [{"t": "rect", "x": 800, "y": 0, "w_": 800, "h": 40, "color": [56, 56, 56], "w": 0, "fill": 1.0, "dash": False},
                            {"t": "rect", "x": 800, "y": 0, "w_": 100, "h": 600, "color": [56, 56, 56], "w": 0, "fill": 1.0, "dash": False},
                            {"t": "rect", "x": 1500, "y": 0, "w_": 100, "h": 600, "color": [56, 56, 56], "w": 0, "fill": 1.0, "dash": False}]}
    json.dump(spec, open("layouts/scroll-view_01.json", "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", "layouts/scroll-view_01.json"], check=True, stdout=subprocess.DEVNULL)
