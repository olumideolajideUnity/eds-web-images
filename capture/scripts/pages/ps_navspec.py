"""Project Settings: ps-navigation-panel-padding-4.png. Real Unity 7 Project Settings navigation (2.5x native) with measured
spacing: list starts 4pt below the toolbar, rows run edge to edge (0pt side inset), 16pt rows, 14pt indent per level."""
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
p, X, Y0 = 2.5, 140, 36.0
cap = Image.open(f"captures/ps_layout{SUF}.png").convert("RGB")
cap.crop((0, round(Y0 * p), round((780 - X) / p * p), round(Y0 * p) + 604)).save(f"captures/ps_navspec{SUF}.png")
P, G = ([120, 80, 210], [20, 150, 70]) if LIGHT else ([175, 145, 244], [36, 196, 97])
c = lambda pt: (pt - Y0) * p
panel_top, rows_top, right = c(49.6), c(53.6), X + 186 * p
ann = [{"t": "rect", "x": X, "y": panel_top, "w_": right - X, "h": 604 - panel_top - 6, "color": P, "dash": True, "w": 2},
       {"t": "rect", "x": X + 4, "y": rows_top, "w_": right - X - 8, "h": 604 - rows_top - 14, "color": P, "dash": True, "w": 2},
       {"t": "line", "x1": 112, "y1": (panel_top + rows_top) / 2, "x2": X, "y2": (panel_top + rows_top) / 2, "color": P, "w": 2},
       {"t": "dot", "x": X, "y": (panel_top + rows_top) / 2, "r": 4, "color": P},
       {"t": "text", "s": "4px", "x": 104, "y": (panel_top + rows_top) / 2, "anchor": "rm", "size": 19, "color": P, "font": _efds_font("segoeuib.ttf")}]
# indent: parent text (21.6pt) to child text (36pt) on the 'Physics Outline' row (69.6-85.6pt)
y0, y1 = c(69.6), c(85.6); a, b = X + 21.6 * p, X + 36 * p
ann += [{"t": "rect", "x": a, "y": y0, "w_": b - a, "h": y1 - y0, "color": G, "fill": 0.35, "dash": False, "solid": True, "w": 2},
        {"t": "line", "x1": 112, "y1": (y0 + y1) / 2, "x2": a, "y2": (y0 + y1) / 2, "color": G, "w": 2},
        {"t": "dot", "x": a, "y": (y0 + y1) / 2, "r": 4, "color": G},
        {"t": "text", "s": "14px", "x": 104, "y": (y0 + y1) / 2, "anchor": "rm", "size": 19, "color": G, "font": _efds_font("segoeuib.ttf")}]
spec = {"out": "replacements/patterns/project-settings-preferences/ps-navigation-panel-padding-4.png", "canvas": [780, 604], "backdrop": [203, 203, 203] if LIGHT else [25, 25, 25],
        "windows": [{"src": f"captures/ps_navspec{SUF}.png", "chrome": "none", "x": X, "y": 0, "w": 780 - X}], "annotations": ann}
lay = f"layouts/project-settings-preferences_05{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
