"""Render Unity 7 owner-drawn dropdown menus (scripts/pages/native_menu.py, following Editor/Platform/Windows/MenuThemeWin.cpp)
for the Dropdown page at the zoom each original uses. Saved at 2x the canvas scale (for crisp @2x output) with their pad."""
import json, os, sys
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from native_menu import draw
LIGHT = os.environ.get("THEME") == "light"; SUF = "_l" if LIGHT else ""
MENU = dict(bg=(235, 235, 235), text=(9, 9, 9), highlight=(214, 214, 214)) if LIGHT else {}
out = {}
for name, items, k, hot, minw in (("anatomy", "dd_anatomy", 3.6, "Checked option", 138), ("position", "dd_position", 2.4, "Directional", 172),
                                  ("os", "dd_os", 2.0, "OS Menu Dropdown", 168)):
    img, pad = draw(f"captures/menus/{items}.txt", k * 2, hot=hot, min_width=minw, **MENU)
    img.save(f"captures/menus/dd_{name}{SUF}.png"); out[name] = {"pad": pad / 2, "w": img.width / 2, "h": img.height / 2, "k": k}
json.dump(out, open(f"captures/menus/dd_menus{SUF}.json", "w"), indent=1); print(out)
