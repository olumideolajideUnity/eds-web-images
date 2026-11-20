"""Typography > Indentation: Unity 7 Inspector on a temp unsaved Main Camera with Transform and the built-in Camera component
(URP Camera Inspector; captures/ty_indent.png from unity/ty_indent_cam.cs, H=700, cropped to the original's 381pt), with the
original's dashed indent guide. Guide x positions sit 1pt left of the measured label glyphs (Transform's labels start at
20.4pt; inside Camera, Projection at 41.6pt and its nested fields at 59.6pt) and it steps at row boundaries, clear of the
foldout arrows and titles (Projection header: arrow at 22pt, title at 34pt)."""
import json, os, subprocess, sys
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, G, themed
X, Y, K, PW, CH = 412, 30, 2, 387.2, 792
x0, x1, x2 = (X + v * K for v in (19.0, 40.4, 58.0))
DY = 4.0 if LIGHT else 0                     # the light skin's GameObject header is 4pt taller (measured: every separator 10px lower)
y1, y2 = Y + (257.6 + DY) * K, Y + (279.0 + DY) * K          # bottom of the Projection header row; boundary below the Projection field row
D = lambda **a: dict(a, color=G, w=2)
ann = [D(t="vdash", x=x0, y1=0, y2=y1), D(t="hdash", x1=x0, x2=x1, y=y1), D(t="vdash", x=x1, y1=y1, y2=y2),
       D(t="hdash", x1=x1, x2=x2, y=y2), D(t="vdash", x=x2, y1=y2, y2=CH)]
spec = {"out": "replacements/fundamentals/typography/typography - indent.png", "canvas": [1600, CH], "backdrop": [203, 203, 203] if LIGHT else [25, 25, 25],
        "windows": [{"src": f"captures/ty_indent{SUF}.png", "chrome": "none", "x": X, "y": Y, "w": PW * K, "h": CH - Y}],
        "annotations": ann}
lay = f"layouts/typography_03{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
