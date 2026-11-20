"""Accessibility > localisation.png (Unity Hub, Add modules with Language packs): screenshots supplied by the user (Unity Hub isn't
the Editor, so it can't be captured from an Editor view buffer): incoming/localisation-windows-raw.png (Windows, Unity 7000.0.0a20, retaken without the Hub's legal-terms banner)
and incoming/localisation-macos-raw.png (macOS, Unity 6.6). The account name and avatar (top right) and, on Windows, a local install path at the bottom edge are blurred, the macOS shot
is cropped to its window (rounded corners kept transparent), and the original's green bracket marks the Language packs section.
Writes the macOS variant as the main file and the Windows one as --windows, each at 1x (1602px wide, the original's width) and @2x."""
import json, os
from PIL import Image, ImageDraw, ImageFilter
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
G = (178, 255, 89, 255)
def make(src, crop, blur_boxes, bx, y0, y1):
    im = Image.open(src).convert("RGBA")
    if crop: im = im.crop(crop)
    for blur_box in blur_boxes:
        region = im.crop(blur_box)
        for _ in range(3): region = region.filter(ImageFilter.GaussianBlur(14))
        im.paste(region, blur_box[:2])
    d = ImageDraw.Draw(im); w = 5; tick = 52
    d.line([(bx, y0), (bx, y1)], fill=G, width=w); d.line([(bx - 2, y0), (bx + tick, y0)], fill=G, width=w); d.line([(bx - 2, y1), (bx + tick, y1)], fill=G, width=w)
    return im
base = "replacements/fundamentals/accessibility/localisation"
os.makedirs(os.path.dirname(base), exist_ok=True)
mac = make("incoming/localisation-macos-raw.png", (112, 76, 3712, 2258), [(3270 - 112, 82 - 76, 3690 - 112, 156 - 76)], 980 - 112, 1313 - 76, 1754 - 76)
win = make("incoming/localisation-windows-raw.png", None, [(3080, 14, 3440, 96), (750, 2212, 1935, 2266)], 752, 1151, 1702)   # name + avatar; install path at the bottom edge
for im, suffix in ((mac, ""), (win, "--windows")):
    h1 = round(im.height * 1602 / im.width)
    im.resize((1602, h1), Image.LANCZOS).save(f"{base}{suffix}.png")
    im.resize((3204, h1 * 2), Image.LANCZOS).save(f"{base}{suffix}@2x.png")
    print(suffix or "macos", (1602, h1))
json.dump({"out": base + ".png", "note": "built by scripts/pages/acc_hub.py from user-supplied Hub screenshots (not composed)"},
          open("layouts/accessibility_localisation.json", "w"), indent=1)
