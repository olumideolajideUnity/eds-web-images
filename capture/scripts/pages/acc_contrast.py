"""Accessibility a11y-4Y/4N (contrast ratios): Unity 7's Inspector Add Component button, drawn with the Inspector's own
"AC Button" style (unity/acc_acbutton.cs; the enabled drawing matches the real Inspector button pixel for pixel), enabled
and disabled, with the original's contrast badges. Ratios are WCAG 2 contrast computed from the measured fill and text."""
import json, os, subprocess, sys
from PIL import Image
from collections import Counter
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, G, OUT as TOUT, BG as TBG, INK, themed
P = 2.5; s = Image.open(f"captures/acc_acbutton{SUF}.png").convert("RGB")
def lum(c):
    f = lambda v: (v / 255) / 12.92 if v / 255 <= 0.03928 else ((v / 255 + 0.055) / 1.055) ** 2.4
    return 0.2126 * f(c[0]) + 0.7152 * f(c[1]) + 0.0722 * f(c[2])
def ratio(a, b): la, lb = sorted((lum(a), lum(b)), reverse=True); return (la + 0.05) / (lb + 0.05)
GREEN, RED, OUT, WHITE = G, ([205, 80, 40] if LIGHT else [255, 110, 64]), TOUT, INK
F = "fonts/Inter-Regular.ttf"
for key, y0 in (("4Y", 10), ("4N", 50)):
    box = (round(40 * P), round((y0 + 3) * P), round(260 * P), round((y0 + 21) * P))
    c = Counter(s.getpixel((x, y)) for x in range(box[0], box[2]) for y in range(box[1], box[3]))
    fill = c.most_common(1)[0][0]; text = max(c, key=lambda k: abs(sum(k) - sum(fill)) if c[k] > 40 else 0)
    r = ratio(fill, text); label = f"{r:.2f}:1"; ok = r >= 4.5
    # button crop (the button plus 6pt of Inspector background), centred where the original's button sits
    bx0, by0, bw, bh, m = 35.2, y0, 229.6, 24, 6
    crop = [round((bx0 - m) * P), round((by0 - m) * P), s.width - round((bx0 + bw + m) * P), s.height - round((by0 + bh + m) * P)]
    win = {"src": f"captures/acc_acbutton{SUF}.png", "chrome": "none", "x": 389 - (bw / 2 + m) * 2, "y": 232 - (bh / 2 + m) * 2,
           "w": (bw + 2 * m) * 2, "crop": crop}
    # badge: outlined capsule with a check / cross and the ratio, centred under the button like the original
    from PIL import ImageFont
    tw = ImageFont.truetype(F, 24).getlength(label); pw = 52 + tw + 20; px0 = 389 - pw / 2
    ann = [{"t": "pill", "s": "", "x": px0, "y": 322, "h": 48, "pad": pw / 2, "size": 24, "bg": TBG, "outline": OUT, "color": WHITE, "font": F},
           {"t": "text", "s": label, "x": px0 + 52, "y": 322, "size": 24, "color": WHITE, "font": F, "anchor": "lm"}]
    ix, iy = px0 + 32, 322
    if ok: ann += [{"t": "line", "x1": ix - 9, "y1": iy, "x2": ix - 3, "y2": iy + 6, "color": GREEN, "w": 3},
                   {"t": "line", "x1": ix - 3, "y1": iy + 6, "x2": ix + 9, "y2": iy - 7, "color": GREEN, "w": 3}]
    else: ann += [{"t": "line", "x1": ix - 8, "y1": iy - 8, "x2": ix + 8, "y2": iy + 8, "color": RED, "w": 3},
                  {"t": "line", "x1": ix - 8, "y1": iy + 8, "x2": ix + 8, "y2": iy - 8, "color": RED, "w": 3}]
    spec = {"out": f"replacements/fundamentals/accessibility/a11y-{key}.png", "canvas": [780, 480], "backdrop": TBG,
            "windows": [win], "annotations": ann}
    lay = f"layouts/accessibility_{key}{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
    print(key, "fill", fill, "text", text, label)
