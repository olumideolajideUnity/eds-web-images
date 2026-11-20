"""Accessibility a11y-5Y/5N (overlay background opacity): a real Unity 7 Scene view on Atrio with one real overlay (Tool Settings,
Panel layout, floating) over a busy part of the scene (captures/acc_ov_y.png; unity/acc_overlay.cs). Unity 7 overlays are always
opaque, so the 'don't' version is constructed like the original's mock: the same overlay blended at 55% over a capture of the
same view without it (captures/acc_ov_bg.png)."""
import json, os, subprocess, sys
from PIL import Image
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, themed
P = 2.5
y = Image.open(f"captures/acc_ov_y{SUF}.png").convert("RGB"); b = Image.open(f"captures/acc_ov_bg{SUF}.png").convert("RGB")
box = tuple(round(v * P) for v in (152.0, 138.0, 311.2, 181.2))          # overlay bounds (pt), measured
n = y.copy(); n.paste(Image.blend(b.crop(box), y.crop(box), 0.55), box[:2]); n.save(f"captures/acc_ov_n{SUF}.png")
CX, CY = 30, 40                                                          # crop origin (pt): 390 x 240pt of the viewport
for key, src in (("5Y", f"captures/acc_ov_y{SUF}.png"), ("5N", f"captures/acc_ov_n{SUF}.png")):
    spec = {"out": f"replacements/fundamentals/accessibility/a11y-{key}.png", "canvas": [780, 480], "backdrop": [203, 203, 203] if LIGHT else [25, 26, 27],
            "windows": [{"src": src, "chrome": "none", "x": -CX * 2, "y": -CY * 2, "w": 840}], "annotations": []}
    lay = f"layouts/accessibility_{key}{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
