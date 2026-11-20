"""Components > Label styles (label-styles.png) at 2x, dark: UI Toolkit specimens (captures/lb_styles.png, unity/lb_styles.cs) plus real
Editor pieces placed like the original: the Light component header (captures/lb_light.png), the bold 'Assets' root row of the Project
folder tree (captures/lb_tree.png), and the main window status bar showing an error, a warning and a log message
(captures/lb_sb_<type>.png via unity/em_statusbar.cs), with the original's pills and dividers."""
import json, os, subprocess, sys
from PIL import Image
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
P = 2.5; OUT = [86, 87, 88]; FB = "fonts/Inter-SemiBold.ttf"
def piece(src, box, x, y):
    im = Image.open(src); l, t, r, b = box
    return {"src": src, "chrome": "none", "x": x, "y": y, "w": (r - l) * 2, "crop": [round(l * P), round(t * P), im.width - round(r * P), im.height - round(b * P)]}
pill = lambda s, x, y: {"t": "pill", "s": s, "x": x, "y": y, "h": 44, "pad": 16, "size": 22, "bg": [50, 50, 50], "outline": OUT, "color": [255, 255, 255], "font": FB}
L = lambda a, b, c, d: {"t": "line", "x1": a, "y1": b, "x2": c, "y2": d, "color": OUT, "w": 2}
wins = [{"src": "captures/lb_styles.png", "chrome": "none", "x": 0, "y": 0, "w": 1600},
        piece("captures/lb_light.png", (0, 117.0, 200, 142.6), 604, 286),
        piece("captures/lb_tree.png", (0, 80, 120, 96), 610, 370)]
wins += [piece(f"captures/lb_sb_{t}.png", (0, 0, 160, 20), 40, 694 + 40 * k) for k, t in enumerate(("Error", "Warning", "Log"))]
ann = [L(532, 0, 532, 460), L(1064, 0, 1064, 460), L(0, 460, 1600, 460),
       pill("Regular", 40, 62), pill("Bold", 572, 62), pill("Focused / highlighted", 1104, 62), pill("Labels with semantic colors", 40, 522)]
spec = {"out": "replacements/components/label/label-styles.png", "canvas": [1600, 880], "backdrop": [50, 50, 50], "windows": wins, "annotations": ann}
json.dump(spec, open("layouts/label_04.json", "w"), indent=1)
subprocess.run([sys.executable, "scripts/compose.py", "layouts/label_04.json"], check=True, stdout=subprocess.DEVNULL)
