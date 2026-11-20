"""Compose Editor captures into the foundations.unity.com screenshot style.

Usage: python compose.py layout.json [--chrome macos|windows|all]   (default: all)
layout.json:
{ "out": "replacements/patterns/authoring-flows/af-gO_scene-insp.png",
  "canvas": [1600, 724],
  "windows": [ {"src": "captures/x.png", "title": "Scene", "x": 137, "y": 80, "w": 838, "h": 563} ] }
x/y/w/h are in 1x canvas pixels and describe the whole window including its title bar.
Optional "crop": [l, t, r, b] trims capture pixels first; "hide_right": N paints over the right N px (e.g. a scrollbar gutter).
Optional top-level "zoom" (chrome scale, e.g. 1.5 for zoomed crops) and "backdrop": [r,g,b] (flat background).
Optional top-level "cursor": {"x": .., "y": ..} draws a pointer with its tip there (canvas px).
Use "h": null to let a window run off the bottom of the canvas, as some originals do.
Writes one file per window chrome at 1x and @2x:
  <out>                  macOS frame (matches the site today)
  <out minus .png>--windows.png   Windows 11 frame
  With "theme": "light" in the layout: <base>--light.png and <base>--windows--light.png (light Editor skin, light frames).
"""
import json, sys, os
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

S = 2
spec = json.load(open(sys.argv[1]))
CW, CH = spec.get("canvas", [1600, 724])
W, H = CW * S, CH * S
def backdrop():
    if spec.get("backdrop"): return Image.new("RGB", (W, H), tuple(spec["backdrop"]))
    bg = Image.new("RGB", (W, H))
    d = ImageDraw.Draw(bg)
    for y in range(H):
        c = round(226 - 5 * y / H) if spec.get("theme") == "light" else round(41 - 5 * y / H)
        d.line([(0, y), (W, y)], fill=(c, c, c))
    return bg
try:
    font = ImageFont.truetype(_efds_font("segoeuib.ttf"), 15 * S)
except OSError:
    font = ImageFont.load_default()
try:
    font_win = ImageFont.truetype(_efds_font("segoeui.ttf"), 14 * S)
except OSError:
    font_win = font

Z = spec.get("zoom", 1.0)            # chrome scale for zoomed-in crops (title bar, corner radius, title font)
BAR = round(38 * Z) * S
try:
    font = ImageFont.truetype(_efds_font("segoeuib.ttf"), round(15 * Z) * S)
    font_win = ImageFont.truetype(_efds_font("segoeui.ttf"), round(14 * Z) * S)
except OSError:
    pass

LIGHT = spec.get("theme") == "light"
# Window frame colours. Windows: Unity colours the native title bar from --unity-colors-app_toolbar-background / default-text
# (ContainerWindow.TryGetNativeThemeColors; dark #141414 / #D2D2D2 drawn as before, light #EBEBEB / #090909 read live).
# macOS: the system's light / dark window title bar.
BARC = (235, 235, 235) if LIGHT else (32, 32, 32)
TXT = (9, 9, 9) if LIGHT else (220, 220, 220)
MAC_TXT = (77, 77, 77) if LIGHT else (196, 196, 196)
LINE = (200, 200, 200) if LIGHT else (35, 35, 35)

def chrome_bar(w_img, ww, title, chrome, inactive=False):
    wd = ImageDraw.Draw(w_img)
    if chrome == "macos":
        for i, col in enumerate([(255, 95, 87), (254, 188, 46), (40, 200, 64)] if not inactive else [(85, 85, 85)] * 3):
            cx, cy = round((20 + i * 20) * Z) * S, BAR // 2
            r6 = round(6 * Z) * S; wd.ellipse([cx - r6, cy - r6, cx + r6, cy + r6], fill=col)
        wd.text(((ww - wd.textlength(title, font=font)) / 2, BAR / 2), title, font=font, fill=(120, 120, 120) if inactive else MAC_TXT, anchor="lm")
    else:  # Windows 11: title left, minimize / maximize / close on the right
        wd.rectangle([0, 0, ww, BAR], fill=BARC)
        tx = round(14 * Z) * S
        logo_p = os.path.join(os.path.dirname(os.path.dirname(os.path.abspath(__file__))), "assets", "unity_logo.png")
        if os.path.exists(logo_p):
            ls = round(18 * Z) * S
            logo = Image.open(logo_p).convert("RGBA").resize((ls, ls), Image.LANCZOS)
            w_img.paste(logo, (tx, (BAR - ls) // 2), logo)
            tx += ls + round(9 * Z) * S
        wd.text((tx, BAR / 2), title, font=font_win, fill=(130, 130, 130) if inactive else TXT, anchor="lm")
        bw, g, cy = round(46 * Z) * S, round(5 * Z) * S, BAR // 2
        col = (120, 120, 120) if inactive else TXT; lw = max(1, S)
        cx = ww - bw // 2                                   # close
        wd.line([cx - g, cy - g, cx + g, cy + g], fill=col, width=lw); wd.line([cx - g, cy + g, cx + g, cy - g], fill=col, width=lw)
        cx -= bw                                            # maximize
        wd.rectangle([cx - g, cy - g, cx + g, cy + g], outline=col, width=lw)
        cx -= bw                                            # minimize
        wd.line([cx - g, cy, cx + g, cy], fill=col, width=lw)
    wd.line([(0, BAR - 1), (ww, BAR - 1)], fill=LINE, width=S)

def render(chrome):
    R = round((10 if chrome == "macos" else 8) * Z) * S
    bg = backdrop()
    for win in spec["windows"]:
        x0, y0, ww = round(win["x"] * S), round(win["y"] * S), round(win["w"] * S)
        if win.get("chrome") == "none":       # bare panel crop, no title bar / shadow / rounded corners (spec images)
            cap = Image.open(win["src"]).convert("RGB")
            if win.get("crop"):
                l, t, r, b = win["crop"]; cap = cap.crop((l, t, cap.width - r, cap.height - b))
            hh = round(cap.height * ww / cap.width)
            cap = cap.resize((ww, hh), Image.LANCZOS)
            if win.get("h"): cap = cap.crop((0, 0, ww, min(hh, win["h"] * S)))
            bg.paste(cap, (x0, y0)); continue
        wh = (win["h"] * S) if win.get("h") else (H - y0 + R)
        cap = Image.open(win["src"]).convert("RGB")
        if win.get("crop"):   # [left, top, right, bottom] in capture pixels to trim, e.g. a scrollbar strip
            l, t, r, b = win["crop"]; cap = cap.crop((l, t, cap.width - r, cap.height - b))
        if win.get("hide_right"):  # paint the rightmost N capture pixels with the panel color (hides a scrollbar gutter)
            n = win["hide_right"]; col = cap.getpixel((cap.width - n - 4, min(cap.height - 1, 400)))
            ImageDraw.Draw(cap).rectangle([cap.width - n, win.get("hide_from", 66), cap.width, cap.height], fill=col)  # 66 px = dock tab strip (26 pt at 2.5x)
        ch = round(cap.height * ww / cap.width)
        cap = cap.resize((ww, ch), Image.LANCZOS).crop((0, 0, ww, min(ch, wh - BAR)))
        sh = Image.new("L", (W, H), 0)
        ImageDraw.Draw(sh).rounded_rectangle([x0, y0 + 8 * S, x0 + ww, y0 + wh + 8 * S], R, fill=150)
        bg.paste((0, 0, 0), (0, 0), sh.filter(ImageFilter.GaussianBlur(18 * S)))
        w_img = Image.new("RGB", (ww, wh), (232, 232, 232) if LIGHT else (56, 56, 56))
        w_img.paste(cap, (0, BAR))
        chrome_bar(w_img, ww, win.get("title", ""), chrome, win.get("inactive", False))
        m = Image.new("L", (ww, wh), 0)
        ImageDraw.Draw(m).rounded_rectangle([0, 0, ww - 1, wh - 1], R, fill=255)
        bg.paste(w_img, (x0, y0), m)
        ImageDraw.Draw(bg).rounded_rectangle([x0, y0, x0 + ww - 1, y0 + wh - 1], R,
            outline=(90, 90, 90) if chrome == "macos" else (70, 70, 70), width=S)
    if spec.get("annotations"):
        draw_annotations(bg, spec["annotations"])
    if spec.get("cursor"):
        draw_cursor(bg, spec["cursor"]["x"] * S, spec["cursor"]["y"] * S, chrome)
    return bg

ANN = (175, 145, 244)
def draw_annotations(img, items):
    """Spec annotations in canvas 1x px. Types: hdash, vdash, rect, line, dot, text."""
    ov = Image.new("RGBA", img.size, (0, 0, 0, 0)); d = ImageDraw.Draw(ov)
    def dash(x1, y1, x2, y2, w, on=7, off=5, col=None):
        import math
        L = math.hypot(x2 - x1, y2 - y1) or 1; ux, uy = (x2 - x1) / L, (y2 - y1) / L; t = 0
        while t < L:
            e = min(t + on * S, L); d.line([(x1 + ux * t, y1 + uy * t), (x1 + ux * e, y1 + uy * e)], fill=(col or ANN) + (255,), width=w); t = e + off * S
    for a in items:
        k = a["t"]; c = tuple(a.get("color", ANN)); w = round(a.get("w", 2) * S)
        if k == "hdash": dash(a["x1"] * S, a["y"] * S, a["x2"] * S, a["y"] * S, w, col=c if "color" in a else None)
        elif k == "vdash": dash(a["x"] * S, a["y1"] * S, a["x"] * S, a["y2"] * S, w, col=c if "color" in a else None)
        elif k == "rect":
            x, y, rw, rh = (v * S for v in (a["x"], a["y"], a["w_"], a["h"]))
            if a.get("fill"): d.rectangle([x, y, x + rw, y + rh], fill=c + (round(255 * a["fill"]),))
            if a.get("dash", True):
                for (x1, y1, x2, y2) in ((x, y, x + rw, y), (x, y + rh, x + rw, y + rh), (x, y, x, y + rh), (x + rw, y, x + rw, y + rh)): dash(x1, y1, x2, y2, w, col=c)
            elif a.get("solid"):
                if a.get("radius"): d.rounded_rectangle([x, y, x + rw, y + rh], a["radius"] * S, outline=c + (255,), width=w)
                else: d.rectangle([x, y, x + rw, y + rh], outline=c + (255,), width=w)
        elif k == "line": d.line([(a["x1"] * S, a["y1"] * S), (a["x2"] * S, a["y2"] * S)], fill=c + (255,), width=w)
        elif k == "dot":
            r = a.get("r", 5) * S; d.ellipse([a["x"] * S - r, a["y"] * S - r, a["x"] * S + r, a["y"] * S + r], fill=c + (255,))
        elif k == "pill":   # outlined capsule label (x = left edge, y = centre), like the site's state labels
            f = ImageFont.truetype(a.get("font", _efds_font("seguisb.ttf")), round(a.get("size", 22) * S))
            tw = d.textlength(a["s"], font=f); ph = a.get("h", 46) * S; px = a.get("pad", 16) * S
            x0, yc = a["x"] * S, a["y"] * S
            d.rounded_rectangle([x0, yc - ph / 2, x0 + tw + 2 * px, yc + ph / 2], ph / 2, fill=tuple(a.get("bg", (56, 56, 56))) + (255,),
                                outline=tuple(a.get("outline", (86, 87, 88))) + (255,), width=round(2 * S))
            d.text((x0 + px, yc), a["s"], font=f, fill=c + (255,), anchor="lm")
        elif k == "text":
            f = ImageFont.truetype(a.get("font", _efds_font("consolab.ttf")), round(a.get("size", 20) * S))
            if a.get("bg"):
                bb = d.textbbox((a["x"] * S, a["y"] * S), a["s"], font=f, anchor=a.get("anchor", "lm")); pd = 6 * S
                d.rounded_rectangle([bb[0] - pd, bb[1] - pd, bb[2] + pd, bb[3] + pd], 4 * S, fill=tuple(a["bg"]) + (255,))
            for i, line in enumerate(a["s"].split(chr(10))):
                d.text((a["x"] * S, (a["y"] + i * a.get("size", 20) * 1.15) * S), line, font=f, fill=c + (255,), anchor=a.get("anchor", "lm"))
            if a.get("_underline") and a["_underline"] in a["s"]:   # underline one word (left-anchored single line)
                pre = a["s"].split(a["_underline"])[0]
                ux0 = a["x"] * S + d.textlength(pre, font=f); ux1 = ux0 + d.textlength(a["_underline"], font=f)
                uy = a["y"] * S + f.size * 0.62
                d.line([(ux0, uy), (ux1, uy)], fill=tuple(a.get("_ucol", a.get("color", ANN))) + (255,), width=max(1, round(1.5 * S)))
        elif k == "image":   # paste a PNG (e.g. an Editor icon) at x,y scaled to width w
            ic = Image.open(a["src"]).convert("RGBA"); iw = round(a["w"] * S)
            ic = ic.resize((iw, round(ic.height * iw / ic.width)), Image.LANCZOS)
            ov.alpha_composite(ic, (round(a["x"] * S), round(a["y"] * S)))
    img.paste(ov, (0, 0), ov)

def draw_cursor(img, x, y, chrome):
    """Draw an OS-style arrow pointer with its tip at (x, y). macOS: black with white rim; Windows: white with black rim."""
    pts = [(0, 0), (0, 17), (4.2, 13.2), (7, 19.6), (9.6, 18.5), (6.9, 12.2), (12.2, 12.2)]
    k = S * 1.15
    poly = [(x + px * k, y + py * k) for px, py in pts]
    fill, rim = ((0, 0, 0), (255, 255, 255)) if chrome == "macos" else ((255, 255, 255), (0, 0, 0))
    sh = Image.new("L", img.size, 0)
    ImageDraw.Draw(sh).polygon([(px + 1.5 * S, py + 2 * S) for px, py in poly], fill=110)
    img.paste((0, 0, 0), (0, 0), sh.filter(ImageFilter.GaussianBlur(1.6 * S)))
    d = ImageDraw.Draw(img)
    d.polygon(poly, fill=rim)
    # inset the inner fill to leave a rim
    cx = sum(px for px, _ in poly) / len(poly); cy = sum(py for _, py in poly) / len(poly)
    inner = [(cx + (px - cx) * 0.78, cy + (py - cy) * 0.80 + 0.6 * S) for px, py in poly]
    d.polygon(inner, fill=fill)

chromes = ["macos", "windows"]
if "--chrome" in sys.argv:
    c = sys.argv[sys.argv.index("--chrome") + 1]
    if c != "all": chromes = [c]
out = spec["out"]
os.makedirs(os.path.dirname(out), exist_ok=True)
base, ext = os.path.splitext(out)
for c in chromes:
    b = (base if c == "macos" else base + "--" + c) + ("--light" if LIGHT else "")   # light: <base>--light / <base>--windows--light
    img = render(c)
    img.save(b + "@2x" + ext, optimize=True)
    img.resize((CW, CH), Image.LANCZOS).save(b + ext, optimize=True)
    print("wrote", b + ext, "and", b + "@2x" + ext)
