"""Accessibility a11y-3 (Profiler colour-blind mode): one real Unity 7 Profiler window (CPU Usage + Rendering modules, a
Play mode recording of Atrio; unity/acc_prof.cs, ty_profiler.cs, ty_gameview.cs), captured twice from the same data:
colour-blind condition Default (captures/acc_prof_cb0.png) and Deuteranopia (captures/acc_prof_cb1.png). As in the
original, the chart's right part (from 43.5% across) comes from the colour-blind capture; legend and everything else are
the Default capture. Realtime GI (the original's module) can't be enabled in Unity 7, so CPU Usage is shown."""
import json, os, subprocess, sys
from PIL import Image
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, OUT, INK, themed
PG = [203, 203, 203] if LIGHT else [25, 26, 27]
P = 2.5
a = Image.open(f"captures/acc_prof_cb0{SUF}.png").convert("RGB"); b = Image.open(f"captures/acc_prof_cb1{SUF}.png").convert("RGB")
CH0, CH1, Y0, Y1 = 180.0, 686.0, 49.2, 259.6          # CPU chart area (pt), measured
split = CH0 + (CH1 - CH0) * 0.435
box = tuple(round(v * P) for v in (split, Y0, CH1, Y1))
a.paste(b.crop(box), box[:2]); a.save(f"captures/acc_prof_split{SUF}.png")
X, Y, K = 90, 152, 2
F = "fonts/Inter-Regular.ttf"
pill = lambda s, x: {"t": "pill", "s": s, "x": x, "y": 80, "h": 46, "pad": 16, "size": 24, "bg": PG, "outline": OUT,
                     "color": INK, "font": F}
spec = {"out": "replacements/fundamentals/accessibility/a11y-3.png", "canvas": [1600, 700], "backdrop": PG,
        "windows": [{"src": f"captures/acc_prof_split{SUF}.png", "chrome": "none", "x": X, "y": Y, "w": 697.2 * K}],
        "annotations": [pill("Default", 95), pill("Color Blind Mode", round(X + split * K))]}
lay = f"layouts/accessibility_3{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
print("split at", round(split, 1), "pt")
