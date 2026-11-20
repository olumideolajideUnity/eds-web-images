"""Project Settings: Unity menu.png (Windows). The real Unity 7 main window (composited from its own views) at 1:1 with the menu
bar and the open Edit menu drawn to Unity's owner-draw code (MenuThemeWin.cpp) from Unity's real Edit menu items."""
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
from PIL import Image, ImageDraw, ImageFont
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from native_menu import draw; from theme import LIGHT, SUF, LAY, themed
S = 2; W, H = 1600 * S, 840 * S; k = 1600 / 1536 * S            # device px per 96-dpi px (1pt)
BG, TX, HOT = ((235, 235, 235), (9, 9, 9), (214, 214, 214)) if LIGHT else ((20, 20, 20), (210, 210, 210), (60, 60, 60))   # menu bar / owner-drawn menu per skin
img = Image.new("RGB", (W, H), BG); d = ImageDraw.Draw(img)
bar_h = round(20 * k)
main = Image.open(f"captures/shn_main{SUF}.png").convert("RGB")
mh = H - bar_h; src_h = round(mh / W * main.width)
img.paste(main.crop((0, 0, main.width, src_h)).resize((W, mh), Image.LANCZOS), (0, bar_h))
f = ImageFont.truetype(_efds_font("segoeui.ttf"), round(12 * k))
avg = round(sum(f.getlength(c) for c in "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz") / 52)
x = 0; edit_x = 0
for name in open("captures/ps_edit_items.txt.bar").read().split():
    iw = round(f.getlength(name) + 14 * k + 2 * avg)
    if name == "Edit": d.rectangle([x, 0, x + iw - 1, bar_h - 1], fill=HOT); edit_x = x
    d.text((x + iw / 2, bar_h / 2), name, font=f, fill=TX, anchor="mm"); x += iw
menu, pad = draw("captures/ps_edit_items.txt", k, bg=BG, text=TX, highlight=HOT)
img.paste(menu.convert("RGB"), (edit_x - pad, bar_h - pad), menu)
img.save(f"captures/ps_unitymenu{SUF}.png")
spec = {"out": "replacements/patterns/project-settings-preferences/Unity menu.png", "canvas": [1600, 840], "backdrop": list(BG),
        "windows": [{"src": f"captures/ps_unitymenu{SUF}.png", "chrome": "none", "x": 0, "y": 0, "w": 1600}]}
lay = f"layouts/project-settings-preferences_03{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
print("menu", menu.size)
