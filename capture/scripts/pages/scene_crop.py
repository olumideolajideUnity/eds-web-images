"""Crop the Scene toolbar off a full-res Scene view capture and compose it as a bare crop at the original's size.
Usage: python scripts/pages/scene_crop.py <capture-name> <out-path> <width> <height> <layout-name> [anchor 0..1]
THEME=light: reads captures/<capture-name>_l.png and writes layouts/<layout-name>_light.json (the --light variants)."""
import json, subprocess, sys, os
sys.path.insert(0, os.path.dirname(os.path.abspath(__file__))); from theme import LIGHT, SUF, LAY, themed
from PIL import Image
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
cap, out, W, H, lay = sys.argv[1] + SUF, sys.argv[2], int(sys.argv[3]), int(sys.argv[4]), sys.argv[5] + LAY
ANCHOR = float(sys.argv[6]) if len(sys.argv) > 6 else 0.0   # 0 = top, 0.5 = centered, 1 = bottom
im = Image.open(f"captures/{cap}.png").convert("RGB")
y = 0
while y < 200:                                # first row of scene content (purple background: blue > green)
    r, g, b = im.getpixel((im.width // 2, y))
    if b > g + 8: break
    y += 1
crop = im.crop((0, y + 2, im.width, im.height))
need = round(crop.width * H / W)              # crop to the original's aspect (trim height, or width if too short)
if need <= crop.height:
    top = round((crop.height - need) * ANCHOR); crop = crop.crop((0, top, crop.width, top + need))
else: crop = crop.crop((0, 0, round(crop.height * W / H), crop.height))
crop.save(f"captures/{cap}_crop.png")
spec = themed({"out": out, "canvas": [W, H], "backdrop": [25, 25, 25], "windows": [{"src": f"captures/{cap}_crop.png", "chrome": "none", "x": 0, "y": 0, "w": W}]})
json.dump(spec, open(f"layouts/{lay}.json", "w"), indent=1)
subprocess.run([sys.executable, "scripts/compose.py", f"layouts/{lay}.json"], check=True, stdout=subprocess.DEVNULL)
print("built", out, W, H, "toolbar cut at", y)
