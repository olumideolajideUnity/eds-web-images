"""Project Settings: kebab-options.png. Real Unity 7 Project browser (captures/ps_project[_l].png, unity/ps_project.cs) cropped at ~5x
around its tab strip's kebab (located from its pixels: light dots in dark, dark dots in light), with the original's leader and pill."""
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
cap = Image.open(f"captures/ps_project{SUF}.png").convert("RGB"); p = 2.5
Z = 5.0                                     # canvas px per pt (the original is ~5x)
wpt, hpt = 1230 / Z, 600 / Z
x0 = 520 - wpt
crop = cap.crop((round(x0 * p), 0, cap.width, round(hpt * p))); crop.save(f"captures/ps_kebab{SUF}.png")
dot = (lambda c: sum(c) < 300) if LIGHT else (lambda c: sum(c) > 450)
pts = [(x / p, y / p) for x in range(round(500 * p), round(520 * p)) for y in range(0, round(26 * p)) if dot(cap.getpixel((x, y)))]
kx = sum(a for a, b in pts) / len(pts); ky = sum(b for a, b in pts) / len(pts); print("kebab", kx, ky)
cx, cy = (kx - x0) * Z, ky * Z
LINE, TXT, PBG, POUT, BD = ([150, 150, 150], [9, 9, 9], [226, 226, 226], [160, 160, 160], [226, 226, 226]) if LIGHT else ([110, 110, 110], [255, 255, 255], [40, 40, 40], [80, 80, 80], [0, 0, 0])
spec = {"out": "replacements/patterns/project-settings-preferences/kebab-options.png", "canvas": [1600, 600], "backdrop": BD,
        "windows": [{"src": f"captures/ps_kebab{SUF}.png", "chrome": "none", "x": 0, "y": 0, "w": 1230}],
        "annotations": [{"t": "line", "x1": cx + 60, "y1": cy, "x2": 1258, "y2": cy, "color": LINE, "w": 1},
                        {"t": "dot", "x": cx + 60, "y": cy, "r": 3, "color": LINE},
                        {"t": "pill", "x": 1258, "y": cy, "h": 46, "s": "Tool Preferences", "color": TXT, "bg": PBG, "outline": POUT, "size": 17, "pad": 13, "font": _efds_font("segoeui.ttf")}]}
lay = f"layouts/project-settings-preferences_08{LAY}.json"; json.dump(themed(spec) if LIGHT else spec, open(lay, "w"), indent=1)
subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
