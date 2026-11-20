"""Accessibility a11y-6Y/6N (disabled text): a real Unity 7 Inspector on a Main Camera (Transform, Camera, Audio Listener and URP's
Universal Additional Camera Data, all collapsed; captures/acc_insp6.png from unity/acc_insp_cam.cs) with its Add Component button
shown disabled. The disabled button is Unity's own 'AC Button' style drawn disabled at the same 230x24pt rect
(captures/acc_acbutton2.png from unity/acc_acbutton2.cs; its enabled drawing matches the real Inspector pixel for pixel).
6N ('don't') uses the same button with its disabled text faded to 30%, like the original's mock."""
import json, os, subprocess, sys
from PIL import Image
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, themed
DY = 4.0 if LIGHT else 0   # light skin: GameObject header 4pt taller (button measured 10px lower)
P = 2.5
insp = Image.open(f"captures/acc_insp6{SUF}.png").convert("RGB"); btn = Image.open(f"captures/acc_acbutton2{SUF}.png").convert("RGB")
for key, by in (("6Y", 50), ("6N", 90)):
    im = insp.copy(); im.paste(btn.crop((round(10 * P), round(by * P), round(240 * P), round((by + 24) * P))), (round(58 * P), round((196.4 + DY) * P)))
    im.save(f"captures/acc_insp_{key}{SUF}.png")
    spec = {"out": f"replacements/fundamentals/accessibility/a11y-{key}.png", "canvas": [780, 530], "backdrop": [203, 203, 203] if LIGHT else [25, 26, 27],
            "windows": [{"src": f"captures/acc_insp_{key}{SUF}.png", "chrome": "none", "x": 44, "y": 69, "w": 692}], "annotations": []}
    lay = f"layouts/accessibility_{key}{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
