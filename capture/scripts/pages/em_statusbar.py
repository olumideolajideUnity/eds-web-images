"""Errors & messaging: ea-color use.png. Three columns like the original, each showing Unity 7's own console icon (large,
d_console.*icon@2x) above the real main-window status bar after logging an Error / Warning / Log message."""
import json, os, subprocess, sys
from PIL import Image, ImageDraw
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, themed
orig = Image.open("original/em/ea-color use.png").convert("RGB")
BG, LINE = ((226, 226, 226), (180, 180, 180)) if LIGHT else (orig.getpixel((100, 100)), orig.getpixel((533, 100)))
S = 2; W, H = 1600 * S, 600 * S
img = Image.new("RGB", (W, H), BG); d = ImageDraw.Draw(img)
cols = [(0, 533), (535, 1067), (1069, 1600)]
for x in (533, 1067): d.rectangle([x * S, 0, (x + 2) * S - 1, H], fill=LINE)
for (x0, x1), t, icon in zip(cols, ("Error", "Warning", "Log"), ("erroricon", "warnicon", "infoicon")):
    sb = Image.open(f"captures/em_sb_{t}{SUF}.png").convert("RGB")          # 2.5 px per pt
    sbg = sb.getpixel((sb.width - 4, sb.height // 2))
    xs = [x for x in range(0, sb.width // 2) if any(sum(abs(a - b) for a, b in zip(sb.getpixel((x, y)), sbg)) > 40 for y in range(4, sb.height - 4, 2))]
    msg = sb.crop((max(0, xs[0] - 6), 0, xs[-1] + 6, sb.height))
    k = 2 * S / 2.5                                                    # status bar at 2x like the original
    msg = msg.resize((round(msg.width * k), round(msg.height * k)), Image.LANCZOS)
    cx = (x0 + x1) // 2 * S; sy = 358 * S
    d.rectangle([x0 * S if x0 == 0 else (x0) * S, sy, x1 * S - 1, sy + msg.height - 1], fill=sbg)
    img.paste(msg, (cx - msg.width // 2, sy))
    ic = Image.open(f"captures/console.{icon}{SUF}.png").convert("RGBA"); sz = 56 * S   # light: console.*icon (64px; no @2x in the light skin)
    ic = ic.resize((sz, sz), Image.LANCZOS); img.paste(ic, (cx - sz // 2, 242 * S - sz // 2), ic)
for x in (533, 1067): d.rectangle([x * S, 0, (x + 2) * S - 1, H], fill=LINE)
img.save(f"captures/em_statusbar_full{SUF}.png")
spec = {"out": "replacements/patterns/errors-and-messaging/ea-color use.png", "canvas": [1600, 600], "backdrop": list(BG),
        "windows": [{"src": f"captures/em_statusbar_full{SUF}.png", "chrome": "none", "x": 0, "y": 0, "w": 1600}]}
lay = f"layouts/errors-and-messaging_05{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
print("bg", BG, "line", LINE)
