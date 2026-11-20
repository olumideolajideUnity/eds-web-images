"""Images that need a UI Toolkit document (Unity 7 has no separate UI Builder window): the saved capture scene
Assets/EFDS_Screenshots/UIToolkitDoc.unity (unity/sf_uidoc.cs) with a UIDocument; captures/tg_vp.png = Window > UI Toolkit > UI Viewport,
captures/sf_insp.png = the UI element Inspector (Button selected in the viewport), captures/tt_uib.png = the same with 'Hello World' typed
into the Tooltip attribute and focused (unity/tt_uitooltip.cs). Composes Tooltip #2, Search field #6 and #9, Toggle #6, Toolbar #2."""
import os as _os; _EFDS_TMP = _os.environ.get("EFDS_TMP", "C:/tmp")
import json, os, subprocess, sys
from PIL import Image, ImageFont
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, G, FILL, OUT, BG, INK, TXT, themed; from native_menu import draw
F, FB = "fonts/Inter-Regular.ttf", "fonts/Inter-SemiBold.ttf"; K = 2.5
MENU = dict(bg=(235, 235, 235), text=(9, 9, 9), highlight=(214, 214, 214)) if LIGHT else {}
L = lambda a, b, c, d, col=G, w=2: {"t": "line", "x1": a, "y1": b, "x2": c, "y2": d, "color": col, "w": w}
def badge(n, cx, cy): return [{"t": "dot", "x": cx, "y": cy, "r": 20, "color": G}, {"t": "dot", "x": cx, "y": cy, "r": 18, "color": FILL},
                              {"t": "text", "s": str(n), "x": cx, "y": cy, "anchor": "mm", "size": 22, "color": G, "font": F}]
num = lambda s, x, y: [{"t": "dot", "x": x, "y": y, "r": 17, "color": OUT}, {"t": "dot", "x": x, "y": y, "r": 15, "color": BG},
                       {"t": "text", "s": s, "x": x, "y": y, "anchor": "mm", "size": 20, "color": INK, "font": FB}]
text = lambda s, x, y, size=22: {"t": "text", "s": s, "x": x, "y": y, "size": size, "color": TXT, "font": F}
def crop(src, x0, y0, x1, y1):
    w, h = Image.open(src.replace(".png", SUF + ".png")).size; return [int(x0 * K), int(y0 * K), w - int(x1 * K), h - int(y1 * K)]
def run(lay, out, wins, ann, canvas=(1600, 600)):
    os.makedirs(os.path.dirname(out), exist_ok=True)
    spec = {"out": out, "canvas": list(canvas), "backdrop": BG, "windows": wins, "annotations": ann}
    out = out.replace(".png", "--light.png") if False else out
    for wd in spec["windows"]: wd["src"] = wd["src"].replace(".png", SUF + ".png")
    lay = f"layouts/{lay}{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)

# Tooltip #2: the Tooltip attribute in the UI element Inspector, typed and focused
run("tooltip_02", "replacements/components/tooltip/tooltips-uib.png",
    [{"src": "captures/tt_uib.png", "chrome": "none", "x": 0, "y": 0, "w": 640, "crop": crop("captures/tt_uib.png", 0, 255, 380, 540)}], [], (640, 480))

# Search field #6: the UI element Inspector's search (always shown in Unity 7: search field + filter toggles)
run("search-field_06", "replacements/components/search-field/condensedsearch-style.png",
    [{"src": "captures/sf_insp.png", "chrome": "none", "x": 420, "y": 0, "w": 760, "crop": crop("captures/sf_insp.png", 0, 0, 380, 300)}], [])

# Search field #9: anatomy 1-6 (Unity 7 has no condensed / collapse buttons, the original's 7 and 8)
r = {k: [v * 2 for v in vv] for k, vv in json.load(open(f"{_EFDS_TMP}/sf_anatomy{SUF}.json")).items()}
ux, uy, uw, uh = r["user"]; px, py, pw, ph = r["popup"]; cx_, cy_, cw_, ch_ = r["cond"]
img, pad = draw("captures/menus/sf_popup.txt", 6, min_width=pw / 3, **MENU); img.save(f"captures/menus/sf_popup_15{SUF}.png")
menu_y = py + ph + 3
ann = [{"t": "rect", "x": cx_ - 20, "y": cy_ - 20, "w_": cw_ + 40, "h": ch_ + 40, "color": BG, "w": 0, "fill": 1.0, "dash": False},
       {"t": "image", "src": f"captures/menus/sf_popup_15{SUF}.png", "x": px - pad / 2, "y": menu_y - pad / 2, "w": img.width / 2}]
sx, sy, sw, sh = r["user:search"]; ann += [{"t": "dot", "x": sx + sw / 2, "y": sy + sh / 2, "r": 5, "color": G}, L(sx + sw / 2, sy + sh / 2, px - 120, sy + sh / 2)] + badge(1, px - 140, sy + sh / 2)
kx, ky, kw, kh = r["user:cancel"]; ann += [{"t": "dot", "x": kx + kw / 2, "y": ky + kh / 2, "r": 5, "color": G}, L(kx + kw / 2, ky + kh / 2, ux + uw + 90, ky + kh / 2)] + badge(2, ux + uw + 110, ky + kh / 2)
ann += [L(px + pw + 14, py, px + pw + 14, py + ph), L(px + pw + 6, py, px + pw + 14, py), L(px + pw + 6, py + ph, px + pw + 14, py + ph), L(px + pw + 14, py + ph / 2, ux + uw + 90, py + ph / 2)] + badge(3, ux + uw + 110, py + ph / 2)
tx, ty, tw, th = r["user:text"]; ann += [L(tx + 20, ty + 4, tx + 20, uy - 30)] + badge(4, tx + 20, uy - 50)
ptx, pty, ptw, pth = r["popup:text"]; ann += [{"t": "dot", "x": ptx + 2, "y": pty + pth - 4, "r": 4, "color": G}, L(ptx + 2, pty + pth - 4, ptx + 2, menu_y + 2), L(ptx + 2, menu_y + 2, px - 120, menu_y + 2)]
ann = ann[:-3] + [L(ptx + 2, pty + pth / 2, px - 120, pty + pth / 2)] + badge(5, px - 140, pty + pth / 2)
my = menu_y + img.height / 4; ann += [L(px - 4, my, px - 120, my)] + badge(6, px - 140, my)
run("search-field_09", "replacements/components/search-field/searchfield-anatomy.png", [{"src": "captures/sf_anatomy.png", "chrome": "none", "x": 0, "y": 0, "w": 1600}], ann)

# Toggle #6: the 'Preview interactions' toggle in the UI Viewport toolbar, and regular checkboxes
run("toggle_06", "replacements/components/toggle/Toggles.png",
    [{"src": "captures/tg_prefsspec.png", "chrome": "none", "x": 0, "y": 0, "w": 1600},
     {"src": "captures/tg_vp.png", "chrome": "none", "x": 40, "y": 30, "w": 1520, "crop": crop("captures/tg_vp.png", 0, 0, 850, 72)}],
    [text("\u201cPreview interactions\u201d toggle in the UI Viewport toolbar (UI Toolkit).", 40, 196), L(0, 300, 1600, 300, OUT, 1),
     text("Regular Toggles (Checkboxes) in use.", 1100, 420)])

# Toolbar #2: 1 UI Viewport toolbar (UI Toolkit), 2 Scene view toolbar (overlays)
run("toolbar_02", "replacements/components/toolbar/toolbar-variations.png",
    [{"src": "captures/tg_vp.png", "chrome": "none", "x": 60, "y": 126, "w": 1480, "crop": crop("captures/tg_vp.png", 0, 48, 850, 72)},
     {"src": "captures/tl_scene.png", "chrome": "none", "x": 60, "y": 426, "w": 1480, "crop": crop("captures/tl_scene.png", 0, 26, 820, 51)}],
    [L(0, 300, 1600, 300, OUT, 1)] + num("1", 60, 60) + num("2", 60, 360))
