"""Inheritance overview: two real Unity 7 Inspectors (Cube prefab instance with Scale overridden; Atrio's WoodTrimKit_Orange
material variant with Base Color overridden), at 2x like the original, with the original's titles, icons, underlines and arrows."""
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
from PIL import Image
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, INK, themed
ppp, K, VIS = 2.5, 2, 262          # capture px/pt, canvas px/pt, visible window height (pt)
def bar(src, test):
    im = Image.open(src).convert("RGB"); runs, cur = [], None
    for y in range(int(60 * ppp), im.height):
        if test(im.getpixel((1, y))): cur = cur or [y, y]; cur[1] = y
        elif cur: runs.append(cur); cur = None
    r = max(runs, key=lambda r: r[1] - r[0]); return (r[0] + r[1]) / 2 / ppp
blue = lambda p: p[2] > 180 and p[2] > p[0] + 100
grey = (lambda p: p[0] < 40 and max(p) - min(p) < 8) if LIGHT else (lambda p: p[0] > 170 and max(p) - min(p) < 8)   # material override bar: near-black in light
BL, GR, TXT = ([40, 110, 200], [60, 60, 60], INK) if LIGHT else ([66, 152, 228], [196, 196, 196], [228, 229, 230])
wins, ann = [], []
for src, x, test, col, title, icon, ux in (
        (f"captures/inh_prefab{SUF}.png", 82, blue, BL, "Prefab instance with overridden property", f"captures/icon_d_Prefab_Icon{SUF}.png", 0),
        (f"captures/inh_mat{SUF}.png", 880, grey, GR, "Material variant with overridden property", f"captures/icon_d_Material_Icon{SUF}.png", 0)):
    y0 = 82
    wins.append({"src": src, "chrome": "none", "x": x, "y": y0, "w": 320 * K, "h": VIS * K})
    by = y0 + bar(src, test) * K
    ann.append({"t": "line", "x1": x - 56, "y1": by, "x2": x - 32, "y2": by, "color": col, "w": 2})
    ann.append({"t": "line", "x1": x - 40, "y1": by - 8, "x2": x - 32, "y2": by, "color": col, "w": 2})
    ann.append({"t": "line", "x1": x - 40, "y1": by + 8, "x2": x - 32, "y2": by, "color": col, "w": 2})
    ann.append({"t": "text", "s": title, "x": x + 42, "y": 42, "size": 24, "color": TXT, "font": _efds_font("segoeui.ttf"), "_underline": "overridden", "_ucol": col})
    ann.append({"t": "image", "src": icon, "x": x + 2, "y": 28, "w": 28})
spec = {"out": "replacements/patterns/inheritance/inheritance-overview.png", "canvas": [1600, 600], "backdrop": [203, 203, 203] if LIGHT else [25, 25, 25], "windows": wins, "annotations": ann}
lay = f"layouts/inheritance_01{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
print("ok")
