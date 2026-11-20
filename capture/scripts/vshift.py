"""Vertical offset (px) of a light capture against its dark twin: correlates per-row edge profiles (mean |row - next row|)
inside [Y0, Y1) and prints the best shift (positive = light content sits lower). Usage: python scripts/vshift.py name [Y0 Y1]"""
import sys
from PIL import Image, ImageChops
n = sys.argv[1]; Y0 = int(sys.argv[2]) if len(sys.argv) > 2 else 0; Y1 = int(sys.argv[3]) if len(sys.argv) > 3 else None
def prof(p):
    im = Image.open(p).convert("L"); w, h = im.size
    d = ImageChops.difference(im.crop((0, 0, w, h - 1)), im.crop((0, 1, w, h)))
    return [sum(d.crop((0, y, w, y + 1)).get_flattened_data()) / w for y in range(h - 1)]
a, b = prof(f"captures/{n}.png"), prof(f"captures/{n}_l.png"); Y1 = Y1 or len(a) - 40
def score(s): return sum(min(a[y], b[y + s]) for y in range(Y0, Y1) if 0 <= y + s < len(b))
best = max(range(-30, 31), key=score); print(n, "shift", best, "px =", best / 2.5, "pt")
