"""Accessibility a11y-1Y/1N/2Y/2N: real Unity 7 Console entries (unity/acc_console.cs) whose text is coloured with rich text
in Unity 7's live warning-text and error-text tokens (dark #F4BC02 / #D32222, light #333308 / #5A0000), at the originals' 2x framing (window running off
the right and bottom edges). 2N ("don't": colour only) is the 2Y capture with its type icons removed and the text moved
into the icon column (rows y 128-400 px moved 45 px left from x 25), as the original was constructed."""
import json, os, subprocess, sys
from PIL import Image
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, themed
a = Image.open(f"captures/acc_2Y{SUF}.png").convert("RGB"); o = a.copy(); Y0, Y1, X0, SH = 128, 400, 25, 45
for yy in range(Y0, Y1):
    o.paste(a.crop((X0 + SH, yy, a.width, yy + 1)), (X0, yy)); o.paste(a.getpixel((a.width - 1, yy)), (a.width - SH, yy, a.width, yy + 1))
o.save(f"captures/acc_2N{SUF}.png")
NBSP = "\u202f"
SETS = {"1Y": ("acc_1Y", 110, 116), "1N": ("acc_1N", 110, 116), "2Y": ("acc_2Y", 96, 98), "2N": ("acc_2N", 96, 98)}
for k, (cap, x, y) in SETS.items():
    out = f"replacements/fundamentals/accessibility/a11y-{k} 2.45.34{NBSP}PM.png"
    spec = {"out": out, "canvas": [780, 480], "backdrop": [203, 203, 203] if LIGHT else [25, 26, 27],
            "windows": [{"src": f"captures/{cap}{SUF}.png", "chrome": "none", "x": x, "y": y, "w": 1280}], "annotations": []}
    lay = f"layouts/accessibility_{k}{LAY}.json"
    json.dump(themed(spec), open(lay, "w", encoding="utf-8"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
