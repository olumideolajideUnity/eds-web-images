"""Home hero (new banner compressed.png): collage of real Unity 7 pieces in the original's arrangement at 2.2px/pt."""
import json, os, subprocess, sys
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
from PIL import Image, ImageDraw, ImageFont
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from native_menu import draw
K, P, X0 = 2.2, 2.5, 122
LIGHT = os.environ.get("THEME") == "light"          # THEME=light: light Editor skin captures (<name>_l.png), light menu / chip / backdrop
SUF = "_l" if LIGHT else ""
cap = lambda n: f"captures/{n}{SUF}.png"
BACK, APPBAR = ((226, 226, 226), (235, 235, 235)) if LIGHT else ((25, 26, 27), (20, 20, 20))   # app toolbar = --unity-colors-app_toolbar-background
W, H = 2120, 1280
img = Image.new("RGB", (W, H), BACK); d = ImageDraw.Draw(img)
def piece(src, box_pt=None):
    im = Image.open(src).convert("RGB")
    if box_pt: im = im.crop(tuple(round(v * P) for v in box_pt))
    return im.resize((round(im.width / P * K), round(im.height / P * K)), Image.LANCZOS)
def shadow(x, y, w, h):
    from PIL import ImageFilter
    s = Image.new("L", (W, H), 0); ImageDraw.Draw(s).rectangle([x + 6, y + 10, x + w + 6, y + h + 10], fill=170)
    img.paste((0, 0, 0), (0, 0), s.filter(ImageFilter.GaussianBlur(16)))
FW = 908 * K
d.rectangle([X0, 0, X0 + FW, 36 * K], fill=APPBAR)
tb = cap("shn_toolbar")
img.paste(piece(tb, (0, 0, 330, 36)), (X0, 0))
img.paste(piece(tb, (720, 0, 816, 36)), (round(X0 + (454 - 48) * K), 0))
img.paste(piece(tb, (1536 - 300, 0, 1536, 36)), (round(X0 + (908 - 300) * K), 0))
sc = piece(cap("home_scene")); img.paste(sc, (X0, round(36 * K)))
ins = piece(cap("home_insp")); img.paste(ins, (X0 + sc.width, round(36 * K)))
menu, pad = draw("captures/pane_SceneView_float.txt", K, **({"bg": (235, 235, 235), "text": (9, 9, 9)} if LIGHT else {}))   # light: app_toolbar-background #EBEBEB / default-text #090909, read live
mx = X0 + sc.width - (menu.width - 2 * pad) - round(4 * K); my = round((36 + 24) * K)
img.paste(menu.convert("RGB"), (mx - pad, my - pad), menu)
hi = piece(cap("home_hier")); shadow(0, 290, hi.width, hi.height); img.paste(hi, (0, 290))
# Project window: two-column Project browser with its folder pane at the full 273pt width (captures/hero_proj.png, built from
# unity/hero_proj.cs: real tab/toolbar of a 273pt window over the folder pane of a wider one), cropped to 242pt so the level-4
# leaf row shows, placed like the original. Indent values read live from the folder tree (GetContentIndent): level-0 content
# starts 20pt from the pane edge (2pt base indent + 18pt foldout), each level adds indentWidth = 14pt.
pr = piece(cap("hero_proj")); px, py = 138, 690; shadow(px, py, pr.width, pr.height); img.paste(pr, (px, py))
OR = (255, 110, 64); f = ImageFont.truetype(_efds_font("segoeuib.ttf"), 26)
BASE, STEP, TREE_TOP = 20, 14, 49.2
X = lambda pt: px + pt * K; Y = lambda pt: py + pt * K
bottom = py + pr.height
ov = Image.new("RGBA", img.size, (0, 0, 0, 0)); od = ImageDraw.Draw(ov)
od.rectangle([X(0), Y(TREE_TOP), X(BASE), bottom], fill=OR + (66,))          # base strip: pane edge -> level-0 content
img.paste(ov, (0, 0), ov); d = ImageDraw.Draw(img)
def vdash(x, y0, y1, on=7, off=6):
    y = y0
    while y < y1: d.line([x, y, x, min(y + on, y1)], fill=OR, width=2); y += on + off
for lv in range(5): vdash(X(BASE + STEP * lv), Y(TREE_TOP), bottom)      # one dashed guide per indent level (0-4)
for row_pt, depth in ((105.2, 1), (168.0, 3), (232.0, 4)):             # leaf rows (no foldout arrow): _Recovery, Animation, Icons
    y = Y(row_pt); a, b = X(BASE + STEP * (depth - 1)), X(BASE + STEP * depth)
    d.line([a, y, b, y], fill=OR, width=3); d.line([a, y - 9, a, y + 9], fill=OR, width=3); d.line([b, y - 9, b, y + 9], fill=OR, width=3)
    d.text((a - 8, y), "14 px", font=f, fill=OR, anchor="rm")
by = bottom + 16; a, b = X(0) + 2, X(BASE)                             # bracket under the window for the base strip
d.line([a, bottom, a, by], fill=OR, width=3); d.line([b, bottom, b, by], fill=OR, width=3); d.line([a, by, b, by], fill=OR, width=3)
d.line([(a + b) / 2, by, (a + b) / 2, by + 10], fill=OR, width=3)
d.text(((a + b) / 2, by + 26), "20px", font=f, fill=OR, anchor="mm")
cx, cy = 1250, 600; cf = ImageFont.truetype(_efds_font("consolab.ttf"), 24)
CHIP, CHIPO, CHIPT, FOC = ((246, 246, 246), (200, 200, 200), (20, 20, 20), (0x0E, 0x54, 0x8A)) if LIGHT else ((32, 32, 32), (64, 64, 64), (240, 240, 240), (0xA4, 0xCE, 0xEB))
d.rounded_rectangle([cx, cy, cx + 230, cy + 100], 10, fill=CHIP, outline=CHIPO, width=2)
d.text((cx + 115, cy + 26), "label-text-focus", font=cf, fill=CHIPT, anchor="mm")
d.rounded_rectangle([cx + 40, cy + 50, cx + 190, cy + 86], 6, fill=(232, 232, 232) if LIGHT else (40, 40, 40), outline=(190, 190, 190) if LIGHT else (70, 70, 70), width=2)
d.rectangle([cx + 46, cy + 55, cx + 76, cy + 81], fill=FOC)
d.text((cx + 84, cy + 68), "#%02X%02X%02X" % FOC, font=cf, fill=CHIPT, anchor="lm")   # live --unity-colors-label-text-focus
PREVIEW = os.environ.get("HERO_PREVIEW")
if PREVIEW: img.save(PREVIEW); sys.exit(0)
img.save(cap("home_hero"))
spec = {"out": "replacements/home/new banner compressed.png", "canvas": [W, H], "backdrop": [25, 26, 27],
        "windows": [{"src": cap("home_hero"), "chrome": "none", "x": 0, "y": 0, "w": W}], **({"theme": "light"} if LIGHT else {})}
LAY = "layouts/home_01" + ("_light" if LIGHT else "") + ".json"
json.dump(spec, open(LAY, "w"), indent=1)
subprocess.run([sys.executable, "scripts/compose.py", LAY], check=True, stdout=subprocess.DEVNULL)
