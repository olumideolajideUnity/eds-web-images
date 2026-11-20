"""Errors & messaging: buttons-order.png. Unity 7's own IMGUI buttons (EFDS.EFDSMessageSpecimens mode 1, 333x215pt
dialog content area) placed in the original's dialog frame, with the original's guides and labels redrawn on top."""
import json, os, sys
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
from PIL import Image, ImageDraw, ImageFilter, ImageFont
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, themed
PAGE, GLOW, FRAME = ((226, 226, 226), (240, 222, 228), (160, 160, 160)) if LIGHT else ((41, 40, 40), (58, 41, 48), (25, 25, 25))
rects = json.loads(sys.argv[1] if len(sys.argv) > 1 else "[[24,30,93.025,20],[237.70627,30,71.49375,20],[144.60002,96,89.10625,20],[237.70627,96,71.49375,20],[24,162,93.025,20],[144.60002,162,89.10625,20],[237.70627,162,71.49375,20]]"); ppp, TAB = 2.5, 26   # button rects (pt) from unity/em_spec.cs MODE 1
OUT = "replacements/patterns/errors-and-messaging/buttons-order.png"

def build(S):                                    # S = 1 (1600x600) or 2 (@2x)
    W, H = 1600 * S, 600 * S
    img = Image.new("RGB", (W, H), PAGE)
    glow = Image.new("L", (W, H), 0)             # the original's soft warm glow behind the labels
    ImageDraw.Draw(glow).ellipse([700 * S, -40 * S, 1650 * S, 560 * S], fill=255)
    img.paste(GLOW, (0, 0), glow.filter(ImageFilter.GaussianBlur(170 * S)))
    cap = Image.open(f"captures/em_buttons{SUF}.png").convert("RGB")
    content = cap.crop((0, round(TAB * ppp), cap.width, cap.height))
    k = 2 * S                                    # canvas px per pt
    X0, Y0 = 137 * S, 85 * S
    img.paste(content.resize((round(333 * k), round(215 * k)), Image.LANCZOS), (X0, Y0))
    d = ImageDraw.Draw(img)
    d.rounded_rectangle([X0 - 1 * S, Y0 - 1 * S, X0 + round(333 * k), Y0 + round(215 * k)], 6 * S, outline=FRAME, width=2 * S)
    OR = (205, 80, 40) if LIGHT else (255, 110, 64); lw = max(1, round(1.5 * S))
    gl = X0 + min(r[0] for r in rects) * k - 4 * S                    # guides sit just outside the outer buttons
    gr = X0 + max(r[0] + r[2] for r in rects) * k + 4 * S
    for gx in (gl, gr): d.line([gx, Y0, gx, Y0 + 215 * k], fill=OR, width=lw)
    rows = sorted({r[1] for r in rects})
    for r in rects:                              # ticks from each guide to the edge button it measures
        cy = Y0 + (r[1] + r[3] / 2) * k
        if r[0] == min(x[0] for x in rects): d.line([gl, cy, X0 + r[0] * k, cy], fill=OR, width=lw)
        if r[0] + r[2] == max(x[0] + x[2] for x in rects): d.line([X0 + (r[0] + r[2]) * k, cy, gr, cy], fill=OR, width=lw)
    reg = ImageFont.truetype(_efds_font("segoeui.ttf"), 23 * S); bold = ImageFont.truetype(_efds_font("segoeuib.ttf"), 23 * S)
    G, O, Wt = ((90, 90, 90), (190, 70, 40), (9, 9, 9)) if LIGHT else ((194, 195, 196), (240, 160, 130), (255, 255, 255))
    labels = [[("2 buttons - ", reg, G), ("Destructive", bold, O), (" and ", reg, G), ("Primary", bold, Wt)],
              [("2 buttons - ", reg, G), ("Secondary", bold, Wt), (" and ", reg, G), ("Primary", bold, Wt)],
              [("3 buttons - ", reg, G), ("Destructive", bold, O), (" + ", reg, G), ("Secondary", bold, Wt), (" and ", reg, G), ("Primary", bold, Wt)]]
    for ry, segs in zip(rows, labels):
        x, cy = 882 * S, Y0 + (ry + 10) * k
        for s, f, c in segs: d.text((x, cy), s, font=f, fill=c, anchor="lm"); x += d.textlength(s, font=f)
    return img

import subprocess
build(2).save(f"captures/em_buttons_order{SUF}.png")
lay = f"layouts/errors-and-messaging_11{LAY}.json"
json.dump(themed({"out": OUT, "canvas": [1600, 600], "backdrop": list(PAGE),
           "windows": [{"src": f"captures/em_buttons_order{SUF}.png", "chrome": "none", "x": 0, "y": 0, "w": 1600}]}),
          open(lay, "w"), indent=1)
subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
print("built", OUT)
