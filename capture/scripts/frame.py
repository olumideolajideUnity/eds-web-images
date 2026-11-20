"""Frame a raw Editor capture in the foundations.unity.com screenshot style."""
import sys
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

src, title, out = sys.argv[1], sys.argv[2], sys.argv[3]
S = 2                               # render at 2x, export 1x + 2x
W, H = 1600 * S, 724 * S
bg = Image.new("RGB", (W, H))
d = ImageDraw.Draw(bg)
for y in range(H):                  # site backdrop: #292929 -> #242424
    t = y / H
    c = round(41 - 5 * t)
    d.line([(0, y), (W, y)], fill=(c, c, c))

cap = Image.open(src).convert("RGB")
content_h = 600 * S
content_w = round(cap.width * content_h / cap.height)
cap = cap.resize((content_w, content_h), Image.LANCZOS)
bar = 38 * S
ww, wh = content_w, content_h + bar
x0, y0 = (W - ww) // 2, (H - wh) // 2
r = 10 * S

# soft shadow
sh = Image.new("L", (W, H), 0)
ImageDraw.Draw(sh).rounded_rectangle([x0, y0 + 8 * S, x0 + ww, y0 + wh + 8 * S], r, fill=150)
sh = sh.filter(ImageFilter.GaussianBlur(18 * S))
bg.paste((0, 0, 0), (0, 0), sh)

win = Image.new("RGB", (ww, wh), (56, 56, 56))
win.paste(cap, (0, bar))
wd = ImageDraw.Draw(win)
for i, col in enumerate([(255, 95, 87), (254, 188, 46), (40, 200, 64)]):
    cx, cy = 20 * S + i * 20 * S, bar // 2
    wd.ellipse([cx - 6 * S, cy - 6 * S, cx + 6 * S, cy + 6 * S], fill=col)
try:
    font = ImageFont.truetype(_efds_font("segoeuib.ttf"), 15 * S)
except OSError:
    font = ImageFont.load_default()
tw = wd.textlength(title, font=font)
wd.text(((ww - tw) / 2, bar / 2), title, font=font, fill=(196, 196, 196), anchor="lm")
wd.line([(0, bar - 1), (ww, bar - 1)], fill=(35, 35, 35), width=S)

mask = Image.new("L", (ww, wh), 0)
ImageDraw.Draw(mask).rounded_rectangle([0, 0, ww - 1, wh - 1], r, fill=255)
bg.paste(win, (x0, y0), mask)
ImageDraw.Draw(bg).rounded_rectangle([x0, y0, x0 + ww - 1, y0 + wh - 1], r, outline=(90, 90, 90), width=S)

bg.save(out.replace(".png", "@2x.png"), optimize=True)
bg.resize((1600, 724), Image.LANCZOS).save(out, optimize=True)
print("wrote", out)
