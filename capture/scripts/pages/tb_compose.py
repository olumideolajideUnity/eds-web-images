"""Components > Tab: compose the dark images from unity/tb_spec.cs (captures/tb_<which>.png) at 2x with the original's pills.
#1 is the same Search window image as Patterns > Content organization #4 (layouts/content-organization_04.json)."""
import json, os, subprocess, sys
from PIL import ImageFont
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, OUT, BG, INK, themed
F, FB = "fonts/Inter-Regular.ttf", "fonts/Inter-SemiBold.ttf"
GREEN = ((205, 225, 190), (77, 140, 80), (30, 60, 30)) if LIGHT else ((74, 86, 61), (178, 255, 89), (232, 234, 231))
ORANGE = ((240, 216, 206), (205, 90, 50), (80, 35, 20)) if LIGHT else ((86, 64, 57), (255, 110, 64), (234, 231, 231))
OUTDIR = "replacements/components/tab"; os.makedirs(OUTDIR, exist_ok=True)
PF = ImageFont.truetype(FB, 22)
def pill(s, x, y, bg=BG, outline=OUT, color=None):
    color = color or INK
    return {"t": "pill", "s": s, "x": x, "y": y, "h": 40, "pad": 14, "size": 22, "bg": list(bg), "outline": list(outline), "color": list(color), "font": FB}, x + PF.getlength(s) + 28
def run(n, out, wins, ann, canvas=(1600, 600)):
    spec = {"out": f"{OUTDIR}/{out}", "canvas": list(canvas), "backdrop": BG, "windows": wins, "annotations": ann}
    lay = f"layouts/tab_{n}{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
full = lambda src, w=1600: {"src": src.replace(".png", SUF + ".png"), "chrome": "none", "x": 0, "y": 0, "w": w}

# 01 Search window tabs: same image as Content organization #4 (light: content-organization_04_light.json, scripts/lightlayout.py)
spec = json.load(open(f"layouts/content-organization_04{LAY}.json")); spec["out"] = f"{OUTDIR}/tabuitk-overview.png"
json.dump(spec, open(f"layouts/tab_01{LAY}.json", "w"), indent=1); subprocess.run([sys.executable, "scripts/compose.py", f"layouts/tab_01{LAY}.json"], check=True, stdout=subprocess.DEVNULL)

# 02 when to use: TabView with a multi-column list; the multi-column tree in its own window (tab strip from the docked host)
p1, _ = pill("Tab view", 120, 100); p2, _ = pill("Multi-column tree view", 920, 100)
run("02", "tabuitk-wtu.png", [full("captures/tb_wtu.png"), {"src": f"captures/tb_tree{SUF}.png", "chrome": "none", "x": 920, "y": 152, "w": 560}], [p1, p2])

# 03 secondary tabs (TabView, "new") and IMGUI toolbar toggle buttons ("imgui only")
a, ax = pill("Secondary Tabs", 80, 102); b, _ = pill("new", ax + 12, 102, *GREEN)
c, cx = pill("Tabs constructed with Toggle Buttons in a Toolbar", 80, 404); d, _ = pill("imgui only", cx + 12, 404, *ORANGE)
run("03", "Tabview Toolbars.png", [full("captures/tb_toolbars.png")], [a, b, c, d])

# 04 reordering a tab: real mid-drag state (window mouse events, no pointer)
p, _ = pill("Reordering a tab", 80, 74)
run("04", "tabuitk-reordering.png", [full("captures/tb_reorder_drag.png")], [p], (1600, 460))
