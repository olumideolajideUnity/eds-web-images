"""Color palette > Accent Borders: the original's diagram (pills, brackets, layer diamonds) with its two controls replaced by
real Unity 7 UI Toolkit controls (captures/cp_accent_tf.png / cp_accent_dd.png from unity/cp_spec.cs WHICH=accent_tf /
accent_dd: a TextField and a DropdownField panel-scaled 1.48x, rendered on the diagram's #383838 so the crops blend)."""
import json, os, subprocess, sys
from PIL import Image, ImageDraw
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, G, INK, themed
CW, CH = 1050, 250
tf = (342, 91, 708, 158)            # control bounds in its capture (px), measured
dd = (328, 89, 722, 161)
M = 10
def win(src, b, x, y):                 # paste the control (plus M px of matching backdrop) with its box at (x, y)
    l, t, r, bt = b[0] - M, b[1] - M, b[2] + M, b[3] + M
    return {"src": src, "chrome": "none", "x": x - M, "y": y - M, "w": r - l, "crop": [l, t, CW - r, CH - bt]}
ann = []
if not LIGHT: bg = {"src": "original/cp/Accent Borders.png", "chrome": "none", "x": 0, "y": 0, "w": 1600}
else:   # the original diagram exists only in dark: redraw it on the light page (same geometry, measured from the original at 1x)
    PG = (226, 226, 226); S = 2; im = Image.new("RGB", (1600 * S, 600 * S), PG); d = ImageDraw.Draw(im)
    def dia(cx, cy, hw, hh, **kw): d.polygon([((cx) * S, (cy - hh) * S), ((cx + hw) * S, cy * S), (cx * S, (cy + hh) * S), ((cx - hw) * S, cy * S)], **kw)
    def chev(cx, cy, up):
        for k in (0, 10):
            y = cy + k; d.line([((cx - 7) * S, (y + (6 if up else 0)) * S), (cx * S, (y + (0 if up else 6)) * S), ((cx + 7) * S, (y + (6 if up else 0)) * S)], fill=tuple(G), width=3 * S, joint="curve")
    for cx in (400, 1200): dia(cx, 468, 146, 78, outline=(160, 160, 160), width=2)
    dia(400, 468, 72, 38, fill=(150, 150, 150)); dia(400, 473, 66, 33, fill=(200, 200, 200)); chev(400, 452, False)      # 1: sunken (dark top lip)
    dia(1200, 468, 72, 38, fill=(165, 165, 165)); dia(1200, 455, 72, 38, fill=(246, 246, 246), outline=(205, 205, 205), width=2); chev(1200, 440, True)   # 2: raised over its shadow
    im.save("captures/cp_accent_diagram_l.png")
    bg = {"src": "captures/cp_accent_diagram_l.png", "chrome": "none", "x": 0, "y": 0, "w": 1600}
    pl = lambda t, x: {"t": "pill", "s": t, "x": x, "y": 62, "h": 44, "pad": 16, "size": 22, "bg": list(PG), "outline": [160, 160, 160], "color": INK, "font": "fonts/Inter-Regular.ttf"}
    L = lambda x1, y1, x2, y2: {"t": "line", "x1": x1, "y1": y1, "x2": x2, "y2": y2, "color": G, "w": 2}
    ann = [pl("1: Inset Border", 40), pl("2: Outset Border", 840), L(401, 201, 401, 166), L(401, 166, 800, 166), L(800, 166, 800, 205),
           L(801, 287, 801, 325), L(801, 325, 1200, 325), L(1200, 325, 1200, 295),
           {"t": "text", "s": "Darker Border", "x": 800, "y": 246, "size": 22, "color": G, "anchor": "mm", "font": "fonts/Inter-SemiBold.ttf"}]
spec = {"out": "replacements/fundamentals/color-palette/Accent Borders.png", "canvas": [1600, 600], "backdrop": [226, 226, 226] if LIGHT else [56, 56, 56],
        "windows": [bg, win(f"captures/cp_accent_tf{SUF}.png", tf, 217, 212), win(f"captures/cp_accent_dd{SUF}.png", dd, 1003, 211)],
        "annotations": ann}
lay = f"layouts/color-palette_02{LAY}.json"; json.dump(themed(spec) if LIGHT else spec, open(lay, "w"), indent=1)
subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
