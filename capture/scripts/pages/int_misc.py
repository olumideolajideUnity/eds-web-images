"""Fundamentals > Interactions: rounded-corners.png, containers.png and assigning.png from real Unity 7 UI Toolkit controls
(unity/int_misc.cs; captures/int_rounded.png, int_containers.png, int_assign0/1/2.png), at 2x with the original's labels.
Unity 7's control corner radius is 4px (resolvedStyle.borderTopLeftRadius of Button, IntegerField input and ObjectField: 4)."""
import os as _os; _EFDS_TMP = _os.environ.get("EFDS_TMP", "C:/tmp")
import json, os, subprocess, sys
from PIL import ImageFont
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from ov_callouts import G
F = "fonts/Inter-Regular.ttf"; FB = "fonts/Inter-SemiBold.ttf"; WHITE, OUT = [255, 255, 255], [86, 87, 88]
def pill(s, x, y): return {"t": "pill", "s": s, "x": x, "y": y, "h": 44, "pad": 16, "size": 24, "bg": [50, 50, 50], "outline": OUT, "color": WHITE, "font": FB}
def run(n, spec):
    json.dump(spec, open(f"layouts/interactions_{n}.json", "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", f"layouts/interactions_{n}.json"], check=True, stdout=subprocess.DEVNULL)
# rounded corners: brackets 6px outside each end of the button (measured 121.2..276.4 x 80.4..102.4pt), '4 px round' labels
r = json.load(open(_EFDS_TMP + "/int_rounded.json"))["button"]; x0, y0, x1, y1 = (v * 2 for v in (r[0], r[1], r[0] + r[2], r[1] + r[3]))
L = lambda a, b, c, d: {"t": "line", "x1": a, "y1": b, "x2": c, "y2": d, "color": G, "w": 2}
ann = [L(x0 - 6, y0 - 4, x0 - 6, y1 + 4), L(x0 - 6, y0 - 4, x0, y0 - 4), L(x0 - 6, y1 + 4, x0, y1 + 4),
       L(x1 + 6, y0 - 4, x1 + 6, y1 + 4), L(x1, y0 - 4, x1 + 6, y0 - 4), L(x1, y1 + 4, x1 + 6, y1 + 4),
       {"t": "text", "s": "4 px\nround", "x": x0 - 14, "y": (y0 + y1) / 2 - 12, "size": 22, "color": G, "font": F, "anchor": "rm"},
       {"t": "text", "s": "4 px\nround", "x": x1 + 14, "y": (y0 + y1) / 2 - 12, "size": 22, "color": G, "font": F, "anchor": "lm"}]
run("06", {"out": "replacements/fundamentals/interactions/rounded-corners.png", "canvas": [780, 360], "backdrop": [50, 50, 50],
           "windows": [{"src": "captures/int_rounded.png", "chrome": "none", "x": 0, "y": 0, "w": 780}], "annotations": ann})
# containers
run("07", {"out": "replacements/fundamentals/interactions/containers.png", "canvas": [780, 508], "backdrop": [50, 50, 50],
           "windows": [{"src": "captures/int_containers.png", "chrome": "none", "x": 0, "y": 0, "w": 780}],
           "annotations": [pill("Interactive content", 34, 62), pill("Static content", 424, 62)]})
# assigning: three 533px panels split by 2px dividers, pills top-left
wins = [{"src": f"captures/int_assign{i}.png", "chrome": "none", "x": i * 534, "y": 0, "w": 532} for i in range(3)]
ann = [{"t": "line", "x1": 533, "y1": 0, "x2": 533, "y2": 600, "color": OUT, "w": 2}, {"t": "line", "x1": 1067, "y1": 0, "x2": 1067, "y2": 600, "color": OUT, "w": 2},
       pill("Idle", 20, 62), pill("Material dragged", 554, 62), pill("Material assigned", 1088, 62)]
run("17", {"out": "replacements/fundamentals/interactions/assigning.png", "canvas": [1600, 600], "backdrop": [50, 50, 50], "windows": wins, "annotations": ann})
