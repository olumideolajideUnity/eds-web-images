"""Derive layouts/<name>_light.json from a dark layout and compose it: every capture src gets its light twin
(captures/x_d.png -> x_l.png, otherwise x.png -> x_l.png), dark annotation colours map to the light palette (scripts/pages/theme.py),
and the spec gets theme 'light' (compose.py then writes the '--light' outputs with light window frames).
Usage: python scripts/lightlayout.py [--dy N] layouts/help-box_03.json [...]   (prints layouts whose light captures are missing)
--dy N moves every annotation down N canvas px (light-skin Inspectors sit 4pt lower: their GameObject/asset header is taller)."""
import json, os, re, subprocess, sys
ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__))); os.chdir(ROOT)
CMAP = {(178, 255, 89): [77, 140, 80], (48, 60, 36): [182, 191, 182], (86, 87, 88): [160, 160, 160], (50, 50, 50): [215, 215, 215],
        (56, 56, 56): [215, 215, 215], (255, 255, 255): [9, 9, 9], (230, 230, 230): [40, 40, 40], (40, 40, 40): [194, 194, 194],
        (255, 110, 64): [205, 80, 40], (74, 86, 61): [205, 225, 190], (86, 64, 57): [240, 216, 206], (26, 26, 26): [140, 140, 140], (253, 134, 120): [200, 70, 60], (59, 41, 39): [244, 218, 214], (238, 238, 238): [9, 9, 9], (90, 90, 90): [200, 200, 200]}
def light_src(src):
    for a, b in (("_d.png", "_l.png"), (".png", "_l.png")):
        if src.endswith(a): return src[: -len(a)] + b
def recolor(v):
    if isinstance(v, list) and len(v) == 3 and all(isinstance(x, (int, float)) for x in v): return CMAP.get(tuple(v), v)
    if isinstance(v, dict): return {k: recolor(x) for k, x in v.items()}
    if isinstance(v, list): return [recolor(x) for x in v]
    return v
missing = []; args = sys.argv[1:]; DY = 0
if args and args[0] == "--dy": DY = float(args[1]); args = args[2:]
for lay in args:
    spec = json.load(open(lay)); spec = recolor(spec); spec["theme"] = "light"; ok = True
    if spec.get("backdrop") and max(spec["backdrop"]) < 80: spec["backdrop"] = [226, 226, 226]   # any dark page -> the light page grey (as the light hero)
    for w in spec.get("windows", []):
        w["src"] = light_src(w["src"])
        if not os.path.exists(w["src"]): ok = False; missing.append((lay, w["src"]))
    for a in spec.get("annotations", []):
        if a.get("t") == "pill":   # compose.py's pill defaults are dark (bg 56, outline 86): give light ones
            a.setdefault("bg", [226, 226, 226]); a.setdefault("outline", [160, 160, 160])
        for k in ("y", "y1", "y2"):
            if DY and isinstance(a.get(k), (int, float)): a[k] += DY
        if a.get("t") == "image" and a.get("src", "").startswith("captures/") and os.path.exists(light_src(a["src"])): a["src"] = light_src(a["src"])
    out = lay.replace(".json", "_light.json"); json.dump(spec, open(out, "w"), indent=1)
    if ok: subprocess.run([sys.executable, "scripts/compose.py", out], check=True, stdout=subprocess.DEVNULL); print("composed", out)
for lay, src in missing: print("MISSING", lay, src)
