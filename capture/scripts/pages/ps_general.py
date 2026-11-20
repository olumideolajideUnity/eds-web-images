"""Project Settings: ps-general.png. Real Unity 7 Preferences > General (captures/ps_general[_l].png: unity/ps_open.cs KIND=User
800x330 + unity/ps_select.cs "Preferences/_General") at 2x. The Compilation Cache Location value is a local path containing the
user name, so it is blurred (privacy; the field itself stays visible)."""
import json, os, subprocess, sys
from PIL import Image, ImageFilter
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, themed
p = 2.5
cap = Image.open(f"captures/ps_general{SUF}.png").convert("RGB")
PATH = tuple(round(v * p) for v in (463, 188, 610.4, 207))       # the path field (pt); the Change Cache Location button stays sharp
cap.paste(cap.crop(PATH).filter(ImageFilter.GaussianBlur(9)), PATH[:2]); cap.save(f"captures/ps_general_b{SUF}.png")
spec = {"out": "replacements/patterns/project-settings-preferences/ps-general.png", "canvas": [1600, 600], "backdrop": [203, 203, 203] if LIGHT else [25, 25, 25],
        "windows": [{"src": f"captures/ps_general_b{SUF}.png", "chrome": "none", "x": 0, "y": 0, "w": 1600, "h": 600}]}
lay = f"layouts/project-settings-preferences_01{LAY}.json"; json.dump(themed(spec) if LIGHT else spec, open(lay, "w"), indent=1)
subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
