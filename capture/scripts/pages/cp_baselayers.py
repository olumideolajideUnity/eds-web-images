"""Color palette > Base Layers: the original's exploded isometric view of the Editor's three base layers, rebuilt from real
Unity 7 captures: Base 1 = main window with its app toolbar (captures/cp_apptb.png, Toolbar view), Base 2 = a Hierarchy
window (captures/cp_hier.png, unity/cp_hier.cs), Base 3 = the Hierarchy's toolbar lifted off the window. Same projection as
the original (measured: 1.62 px/pt along both axes, baselines rising 30.5 deg, rows falling 31 deg)."""
import json, os, subprocess, sys
def _efds_font(n):
    # Windows fonts when present; elsewhere the bundled Inter (capture/fonts) or the system monospace font
    import os as _o
    w = "C:/Windows/Fonts/" + n
    if _o.path.exists(w): return w
    d = _o.path.dirname(_o.path.abspath(__file__))
    while not _o.path.isdir(_o.path.join(d, "fonts")) and _o.path.dirname(d) != d: d = _o.path.dirname(d)
    mono = [p for p in ("/System/Library/Fonts/Menlo.ttc", "/usr/share/fonts/truetype/dejavu/DejaVuSansMono-Bold.ttf") if _o.path.exists(p)]
    if n.startswith("consola") and mono: return mono[0]
    return _o.path.join(d, "fonts", "Inter-SemiBold.ttf" if n in ("segoeuib.ttf", "seguisb.ttf", "consolab.ttf") else "Inter-Regular.ttf")
from PIL import Image, ImageDraw, ImageFilter
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, G, FILL as F, themed
PPP, S = 2.5, 2                                   # capture px per pt; output scale (rendered at 2x of the 1600x600 canvas)
A, B = (1.4, -0.825), (1.375, 0.825)              # 1x canvas px per pt along u (baseline) and v (rows)
O = (545.8, 355.6)                                # 1x canvas position of the main window's top-left corner (flat 0,0)
BG = (226, 226, 226) if LIGHT else (50, 51, 52)
def P(u, v, lift=0): return (O[0] + u * A[0] + v * B[0], O[1] + u * A[1] + v * B[1] - lift)
def place(out, img, u0, v0, lift, shadow=0):
    """Project a flat layer (capture px at PPP) whose top-left sits at flat (u0, v0) pt, raised by lift (1x px)."""
    det = (A[0] * B[1] - B[0] * A[1]) * S * S
    ox, oy = (v * S for v in P(u0, v0, lift))
    # inverse of [[A0 B0],[A1 B1]]*S, then pt -> capture px
    a, b = B[1] * S / det * PPP, -B[0] * S / det * PPP
    d, e = -A[1] * S / det * PPP, A[0] * S / det * PPP
    w, h = img.size[0] / PPP, img.size[1] / PPP
    quad = [tuple(c * S for c in P(u0 + du, v0 + dv, lift)) for du, dv in ((0, 0), (w, 0), (w, h), (0, h))]
    if shadow:
        sh = Image.new("L", out.size, 0); ImageDraw.Draw(sh).polygon([(x, y + shadow * S) for x, y in quad], fill=150)
        out.paste((0, 0, 0), (0, 0), sh.filter(ImageFilter.GaussianBlur(14 * S)))
    rgba = img.convert("RGBA")
    proj = rgba.transform(out.size, Image.AFFINE, (a, b, -(a * ox + b * oy), d, e, -(d * ox + e * oy)), Image.BICUBIC)
    out.alpha_composite(proj)
    return quad

hier = Image.open(f"captures/cp_hier{SUF}.png").convert("RGB")
TB0, TB1 = round(26 * PPP), round(49.2 * PPP)      # Hierarchy toolbar band (pt 26 - 49.2)
tool = hier.crop((0, TB0, hier.width, TB1))
win = hier.copy(); ImageDraw.Draw(win).rectangle([0, TB0, win.width, TB1 - 1], fill=(190, 190, 190) if LIGHT else (40, 40, 40))   # the toolbar's slot in Base 2
app = Image.open(f"captures/cp_apptb{SUF}.png").convert("RGB")
L1W, L1H = 640, 460
main = Image.new("RGB", (round(L1W * PPP), round(L1H * PPP)), (165, 165, 165) if LIGHT else (20, 20, 20))
main.paste(app.crop((0, 0, min(app.width, main.width), app.height)), (0, 0))
m = Image.new("L", main.size, 0); ImageDraw.Draw(m).rounded_rectangle([0, 0, main.width + 40, main.height + 40], round(8 * PPP), fill=255)
main = Image.composite(main.convert("RGBA"), Image.new("RGBA", main.size, (0, 0, 0, 0)), m)

out = Image.new("RGBA", (1600 * S, 600 * S), BG + (255,))
HX, HY, L1D, L3L = 8, 40, -120, 150                # Hierarchy's flat offset in the main window; Base 1 drop; Base 3 lift
place(out, main, 0, 0, L1D)
place(out, win, HX, HY, 0, shadow=10)
place(out, tool, HX, HY + 26, L3L, shadow=12)
out.convert("RGB").save(f"captures/cp_baselayers_iso{SUF}.png")

# callouts like the original: green pills on the left, leader lines to a dot on each layer, dashed vertical joining them
x3, y3 = P(HX, 70, L3L); _, y2 = P(HX, 70, 0)
v1 = (x3 - O[0]) / B[0]; _, y1 = P(0, v1, L1D)
ann = [{"t": "vdash", "x": x3, "y1": y3, "y2": y1, "color": [120, 120, 120] if LIGHT else [150, 150, 150], "w": 1}]
for n, y in ((3, y3), (2, y2), (1, y1)):
    ann += [{"t": "line", "x1": 268, "y1": y, "x2": x3, "y2": y, "color": G, "w": 2}, {"t": "dot", "x": x3, "y": y, "r": 5, "color": G},
            {"t": "pill", "s": "Base %d" % n, "x": 160, "y": y, "h": 48, "pad": 18, "size": 26, "bg": F, "outline": G, "color": G,
             "font": _efds_font("seguisb.ttf")}]
spec = {"out": "replacements/fundamentals/color-palette/Base Layers.png", "canvas": [1600, 600], "backdrop": list(BG),
        "windows": [{"src": f"captures/cp_baselayers_iso{SUF}.png", "chrome": "none", "x": 0, "y": 0, "w": 1600}], "annotations": ann}
lay = f"layouts/color-palette_03{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
print("dots", round(x3), round(y3), round(y2), round(y1))
