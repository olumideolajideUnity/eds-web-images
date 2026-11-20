"""Project Settings: kebab-preferences.png. Real Unity 7 main window crop (own views) at 2x with the Hierarchy pane menu, a native
menu Unity owner-draws (MenuThemeWin.cpp), drawn from the items HostView.PopupGenericMenu builds (pane_menu_dump.cs)."""
import json, os, subprocess, sys
from PIL import Image
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from native_menu import draw; from theme import LIGHT, SUF, LAY, themed
KX = 237.0 if LIGHT else 276.0   # Hierarchy kebab centre x (pt), measured: the light session's Hierarchy pane is narrower
S, PX = 2, 2.5; Z = 1.5; KP = Z * S                                   # canvas px per pt at the 2x render
main = Image.open(f"captures/shn_main{SUF}.png").convert("RGB")
img = main.crop((0, 0, round(1600 / Z * PX), round(600 / Z * PX))).resize((1600 * S, 600 * S), Image.LANCZOS)
menu, pad = draw("captures/pane_HierarchyWindow.txt", KP, **({"bg": (235, 235, 235), "text": (9, 9, 9), "highlight": (214, 214, 214)} if LIGHT else {}))
img.paste(menu.convert("RGB"), (round(KX * KP) - pad, round(53 * KP) - pad), menu)
img.save(f"captures/ps_panemenu{SUF}.png")
spec = {"out": "replacements/patterns/project-settings-preferences/kebab-preferences.png", "canvas": [1600, 600], "backdrop": [203, 203, 203] if LIGHT else [25, 25, 25],
        "windows": [{"src": f"captures/ps_panemenu{SUF}.png", "chrome": "none", "x": 0, "y": 0, "w": 1600}]}
lay = f"layouts/project-settings-preferences_07{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
