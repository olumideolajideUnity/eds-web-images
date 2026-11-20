"""Project Settings: ps-navigation-content-padding-8-4.png. Real Unity 7 Preferences > General content with the spacing read from
the Editor's style catalogue and checked against pixels: window margin 3/4pt, panel margin 10/10pt, header 26pt, label
column fixed at 250pt (-unity-label-width), fields fill the rest."""
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
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, themed
p, K, X, X0, Y0 = 2.5, 1.6, 140, 194.0, 40.0
cap = Image.open(f"captures/ps_general_tall{SUF}.png").convert("RGB")
from PIL import ImageFilter
PATH = tuple(round(v * p) for v in (463, 188, 800, 207))   # Compilation Cache Location value: a local path with the user name -> blurred (privacy)
cap.paste(cap.crop(PATH).filter(ImageFilter.GaussianBlur(9)), PATH[:2])
wpt, hpt = (780 - X) / K, 604 / K
cap.crop((round(X0 * p), round(Y0 * p), round((X0 + wpt) * p), round((Y0 + hpt) * p))).save(f"captures/ps_contentspec{SUF}.png")
P, C = ([120, 80, 210], [205, 80, 60]) if LIGHT else ([175, 145, 244], [255, 128, 112])
cx = lambda pt: X + (pt - X0) * K; cy = lambda pt: (pt - Y0) * K
L, R = 209, X0 + wpt + 4
ann = []
def lab(s, y, col, tx=None):
    return [{"t": "line", "x1": 108, "y1": y, "x2": tx or X, "y2": y, "color": col, "w": 2}, {"t": "dot", "x": tx or X, "y": y, "r": 4, "color": col},
            {"t": "text", "s": s, "x": 100, "y": y, "anchor": "rm", "size": 17, "color": col, "font": _efds_font("consolab.ttf")}]
ann += [{"t": "rect", "x": cx(L), "y": cy(53.2), "w_": cx(R) - cx(L), "h": cy(79.2) - cy(53.2), "color": P, "fill": 0.12, "dash": True, "w": 2},
        {"t": "rect", "x": cx(L), "y": cy(89.2), "w_": cx(R) - cx(L), "h": 604 - cy(89.2) + 10, "color": P, "dash": True, "w": 2}]
ann += lab("4px", (cy(49.2) + cy(53.2)) / 2, P, cx(L)) + lab("10px", (cy(79.2) + cy(89.2)) / 2, P, cx(L))
ann += [{"t": "line", "x1": cx(196), "y1": cy(118), "x2": cx(L), "y2": cy(118), "color": P, "w": 2}] + lab("13px", cy(118), P, cx(196))
ann += [{"t": "rect", "x": cx(L), "y": cy(91), "w_": 250 * K, "h": cy(330) - cy(91), "color": C, "dash": False, "solid": True, "w": 2},
        {"t": "rect", "x": cx(465), "y": cy(91), "w_": cx(R) - cx(465), "h": cy(330) - cy(91), "color": C, "dash": False, "solid": True, "w": 2}]
ann += lab("Columns\n250/fill", cy(160), C, cx(L) + 30)
spec = {"out": "replacements/patterns/project-settings-preferences/ps-navigation-content-padding-8-4.png", "canvas": [780, 604], "backdrop": [203, 203, 203] if LIGHT else [25, 25, 25],
        "windows": [{"src": f"captures/ps_contentspec{SUF}.png", "chrome": "none", "x": X, "y": 0, "w": 780 - X}], "annotations": ann}
lay = f"layouts/project-settings-preferences_06{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
