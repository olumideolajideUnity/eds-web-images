"""Fundamentals > Interactions: checked.png (#13) and higlighted.png (#15), light skin left / dark skin right like the originals.
checked: UI Toolkit checkboxes + toggle-button group (captures/int_checked_<l|d>.png, unity/int_states.cs) with real IMGUI window tab
strips laid over the empty rows: 'Checked tab' (captures/int_tabs_false[_l].png) and 'Checked + focused' (int_tabs_true[_l].png,
unity/int_tabs.cs). higlighted: the list view half (captures/int_highlighted_<l|d>.png) with the Project browser's folder tree
(captures/int_tree_<l|d>.png, unity/int_tree.cs, Assets row highlighted) cropped into the top box. Captures are 2.5 px/pt; halves 2x."""
import json, os, subprocess, sys
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
PPP = 2.5
def half(src, dx, w=800): return {"src": src, "chrome": "none", "x": dx, "y": 0, "w": w}
def tabs(src, dx, y):          # the tab strip only (top 26 pt of the specimen window), 240 pt wide at 2x
    return {"src": src, "chrome": "none", "x": 160 + dx, "y": y, "w": 480, "crop": [0, 0, 0, 190 - round(26 * PPP)]}
def tree(src, dx):             # folder-tree pane: 200 x 150 pt from the pane's top-left (x 0, y 50 pt)
    l, t, r, b = 0, round(50 * PPP), round(200 * PPP), round(200 * PPP)
    return {"src": src, "chrome": "none", "x": 200 + dx, "y": 40, "w": 400, "crop": [l, t, 725 - r, 690 - b]}
def run(n, out, h, wins, ann=()):
    spec = {"out": f"replacements/fundamentals/interactions/{out}", "canvas": [1600, h], "backdrop": [56, 56, 56], "windows": wins, "annotations": list(ann)}
    json.dump(spec, open(f"layouts/interactions_{n}.json", "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", f"layouts/interactions_{n}.json"], check=True, stdout=subprocess.DEVNULL); print("composed", out)
run("13", "checked.png", 520, [half("captures/int_checked_l.png", 0), half("captures/int_checked_d.png", 800),
    tabs("captures/int_tabs_false_l.png", 0, 287), tabs("captures/int_tabs_true_l.png", 0, 407),
    tabs("captures/int_tabs_false.png", 800, 287), tabs("captures/int_tabs_true.png", 800, 407)])
box = lambda dx, c: {"t": "rect", "x": 200 + dx, "y": 40, "w_": 400, "h": 300, "dash": False, "solid": True, "color": c, "w": 1}
run("15", "higlighted.png", 600, [half("captures/int_highlighted_l.png", 0), half("captures/int_highlighted_d.png", 800),
    tree("captures/int_tree_l.png", 0), tree("captures/int_tree_d.png", 800)], [box(0, [170, 170, 170]), box(800, [35, 35, 35])])
