"""Project Settings: label-doc.png. Real Unity 7 Inspector (Mesh Renderer; captures/ps_meshr[_l].png, unity/ps_meshr.cs) cropped at 4x
around one property label, with a dashed box measured from the label's glyph pixels (light text in dark, dark text in light). The
light skin's Inspector header is 4pt taller, so its rows sit 4pt lower."""
import json, os, subprocess, sys
from PIL import Image
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, themed
im = Image.open(f"captures/ps_meshr{SUF}.png").convert("RGB"); p = 2.5; DY = 4 if LIGHT else 0
ink = (lambda c: sum(c) < 300) if LIGHT else (lambda c: sum(c) > 420)
xs = [x / p for x in range(0, round(250 * p)) for y in range(round((389 + DY) * p), round((404 + DY) * p), 2) if ink(im.getpixel((x, y)))]
ys = [y / p for y in range(round((386 + DY) * p), round((408 + DY) * p)) if any(ink(im.getpixel((x, y))) for x in range(0, round(250 * p), 2))]
print("label x", min(xs), max(xs), "y", min(ys), max(ys))
Y0 = 386 + DY; K = 4
im.crop((0, round(Y0 * p), round(400 * p), round((Y0 + 69) * p))).save(f"captures/ps_label{SUF}.png")
x0, x1, y0, y1 = min(xs) - 3, max(xs) + 4, min(ys) - 3, max(ys) + 3
spec = {"out": "replacements/patterns/project-settings-preferences/label-doc.png", "canvas": [1600, 276], "backdrop": [203, 203, 203] if LIGHT else [25, 25, 25],
        "windows": [{"src": f"captures/ps_label{SUF}.png", "chrome": "none", "x": 0, "y": 0, "w": 1600}],
        "annotations": [{"t": "rect", "x": x0 * K, "y": (y0 - Y0) * K, "w_": (x1 - x0) * K, "h": (y1 - y0) * K, "color": [120, 80, 210] if LIGHT else [175, 145, 244], "dash": True, "w": 2}]}
lay = f"layouts/project-settings-preferences_10{LAY}.json"; json.dump(themed(spec) if LIGHT else spec, open(lay, "w"), indent=1)
subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
