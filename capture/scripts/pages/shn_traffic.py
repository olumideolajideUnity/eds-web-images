"""Selection history navigation: toolbar-layout.png. The real Unity 7 main toolbar (UnityEditor.Toolbar, 1536pt) across the
original's toolbar slot, with the original's pill and high-to-low traffic gradient redrawn."""
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
from PIL import Image, ImageDraw, ImageFont
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); import theme as T
PG = (226, 226, 226) if T.LIGHT else (56, 56, 56)
S = 2; W, H = 1600 * S, 600 * S
img = Image.new("RGB", (W, H), PG); d = ImageDraw.Draw(img)
tb = Image.open(f"captures/shn_toolbar{T.SUF}.png").convert("RGB")
tw = 1520 * S; th = round(tb.height * tw / tb.width)
img.paste(tb.resize((tw, th), Image.LANCZOS), (40 * S, 279 * S - th // 2))
F = lambda n, sz: ImageFont.truetype("C:/Windows/Fonts/" + n, sz * S)
G, R, Wt = (tuple(T.G), (205, 80, 40), tuple(T.INK)) if T.LIGHT else ((178, 255, 89), (255, 110, 64), (255, 255, 255))
# pill
f = F("segoeui.ttf", 24); t = "Drawing focus Left to Right"; tl = d.textlength(t, font=f)
d.rounded_rectangle([40 * S, 40 * S, 40 * S + tl + 74 * S, 84 * S], 22 * S, outline=tuple(T.OUT) if T.LIGHT else (81, 81, 81), width=2 * S, fill=PG)
d.text((57 * S, 62 * S), t, font=f, fill=Wt, anchor="lm")
ax = 57 * S + tl + 10 * S; ay = 62 * S
d.line([ax, ay, ax + 18 * S, ay], fill=Wt, width=2 * S); d.line([ax + 10 * S, ay - 8 * S, ax + 18 * S, ay, ax + 10 * S, ay + 8 * S], fill=Wt, width=2 * S, joint="curve")
# gradient line
x0, x1, y = 250 * S, 1350 * S, 359 * S
for x in range(x0, x1):
    k = (x - x0) / (x1 - x0); c = tuple(round(G[i] + (R[i] - G[i]) * k) for i in range(3))
    d.line([x, y - 1 * S, x, y + 1 * S], fill=c)
def circ(cx, cy, col, up):
    r = 18 * S; d.ellipse([cx - r, cy - r, cx + r, cy + r], outline=col, width=2 * S)
    s = -1 if up else 1; d.line([cx, cy - 9 * S * s, cx, cy + 9 * S * s], fill=col, width=2 * S)
    d.line([cx - 7 * S, cy + 2 * S * s, cx, cy + 9 * S * s, cx + 7 * S, cy + 2 * S * s], fill=col, width=2 * S, joint="curve")
circ(58 * S, 359 * S, G, True); circ(1392 * S, 359 * S, R, False)
fb = F("segoeui.ttf", 25)
for (x, col, a, b) in ((96, G, "High Traffic", "Actions"), (1430, R, "Low Traffic", "Actions")):
    d.text((x * S, 357 * S), a, font=fb, fill=col, anchor="ls"); d.text((x * S, 386 * S), b, font=fb, fill=col, anchor="ls")
img.save(f"captures/shn_traffic{T.SUF}.png")
spec = {"out": "replacements/patterns/selection-history-navigation/toolbar-layout.png", "canvas": [1600, 600], "backdrop": list(PG),
        "windows": [{"src": f"captures/shn_traffic{T.SUF}.png", "chrome": "none", "x": 0, "y": 0, "w": 1600}]}
lay = f"layouts/selection-history-navigation_02{T.LAY}.json"; json.dump(T.themed(spec), open(lay, "w"), indent=1)
subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
