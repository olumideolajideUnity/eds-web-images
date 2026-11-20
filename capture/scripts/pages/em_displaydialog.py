"""Errors & messaging: DialogMessageType.png. Unity 7's EditorUtility.DisplayDialog on Windows (Win32 IDD_DIALOG_TWO_OPT, laid out by
EditorDialog.cpp, unchanged so the original's geometry is kept) with Unity 7's dark dialog theme (MenuThemeWin.cpp ApplyDialogTheme):
caption/background = --unity-colors-app_toolbar-background (20,20,20), text = --unity-colors-default-text (210,210,210), immersive
dark title bar, push buttons + checkbox switched to Windows' DarkMode_Explorer theme (rendered here with uxtheme), Unity's own
DialogWarnIcon.png. Message font is the dialog's MS Shell Dlg (Tahoma)."""
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
from PIL import Image, ImageDraw, ImageFilter, ImageFont
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from uxtheme_parts import render, color; from theme import LIGHT, SUF, LAY, themed
# light skin: the same theme variables in their light values (app_toolbar-background 235, default-text 9, as the light native menus) and
# Windows' light Explorer::Button parts; dark: as above
THEMECLS = "Explorer::Button" if LIGHT else "DarkMode_Explorer::Button"
BG, TX, BTX = ((235, 235, 235), (9, 9, 9), color("Explorer::Button", 1, 1) or (0, 0, 0)) if LIGHT else ((20, 20, 20), (210, 210, 210), color("DarkMode_Explorer::Button", 1, 1) or (255, 255, 255))
PAGE, EDGE, FOCUS = ((226, 226, 226), (200, 200, 200), (90, 90, 90)) if LIGHT else ((56, 56, 56), (58, 58, 58), (170, 170, 170))
X0, Y0, X1, Y1, TB = 393, 123, 1205, 478, 188          # dialog window and title-bar bottom (original geometry)
img = Image.new("RGB", (1600, 600), PAGE)
sh = Image.new("L", img.size, 0); ImageDraw.Draw(sh).rounded_rectangle([X0, Y0 + 6, X1, Y1 + 8], 16, fill=90 if LIGHT else 150)
img.paste((0, 0, 0), (0, 0), sh.filter(ImageFilter.GaussianBlur(18)))
d = ImageDraw.Draw(img)
d.rounded_rectangle([X0, Y0, X1, Y1], 16, fill=BG, outline=EDGE, width=2)
ft = ImageFont.truetype(_efds_font("segoeui.ttf"), 27); d.text((413, 156), "DisplayDialog Test", font=ft, fill=TX, anchor="lm")
cx, cy, g = 1168, 153, 8; d.line([cx - g, cy - g, cx + g, cy + g], fill=TX, width=2); d.line([cx - g, cy + g, cx + g, cy - g], fill=TX, width=2)
ic = Image.open(os.environ.get("EFDS_UNITY_SRC", "unity-src") + "/Editor/Resources/Windows/DialogIcons/DialogWarnIcon.png").convert("RGBA").resize((96, 96), Image.LANCZOS)
img.paste(ic, (426, 222), ic)
fm = ImageFont.truetype(_efds_font("tahoma.ttf"), 24.5)
for i, line in enumerate(("This is an example of a dialog with two buttons and an", "opt-out checkbox. If you click OK and check the box,", "Unity will remember your decision until you restart.")):
    d.text((552, 226 + i * 30), line, font=fm, fill=TX, anchor="lm")
for (x0, x1, label, state) in ((816, 989, "OK", 5), (1010, 1183, "Cancel", 1)):
    b = render(THEMECLS, 1, state, x1 - x0, 48, BG, dpi=192); img.paste(b, (x0, 341))
    d.text(((x0 + x1) / 2, 365), label, font=fm, fill=BTX, anchor="mm")
for x in range(822, 984, 2): d.point([(x, 346), (x, 383)], fill=FOCUS)       # keyboard focus rect on the default button
for y in range(346, 384, 2): d.point([(821, y), (983, y)], fill=FOCUS)
cb = render(THEMECLS, 3, 1, 32, 32, BG, dpi=192); img.paste(cb, (411, 430))
d.text((448, 447), "Don't ask again for this session", font=fm, fill=BTX, anchor="lm")
img.save(f"captures/em_displaydialog{SUF}.png")
spec = {"out": "replacements/patterns/errors-and-messaging/DialogMessageType.png", "canvas": [1600, 600], "backdrop": list(PAGE),
        "windows": [{"src": f"captures/em_displaydialog{SUF}.png", "chrome": "none", "x": 0, "y": 0, "w": 1600}]}
lay = f"layouts/errors-and-messaging_08{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
