"""Components > Slider: slider-htut-Y.png, slider-htut-N.png and slider-layout.png.
Like the originals these are reduced Inspectors: a never-saved 'Main Camera' object (unity/sl_htut.cs) whose real components are
hidden, carrying one specimen component: Camera with only Field of View (MODE Y, captures/sl_htutY[_l].png) or Transform with
Position drawn as three sliders, the 'don't' example (MODE N, captures/sl_htutN[_l].png). Captured with capdock InspectorWindow at
357 x 230 pt (2.5 px/pt). Post-processing, as in the constructed originals: Unity's ' (Script)' title suffix and the specimen
Transform's enable checkbox (the real Transform has none) are painted out, the Add Component button below the component is
cleared, and the tab + GameObject header are faded. slider-layout splices the tab strip onto the Camera component and draws
the four guides (label start, slider start, slider end, field end) with numbered badges in the original's style."""
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
from PIL import Image
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, themed
TAB, SEP = 66, 242                                  # capture px: tab strip bottom, separator above the component header

def rows_below(im, y0, test):
    return [y for y in range(y0, im.height) if test(im.getpixel((5, y)))]

def clean(name, checkbox):
    im = Image.open(f"captures/{name}{SUF}.png").convert("RGB"); W = im.width
    hb = im.getpixel((W // 2, SEP + 30)); bg = im.getpixel((W // 2, im.height - 4))
    # ' (Script)': the last word group of the header title (dark text columns between the icon and the checkbox)
    ys = range(SEP + 8, SEP + 52)
    ink = [x for x in range(90, W - 70) if any(sum(im.getpixel((x, y))) < 3 * 120 if LIGHT else sum(im.getpixel((x, y))) > 3 * 150 for y in ys)]
    groups, start = [], ink[0]
    for a, b in zip(ink, ink[1:] + [10 ** 6]):
        if b - a > 9: groups.append((start, a)); start = b
    x0 = groups[-1][0] - 2                          # '(Script)' begins after the word gap
    im.paste(hb, (x0, SEP + 6, W - 70, SEP + 54))
    if not checkbox: im.paste(hb, (W - 70, SEP + 6, W, SEP + 54))
    # clear below the component's bottom separator (Add Component button)
    seps = [y for y in range(SEP + 60, im.height) if abs(sum(im.getpixel((5, y))) - sum(bg)) > 30]
    im.paste(bg, (0, seps[-1] + 1 if seps else SEP + 140, W, im.height))
    return im, hb, bg, (seps[-1] if seps else None)

def fade(im, y1, col, a=0.7):
    top = im.crop((0, 0, im.width, y1)); im.paste(Image.blend(top, Image.new("RGB", top.size, col), a), (0, 0))

def run(lay, spec):
    path = f"layouts/{lay}{LAY}.json"; json.dump(themed(spec), open(path, "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", path], check=True, stdout=subprocess.DEVNULL); print("composed", path)

PAGE = [50, 51, 52]                                  # the originals' backdrop
for n, mode, checkbox in (("04", "Y", True), ("05", "N", False)):
    im, hb, bg, _ = clean(f"sl_htut{mode}", checkbox)
    fade(im, SEP, bg)
    k = 714 / im.width; im = im.crop((0, 0, im.width, round(440 / k)))
    im.save(f"captures/sl_htut{mode}_post{SUF}.png")
    run(f"slider_{n}", {"out": f"replacements/components/slider/slider-htut-{mode}.png", "canvas": [780, 480], "backdrop": PAGE,
                        "windows": [{"src": f"captures/sl_htut{mode}_post{SUF}.png", "chrome": "none", "x": 32, "y": 40, "w": 714}], "annotations": []})

# slider-layout: tab strip + Camera component (no GameObject header), guides from the capture's own geometry
im, hb, bg, sep2 = clean("sl_htutY", True)
W = im.width; tab = im.crop((0, 0, W, TAB)); fade(tab, TAB, bg)
body = im.crop((0, SEP, W, im.height))
lay = Image.new("RGB", (W, TAB + body.height), bg); lay.paste(tab, (0, 0)); lay.paste(body, (0, TAB))
src = Image.open(f"captures/sl_htutY{SUF}.png").convert("RGB")
# geometry from pixels: label ink start, slider track start/end, field right edge on the row centre line
yc = SEP + 90                                                          # Field of View row centre (capture px)
line = [src.getpixel((x, yc)) for x in range(W)]
dark = lambda c: sum(c) < 3 * 140 if LIGHT else sum(c) > 3 * 150
lab = next(x for x in range(10, W // 3) if any(dark(src.getpixel((x, y))) for y in range(yc - 15, yc + 15)))
trk = [x for x in range(W // 3, W - 138) if abs(sum(line[x]) - sum(bg)) > 24]
t0, t1 = trk[0], trk[-1]
fld = [x for x in range(W - 138, W) if abs(sum(line[x]) - sum(bg)) > 24]; f1 = fld[-1]
lay.save(f"captures/sl_layout{SUF}.png")
X, Y, S = 283, 165, 1037 / W
gx = [round(X + v * S) for v in (lab, t0, t1, f1)]
G2, FILL2, INK2 = ([46, 125, 50], [219, 231, 220], [46, 125, 50]) if LIGHT else ([178, 255, 89], [48, 60, 36], [178, 255, 89])
PAGE2 = [250, 250, 250] if LIGHT else [40, 40, 40]
def badge(cx, cy, n): return [{"t": "dot", "x": cx, "y": cy, "r": 25, "color": G2}, {"t": "dot", "x": cx, "y": cy, "r": 23, "color": FILL2},
                             {"t": "text", "s": str(n), "x": cx, "y": cy, "anchor": "mm", "size": 26, "color": INK2, "font": _efds_font("segoeui.ttf")}]
ann, B = [], 108
ann += badge(gx[0] - 43, B, 1) + [{"t": "line", "x1": gx[0] - 26, "y1": B + 18, "x2": gx[0], "y2": 150, "color": G2, "w": 2},
                                   {"t": "line", "x1": gx[0], "y1": 150, "x2": gx[0], "y2": 606, "color": G2, "w": 2}]
ann += badge(gx[1], B, 2) + [{"t": "vdash", "x": gx[1], "y1": B + 25, "y2": 606, "color": G2, "w": 2}]
ann += badge(gx[2], B, 3) + [{"t": "line", "x1": gx[2], "y1": B + 25, "x2": gx[2], "y2": 606, "color": G2, "w": 2}]
ann += badge(gx[3] + 43, B, 4) + [{"t": "line", "x1": gx[3] + 26, "y1": B + 18, "x2": gx[3], "y2": 150, "color": G2, "w": 2},
                                   {"t": "line", "x1": gx[3], "y1": 150, "x2": gx[3], "y2": 606, "color": G2, "w": 2}]
ry = round(Y + (TAB + (yc - SEP)) * S)
ann += [{"t": "text", "s": "\u2190", "x": gx[1] - 32, "y": ry, "anchor": "mm", "size": 30, "color": G2, "font": _efds_font("segoeui.ttf")}]
run("slider_10", {"out": "replacements/components/slider/slider-layout.png", "canvas": [1604, 606], "backdrop": PAGE2,
                  "windows": [{"src": f"captures/sl_layout{SUF}.png", "chrome": "none", "x": X, "y": Y, "w": 1037}], "annotations": ann})
print("guides", gx, "row y", ry)
