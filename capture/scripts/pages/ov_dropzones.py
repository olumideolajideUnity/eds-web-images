"""Overlays: drop zones 1 and 2 (also reused by Fundamentals > Interactions). Real mid-drag states of a Scene view overlay
(captures/ov_dz_<corner|edge|toolbar|mid>[_l].png from unity/ov_drag.cs, TO = 16,64 / 10,175 / 430,37 / 330,160), cropped to
panels with numbered badges. 1: target (corner hover) and ghost (mid drag), 360pt panels; 2: corner, edge and toolbar zones, 250pt."""
import json, os, subprocess, sys
from PIL import Image
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from ov_callouts import badge; from theme import LIGHT, SUF, LAY, themed
p = 2.5; BD = [203, 203, 203] if LIGHT else [25, 25, 25]
def crop(name, w_pt, out):
    im = Image.open(f"captures/ov_dz_{name}{SUF}.png").convert("RGB"); im.crop((0, 0, round(w_pt * p), im.height)).save(out); return out
def run(n, out, canvas, wins, ann):
    spec = {"out": f"replacements/patterns/overlays/{out}", "canvas": canvas, "backdrop": BD, "windows": wins, "annotations": ann}
    lay = f"layouts/overlays_{n}{LAY}.json"; json.dump(themed(spec) if LIGHT else spec, open(lay, "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL); print("composed", lay)
wins, ann = [], []
for i, n in enumerate(("corner", "mid")):
    src = crop(n, 360, f"captures/ov_dz1_{i}{SUF}.png"); x = 60 + i * 780
    wins.append({"src": src, "chrome": "none", "x": x, "y": 30, "w": 720, "h": 540}); ann += badge(x - 30, 52, i + 1)
run("06", "overlays-drop zones 1.png", [1600, 600], wins, ann)
wins, ann = [], []
for i, n in enumerate(("corner", "edge", "toolbar")):
    src = crop(n, 250, f"captures/ov_dz2_{i}{SUF}.png"); x = 52 + i * 520
    wins.append({"src": src, "chrome": "none", "x": x, "y": 40, "w": 470, "h": 560}); ann += badge(x - 26, 62, i + 1)
run("07", "overlays-drop zones 2.png", [1604, 600], wins, ann)
