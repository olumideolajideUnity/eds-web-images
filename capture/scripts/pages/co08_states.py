"""Content organization #8 (2-2-fold_hover.png): foldout states, rendered by Unity 7's own foldout style in forced states."""
import json, os, subprocess, sys
from PIL import Image

ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, themed
K = 2.0
d = json.load(open(f"captures/co08_raw{SUF}.json")); im = Image.open(f"captures/co08_raw{SUF}.png").convert("RGB")
ppp, (wx, wy) = d["ppp"], d["win"][:2]
ax, ay, aw, ah = d["rects"]["area"]
K = 780 / aw                                   # match the original 780 px width
crop = im.crop((int((ax - wx) * ppp), int((ay - wy) * ppp), int((ax - wx + aw) * ppp), int((ay - wy + ah) * ppp)))
crop.save(f"captures/co08_crop{SUF}.png")

# Unity 7 hover check: compare the idle and hover rows pixel by pixel
rh = crop.height // 4
idle, hover = crop.crop((0, 0, crop.width, rh)), crop.crop((0, rh, crop.width, 2 * rh))
diff = sum(1 for a, b in zip(idle.getdata(), hover.getdata()) if a != b)
print("pixels differing between idle and hover rows:", diff)

W, H = 780, round(ah * K)
print('area pt', aw, ah, 'K', round(K, 3), 'canvas', W, H)
ann = [{"t": "line", "x1": 0, "y1": i * H / 4, "x2": W, "y2": i * H / 4, "color": [170, 170, 170] if LIGHT else [80, 80, 80], "w": 1} for i in (1, 2, 3)]
for i, label in enumerate(("Idle", "Hover", "Focused", "Disabled")):
    ann.append({"t": "text", "s": label, "x": W - 59, "y": (i + .5) * H / 4, "anchor": "rm", "size": 22, **({"color": [120, 80, 210]} if LIGHT else {})})
ann.append({"t": "rect", "x": .5, "y": .5, "w_": W - 1, "h": H - 1, "dash": False, "solid": True, "color": [160, 160, 160] if LIGHT else [24, 24, 24], "w": 1})
spec = {"out": "replacements/patterns/content-organization/2-2-fold_hover.png", "canvas": [W, H], "backdrop": [203, 203, 203] if LIGHT else [25, 25, 25],
        "windows": [{"src": f"captures/co08_crop{SUF}.png", "chrome": "none", "x": 0, "y": 0, "w": W}], "annotations": ann}
lay = f"layouts/content-organization_2-2-fold_hover{LAY}.json"
json.dump(themed(spec) if LIGHT else spec, open(lay, "w"), indent=1)
subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
print("#8 built", W, H)
