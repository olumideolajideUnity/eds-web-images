"""Assemble an animated GIF from Editor view-buffer frames (captures/gif/<frame>.png at 2.5 px/pt, captured by scripts/gif_cap.sh after
real mouse events sent into the window, unity/gif_mouse.cs). No screen recording: every frame comes from Unity's own view buffer.
Usage: python scripts/gif_build.py layouts/gif_<name>.json
Spec: {"out": "replacements/<page>/<file>.gif", "size": [w, h] (the original's px; 1x = 2 px/pt), "crop": [x, y, w, h] (pt, in the
capture), "frames": [["ec_0", 1500], ...] (frame name, ms), "theme": "light" (optional: writes the --light variants),
"pointer": [[x, y] per frame] (pt, relative to the crop; the arrow's tip)}.
Pointer: before a frame whose pointer differs from the previous one, the arrow glides there (eased, ~0.4 s) over the previous frame,
like moving to a control and clicking it; [x, y, "self"] glides over the frame's own image instead (hover changes, e.g. after a
tooltip closes). After the last frame it glides back to the first position so the loop is seamless.
Writes <base>.gif / <base>@2x.gif with the macOS arrow (black, white outline) and the --windows pair with the Windows arrow
(white, black outline), all four sharing one palette."""
import json, os, sys
from PIL import Image, ImageDraw
ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__))); os.chdir(ROOT)
spec = json.load(open(sys.argv[1], encoding="utf-8")); K = 2.5
x, y, w, h = spec["crop"]; W, H = spec["size"]
ARROW = [(0, 0), (0, 16), (4, 12.2), (6.9, 18.6), (9.4, 17.5), (6.6, 11.2), (11.4, 11.2)]  # pt, tip at (0, 0)
GLIDE_STEPS, STEP_MS, MIN_HOLD = 10, 40, 250

def arrow_layer(ppt, fill, edge):
    """The arrow as an RGBA sprite at ppt px/pt, drawn 4x and downsampled for clean edges."""
    S = ppt * 4; pad = 2 * S
    im = Image.new("RGBA", (round(13 * S + 2 * pad), round(21 * S + 2 * pad)), (0, 0, 0, 0)); d = ImageDraw.Draw(im)
    d.polygon([(pad + a * S, pad + b * S) for a, b in ARROW], fill=fill, outline=edge, width=max(1, round(S * 1.0)))
    return im.resize((im.width // 4, im.height // 4), Image.LANCZOS), pad / 4

src = []
for name, ms in spec["frames"]:
    im = Image.open(f"captures/gif/{name}.png").convert("RGB")
    c = Image.new("RGB", (round(w * K), round(h * K)), im.getpixel((im.width - 2, im.height - 2)))
    c.paste(im.crop((round(x * K), round(y * K), min(im.width, round((x + w) * K)), min(im.height, round((y + h) * K)))), (0, 0))
    src.append(c)

# Timeline: (image index, pointer pt or None, ms)
ptrs = spec.get("pointer") or [None] * len(spec["frames"])
def ease(t): return t * t * (3 - 2 * t)
def glide(seq, img, a, b, steal=True):
    """Add the moving frames over image img, taking their time out of the last hold (steal)."""
    n = GLIDE_STEPS
    if steal and seq and seq[-1][0] == img and seq[-1][1] == a:
        n = max(3, min(n, (seq[-1][2] - MIN_HOLD) // STEP_MS)); seq[-1] = (img, a, seq[-1][2] - n * STEP_MS)
    for k in range(1, n + 1):
        t = ease(k / n); seq.append((img, (a[0] + (b[0] - a[0]) * t, a[1] + (b[1] - a[1]) * t), STEP_MS))
seq = []
for i, (name, ms) in enumerate(spec["frames"]):
    p = ptrs[i]; prev = ptrs[i - 1] if i else None
    self_glide = bool(p and len(p) > 2 and p[2] == "self"); p = tuple(p[:2]) if p else None
    if p and prev and tuple(prev[:2]) != p:
        if self_glide:
            glide(seq, i, tuple(prev[:2]), p, steal=False)
            seq.append((i, p, max(MIN_HOLD, ms - GLIDE_STEPS * STEP_MS))); continue
        glide(seq, i - 1, tuple(prev[:2]), p)
    seq.append((i, p, ms))
if ptrs[0] and seq[-1][1] and tuple(ptrs[0][:2]) != seq[-1][1]:
    glide(seq, seq[-1][0], seq[-1][1], tuple(ptrs[0][:2]))

base, ext = os.path.splitext(spec["out"]); suf = "--light" if spec.get("theme") == "light" else ""
os.makedirs(os.path.dirname(spec["out"]), exist_ok=True)
pal = None
for scale, tag in ((1, ""), (2, "@2x")):
    ppt = 2 * scale  # output px per pt
    fr = [f.resize((W * scale, H * scale), Image.LANCZOS) for f in src]
    if pal is None:  # one palette from the 1x frames, shared by every variant
        strip = Image.new("RGB", (fr[0].width, fr[0].height * len(fr)))
        for i, f in enumerate(fr): strip.paste(f, (0, i * f.height))
        strip = strip.resize((strip.width, strip.height + 2)); ImageDraw.Draw(strip).rectangle([0, strip.height - 2, strip.width, strip.height], fill=(0, 0, 0))
        ImageDraw.Draw(strip).rectangle([0, strip.height - 1, strip.width // 2, strip.height], fill=(255, 255, 255))
        pal = strip.quantize(colors=255, method=Image.Quantize.MEDIANCUT)
    plain = [f.quantize(palette=pal, dither=Image.Dither.NONE) for f in fr]
    for chrome, fill, edge in (("", (0, 0, 0, 255), (255, 255, 255, 255)), ("--windows", (255, 255, 255, 255), (0, 0, 0, 255))):
        spr, pad = arrow_layer(ppt, fill, edge); out = []
        for img, p, ms in seq:
            if p is None: out.append((plain[img], ms)); continue
            f = fr[img].convert("RGBA")
            px, py = round(p[0] * ppt - pad), round(p[1] * ppt - pad)
            layer = Image.new("RGBA", f.size, (0, 0, 0, 0)); layer.paste(spr, (px, py), spr); f = Image.alpha_composite(f, layer)
            out.append((f.convert("RGB").quantize(palette=pal, dither=Image.Dither.NONE), ms))
        o = f"{base}{chrome}{suf}{tag}{ext}"
        out[0][0].save(o, save_all=True, append_images=[f for f, _ in out[1:]], duration=[ms for _, ms in out], loop=0, optimize=False, disposal=1)
    print("wrote", f"{base}{suf}{tag}{ext}", fr[0].size, len(out), "frames", round(sum(ms for _, ms in out) / 1000, 2), "s")
