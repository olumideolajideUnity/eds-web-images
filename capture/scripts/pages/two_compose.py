"""Two-theme component images: light half left, dark half right, from unity/two_spec.cs (captures/two_<which>_l.png = light skin,
two_<which>.png = dark skin, rects in C:/tmp/two_<which>[_l].json) plus three Inspector / window halves (Help box overview: the real
Canvas inspector, unity/hb_canvas_warn.cs; Progress bar overview: the Depth of field specimen, unity/pb_dof.cs; Color pickers: Unity's
ColorPicker, unity/cf_pickers.cs). Each image is built once both halves exist (otherwise 'half missing'); PREVIEW=1 mirrors the light
half into the dark side and writes C:/tmp/preview/ instead, to check layout before the dark pass.
Row labels, column headers, dividers and numbered callouts follow the originals (light ink on the light half, dark-theme ink and
the dark callout green on the dark half)."""
import os as _os; _EFDS_TMP = _os.environ.get("EFDS_TMP", "C:/tmp")
import json, os, subprocess, sys
from PIL import Image
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
PREVIEW = os.environ.get("PREVIEW") == "1"
F = "fonts/Inter-Regular.ttf"
LG, LF, DG, DF = [77, 140, 80], [182, 191, 182], [178, 255, 89], [48, 60, 36]       # callout green / badge fill per half
LT, DT = [117, 117, 117], [194, 195, 196]                                            # row label / header ink per half
PPP = 2.5
def cap(name, dark):
    p = f"captures/{name}{'' if dark else '_l'}.png"
    if dark and PREVIEW: p = f"captures/{name}_l.png"
    return p
def rects(which, dark):
    p = f"{_EFDS_TMP}/two_{which}{'' if dark and not PREVIEW else '_l'}.json"
    return {k: [v * 2 for v in r] for k, r in json.load(open(p)).items()} if os.path.exists(p) else {}
def bg_of(path): im = Image.open(path).convert("RGB"); return list(im.getpixel((3, im.height - 3)))
def lab(s, x, y, dark, anchor, size=24): return {"t": "text", "s": s, "x": x, "y": y, "anchor": anchor, "size": size, "color": DT if dark else LT, "font": F}
def badge(n, cx, cy, dark):
    g, f = (DG, DF) if dark else (LG, LF)
    return [{"t": "dot", "x": cx, "y": cy, "r": 20, "color": g}, {"t": "dot", "x": cx, "y": cy, "r": 18, "color": f},
            {"t": "text", "s": str(n), "x": cx, "y": cy, "anchor": "mm", "size": 22, "color": g, "font": F}]
def ln(a, b, c, d, dark): return {"t": "line", "x1": a, "y1": b, "x2": c, "y2": d, "color": DG if dark else LG, "w": 2}
def dot(x, y, dark): return {"t": "dot", "x": x, "y": y, "r": 5, "color": DG if dark else LG}
def down(n, x, y, by, dark, ox):            # callout: dot on the target, line down (or up) to a badge at by
    return [dot(ox + x, y, dark), ln(ox + x, y, ox + x, by + (-20 if by > y else 20), dark)] + badge(n, ox + x, by, dark)
def dividers(ys, dark_only=False):
    out = []
    for dark, x0 in ((False, 0), (True, 800)):
        b = bg_of(cap(SRC, dark)); c = [min(255, v + 17) for v in b] if not dark else [min(255, v + 30) for v in b]
        out += [{"t": "line", "x1": x0, "y1": y * 2, "x2": x0 + 800, "y2": y * 2, "color": c, "w": 2} for y in ys]
    return out
SRC = ""
def build(page, n, out, which, h=600, ann=(), windows=None, src=None):
    global SRC
    l, d = src or (cap(f"two_{which}", False), cap(f"two_{which}", True))
    if not (os.path.exists(l) and os.path.exists(d)): print("half missing:", out); return
    wins = windows or [{"src": l, "chrome": "none", "x": 0, "y": 0, "w": 800}, {"src": d, "chrome": "none", "x": 800, "y": 0, "w": 800}]
    o = f"{_EFDS_TMP}/preview/{out}" if PREVIEW else f"replacements/components/{page}/{out}"
    os.makedirs(os.path.dirname(o), exist_ok=True)
    spec = {"out": o, "canvas": [1600, h], "backdrop": [194, 194, 194], "windows": wins, "annotations": list(ann)}
    lay = f"layouts/{page}_{n:02d}.json" if not PREVIEW else f"{_EFDS_TMP}/preview/{page}_{n:02d}.json"
    json.dump(spec, open(lay, "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL); print("composed", out)
def pair(which):
    global SRC; SRC = f"two_{which}"; return SRC

# --- Button
build("button", 2, "buttons-overview.png", "btn_overview")
build("button", 13, "buttons-basicbutton-states.png", "btn_states", 880)
pair("btn_tstates")
rows = ["Default", "Hover", "Pressed", "Pressed + Focused", "Focused", "Hover Focused", "Disabled"]
ann = [lab(s, 40, (51 + 56 * i) * 2, False, "lm") for i, s in enumerate(rows)] + [lab(s, 1560, (51 + 56 * i) * 2, True, "rm") for i, s in enumerate(rows)]
build("button", 14, "buttons-togglebutton-states.png", "btn_tstates", 880, ann)

# --- Color field
build("color-field", 1, "colorField-overview.png", "cf_overview")
build("color-field", 6, "colorField-is.png", "cf_is")
ann = []
for dark, ox in ((False, 0), (True, 800)):
    r = rects("cf_color", dark)
    if r:
        x, y, w_, h_ = r["f:input"]; ex, ey, ew, eh = r.get("f:eye", [x + w_, y, 24, h_])
        ann += [dot(ox + x + w_ * 0.5, y, dark), ln(ox + x + w_ * 0.5, y, ox + x + w_ * 0.5, y - 80, dark)] + badge(1, ox + x + w_ * 0.5, y - 100, dark)
        ann += down(2, ex + ew / 2, ey + eh, ey + eh + 100, dark, ox)
build("color-field", 7, "colorField-color.png", "cf_color", 600, ann)
# Color pickers: each half composed with its own window frame theme, then stitched
def pickers():
    l, d = cap("cf_pickers", False), cap("cf_pickers", True)
    if not (os.path.exists(l) and os.path.exists(d)): print("half missing: colorPickers.png"); return
    halves = {}
    for dark, src in ((False, l), (True, d)):
        spec = {"out": f"{_EFDS_TMP}/cp_half_{'d' if dark else 'l'}.png", "canvas": [800, 1092], "backdrop": [56, 56, 56] if dark else [194, 194, 194],
                "windows": [{"src": src, "title": "Color", "x": 170, "y": 60, "w": 466, "h": round(Image.open(src).height / PPP * 2) + 32}], "annotations": []}
        if not dark: spec["theme"] = "light"
        p = f"{_EFDS_TMP}/cp_half_{'d' if dark else 'l'}.json"; json.dump(spec, open(p, "w"), indent=1)
        subprocess.run([sys.executable, "scripts/compose.py", p], check=True, stdout=subprocess.DEVNULL)
    o = _EFDS_TMP + "/preview/colorPickers.png" if PREVIEW else "replacements/components/color-field/colorPickers.png"
    b, e = os.path.splitext(o)
    for chrome in ("", "--windows"):
        for sc in ("", "@2x"):
            lp = f"{_EFDS_TMP}/cp_half_l{chrome}--light{sc}.png"; dp = f"{_EFDS_TMP}/cp_half_d{chrome}{sc}.png"
            A, B = Image.open(lp).convert("RGB"), Image.open(dp).convert("RGB")
            out = Image.new("RGB", (A.width + B.width, A.height)); out.paste(A, (0, 0)); out.paste(B, (A.width, 0)); out.save(f"{b}{chrome}{sc}{e}")
    json.dump({"out": o, "canvas": [1600, 1092], "backdrop": [194, 194, 194], "windows": [], "annotations": [], "note": "stitched from C:/tmp/cp_half_l/d (scripts/pages/two_compose.py)"},
              open("layouts/color-field_03.json" if not PREVIEW else _EFDS_TMP + "/preview/color-field_03.json", "w"), indent=1)
    print("composed colorPickers.png")
pickers()

# --- Dropdown: the open menu (Unity 7 owner-drawn, scripts/pages/native_menu.py) under the focused field, per half
def dd_overview():
    sys.path.insert(0, "scripts/pages"); from native_menu import draw
    srcs = [cap("two_dd_overview", False), cap("two_dd_overview", True)]
    if not all(os.path.exists(p) for p in srcs): print("half missing: dd-overview.png"); return
    wins = []
    for dark, ox, src in ((False, 0, srcs[0]), (True, 800, srcs[1])):
        r = rects("dd_overview", dark); x, y, w_, h_ = [v / 2 for v in r["b:input"]]          # pt
        kw = {} if dark and not PREVIEW else dict(bg=(235, 235, 235), text=(9, 9, 9), highlight=(214, 214, 214))
        menu, pad = draw("captures/menus/dd_position.txt", PPP, hot="Directional", min_width=w_, **kw)   # at the capture's 2.5 px/pt
        im = Image.open(src).convert("RGBA"); im.alpha_composite(menu, (round(x * PPP) - pad, round((y + h_) * PPP) - pad))
        p = f"captures/two_dd_overview_menu{'' if dark else '_l'}.png"; im.convert("RGB").save(p)
        wins.append({"src": p, "chrome": "none", "x": ox, "y": 0, "w": 800})
    build("dropdown", 1, "dd-overview.png", "dd_overview", windows=wins)
dd_overview()
build("dropdown", 7, "dd-states.png", "dd_states")

# --- Foldout
build("foldout", 1, "foldout-overview.png", "fo_overview")
pair("fo_states")
ann = dividers([151, 242, 333])
for i, s in enumerate(("Expanded", "Collapsed")): ann += [lab(s, [148, 278][i] * 2, 58, False, "mm"), lab(s, 800 + [73, 202][i] * 2, 58, True, "mm")]
for i, s in enumerate(("Default", "Hover", "Focused", "Disabled")): ann += [lab(s, 60, (105 + 91 * i) * 2, False, "lm"), lab(s, 1560, (105 + 91 * i) * 2, True, "rm")]
build("foldout", 8, "foldout-states.png", "fo_states", 848, ann)
ann = []
r = rects("fo_color", True)
if r:
    tx, ty, tw, th = r.get("f:text", r["f"]); ax, ay, aw, ah = r.get("f:arrow", [r["f"][0], r["f"][1], 30, 30]); ix, iy, iw, ih = r.get("f:image", [ax + aw, ay, 30, 30])
    bx = 800 + 104
    ann += badge(1, bx, ty - 34, True) + [ln(bx + 20, ty - 34, 800 + tx + tw * 0.4, ty - 34, True), ln(800 + tx + tw * 0.4, ty - 34, 800 + tx + tw * 0.4, ty + 4, True)]
    ann += badge(2, bx, ay + ah / 2, True) + [ln(bx + 20, ay + ah / 2, 800 + ax + aw * 0.35, ay + ah / 2, True), dot(800 + ax + aw * 0.35, ay + ah / 2, True)]
    ann += badge(3, bx, iy + ih + 36, True) + [ln(bx + 20, iy + ih + 36, 800 + ix + iw / 2, iy + ih + 36, True), ln(800 + ix + iw / 2, iy + ih + 36, 800 + ix + iw / 2, iy + ih, True), dot(800 + ix + iw / 2, iy + ih, True)]
build("foldout", 9, "foldout-color.png", "fo_color", 600, ann)

# --- Help box
def inspector_half(name, dark, keep_tab, erase_script):
    """Crop a floating-Inspector capture to the component (optionally keeping the tab strip): returns a processed image path."""
    p = cap(name, dark); im = Image.open(p).convert("RGB"); W = im.width; bg = im.getpixel((W // 2, im.height - 4))
    seps = [y for y in range(150, im.height) if abs(sum(im.getpixel((5, y))) - sum(bg)) > 30]
    top = seps[0]; bot = seps[-1]
    hb = im.getpixel((W - 85, top + 12))                                     # header background, clear of the title text
    if erase_script:
        ys = range(top + 8, top + 52); light = not dark or PREVIEW
        ink = [x for x in range(90, W - 70) if any((sum(im.getpixel((x, y))) < 360) if light else (sum(im.getpixel((x, y))) > 450) for y in ys)]
        groups, s0 = [], ink[0]
        for a, b in zip(ink, ink[1:] + [10 ** 6]):
            if b - a > 9: groups.append((s0, a)); s0 = b
        im.paste(hb, (groups[-1][0] - 2, top + 6, W - 70, top + 54))
    body = im.crop((0, top, W, bot + 3))
    if keep_tab:
        tab = im.crop((0, 0, W, 66)); out = Image.new("RGB", (W, 66 + body.height), bg); out.paste(tab, (0, 0)); out.paste(body, (0, 66))
    else: out = body
    o = f"captures/{name}_post{'' if dark else '_l'}.png"; out.save(o); out.info["bg"] = bg; return o, out.size, bg
def hb_overview():
    if not (os.path.exists(cap("hb_canvas_warn", False)) and os.path.exists(cap("hb_canvas_warn", True))): print("half missing: helpBox-overview.png"); return
    wins, ann = [], []
    for dark, ox in ((False, 0), (True, 800)):
        o, (w_, h_), _ = inspector_half("hb_canvas_warn", dark, False, False)
        H2 = h_ * 640 / w_
        wins.append({"src": o, "chrome": "none", "x": ox + 80, "y": 90, "w": 640})
        ann.append({"t": "rect", "x": ox + 80, "y": 90, "w_": 640, "h": H2, "color": [25, 25, 25] if dark else [153, 153, 153], "w": 2, "dash": False, "solid": True})
    build("help-box", 1, "helpBox-overview.png", "hb", 600, ann, windows=wins, src=(cap("hb_canvas_warn", False), cap("hb_canvas_warn", True)))
hb_overview()
ann = []
r = rects("hb_anatomy", True)
if r:
    x, y, w_, h_ = r["h"]; ix, iy, iw, ih = r["h:icon"]; bx, by_, bw, bh = r["h:button"]; kx, ky, kw, kh = r["h:link"]
    ann += down(1, ix + iw / 2, iy + ih / 2, y + h_ + 22, True, 800)
    ann += [ln(800 + x + w_ + 8, y + 8, 800 + x + w_ + 18, y + 8, True), ln(800 + x + w_ + 18, y + 8, 800 + x + w_ + 18, y + h_ * 0.55, True),
            ln(800 + x + w_ + 8, y + h_ * 0.55, 800 + x + w_ + 18, y + h_ * 0.55, True), ln(800 + x + w_ + 18, y + h_ * 0.3, 800 + x + w_ + 32, y + h_ * 0.3, True)] + badge(2, 800 + x + w_ + 52, y + h_ * 0.3, True)
    ann += down(3, bx + bw / 2, by_ + bh, y + h_ + 70, True, 800) + down(4, kx + kw / 2, ky + kh, y + h_ + 70, True, 800)
build("help-box", 6, "helpBox-anatomy.png", "hb_anatomy", 600, ann)
ann = []
r = rects("hb_color", True)
if r:
    x, y, w_, h_ = r["info"]; tx, ty, tw, th = r.get("info:text", [x + 60, y + 10, 200, 30]); kx, ky, kw, kh = r["info:link"]
    ann += [ln(800 + tx + 70, y - 70 + 20, 800 + tx + 70, ty + th / 2, True)] + badge(3, 800 + tx + 70, y - 70, True)
    ann += [dot(800 + x + w_ * 0.62, y + h_ * 0.35, True), ln(800 + x + w_ * 0.62, y + h_ * 0.35, 800 + x + w_ * 0.62, y - 50, True)] + badge(1, 800 + x + w_ * 0.62, y - 70, True)
    ann += [ln(800 + x + w_ * 0.81, y, 800 + x + w_ * 0.81, y - 50, True)] + badge(2, 800 + x + w_ * 0.81, y - 70, True)
    ann += [dot(800 + kx + kw, ky + kh / 2, True), ln(800 + kx + kw, ky + kh / 2, 800 + x + w_ + 30, ky + kh / 2, True)] + badge(4, 800 + x + w_ + 50, ky + kh / 2, True)
build("help-box", 8, "helpBox-color.png", "hb_color", 600, ann)

# --- Label
build("label", 1, "label-overview.png", "label_overview")

# --- List view
build("list-view", 1, "lv-overview.png", "lv_overview")
pair("lv_states")
ann = dividers([119, 177, 234, 291])
for i, s in enumerate(("Default", "Hover", "Selected", "Focused", "Disabled")): ann += [lab(s, 60, [88, 147, 204, 261, 318][i] * 2, False, "lm"), lab(s, 1560, [88, 147, 204, 261, 318][i] * 2, True, "rm")]
build("list-view", 6, "lv-states.png", "lv_states", 740, ann)
pair("lv_color")
ann = dividers([177, 234, 291])
ann += [lab("Alternating", 60, 180, False, "lm"), lab("Rows", 60, 220, False, "lm"), lab("Alternating", 1540, 180, True, "rm"), lab("Rows", 1540, 220, True, "rm")]
for i, s in enumerate(("Hover", "Selected", "Focused")): ann += [lab(s, 60, [205, 262, 319][i] * 2, False, "lm"), lab(s, 1540, [205, 262, 319][i] * 2, True, "rm")]
build("list-view", 7, "lv-color.png", "lv_color", 740, ann)

# --- Numeric field
build("numeric-field", 1, "num_field-overview.png", "nf_overview")
build("numeric-field", 8, "num_field-states.png", "nf_states")
ann = []
r = rects("nf_color", True)
if r:
    x, y, w_, h_ = r["f:input"]; tx, ty, tw, th = r.get("f:text", [x + 10, y, 30, h_]); by = y + h_ + 100
    ann += down(1, x + 22, y + h_ - 6, by, True, 800) + down(2, x + w_ * 0.67, y + h_ - 8, by, True, 800) + down(3, x + w_ * 0.8, y + 1, by, True, 800)
build("numeric-field", 9, "num_field-color.png", "nf_color", 600, ann)

# --- Object field
build("object-field", 1, "object_fields-overview.png", "of_overview")
build("object-field", 8, "object_fields-states.png", "of_states")
ann = []
r = rects("of_color", True)
if r:
    x, y, w_, h_ = r["f:input"]; sx, sy, sw, sh = r.get("f:sel", [x + w_ - 30, y, 30, h_]); by = y + h_ + 90
    ann += down(1, x + w_ * 0.42, y + h_ * 0.6, by, True, 800) + down(2, x + w_ * 0.55, y + h_ - 8, by, True, 800) + down(3, sx + sw / 2, sy + sh / 2, by, True, 800)
build("object-field", 9, "object_fields-color.png", "of_color", 600, ann)

# --- Progress bar
def pb_overview():
    if not (os.path.exists(cap("pb_dof", False)) and os.path.exists(cap("pb_dof", True))): print("half missing: pb-overview.png"); return
    wins = []
    for dark, ox in ((False, 0), (True, 800)):
        o, (w_, h_), bgc = inspector_half("pb_dof", dark, True, True)
        im = Image.open(o); full = Image.new("RGB", (w_, round(480 / 640 * w_)), bgc); full.paste(im, (0, 0)); full.save(o)
        wins.append({"src": o, "chrome": "none", "x": ox + 80, "y": 120, "w": 640})
    build("progress-bar", 1, "pb-overview.png", "pb", 600, [], windows=wins, src=(cap("pb_dof", False), cap("pb_dof", True)))
pb_overview()

# --- Radio button
build("radio-button", 1, "radio-overview.png", "rb_overview")
pair("rb_states")
ann = dividers([151, 242, 333])
for i, s in enumerate(("Checked", "Unchecked")): ann += [lab(s, [183, 310][i] * 2, 58, False, "mm"), lab(s, 800 + [88, 215][i] * 2, 58, True, "mm")]
for i, s in enumerate(("Default", "Hover", "Focused", "Disabled")): ann += [lab(s, 60, (105 + 91 * i) * 2, False, "lm"), lab(s, 1560, (105 + 91 * i) * 2, True, "rm")]
build("radio-button", 4, "radio-states.png", "rb_states", 848, ann)
