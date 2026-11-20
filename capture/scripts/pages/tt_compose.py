"""Components > Tooltip: compose the dark images at 2x. Tooltips are real Unity 7 TooltipView renders grabbed from their own view buffer
(unity/tt_show.cs -> captures/tt_<key>.png, size + offset in C:/tmp/tt_<key>.json) with Unity 7's own tooltip strings (FOV Axis = CameraEditor.Styles.FOVAxisMode), placed the way the
Editor places them: centred on the hovered element, 10 px below it (measured from TooltipView's window rect). No pointer is drawn.
The tooltip's drop shadow is drawn by the OS and isn't part of the view buffer."""
import os as _os; _EFDS_TMP = _os.environ.get("EFDS_TMP", "C:/tmp")
import json, os, subprocess, sys
from PIL import ImageFont
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, G, FILL, OUT, BG, INK, themed
F, FB = "fonts/Inter-Regular.ttf", "fonts/Inter-SemiBold.ttf"; K = 2.5
OUTDIR = "replacements/components/tooltip"; os.makedirs(OUTDIR, exist_ok=True)
J = lambda n: json.load(open(f"{_EFDS_TMP}/{n}{SUF}.json"))
L = lambda a, b, c, d, col=G, w=2: {"t": "line", "x1": a, "y1": b, "x2": c, "y2": d, "color": col, "w": w}
PF = ImageFont.truetype(FB, 22); GF = ImageFont.truetype(F, 20)
pill = lambda s, x, y: {"t": "pill", "s": s, "x": x, "y": y, "h": 40, "pad": 14, "size": 22, "bg": BG, "outline": OUT, "color": INK, "font": FB}
def gpill(s, x, y, right=False):
    w = GF.getlength(s) + 24
    return {"t": "pill", "s": s, "x": x - w if right else x, "y": y, "h": 34, "pad": 12, "size": 20, "bg": FILL, "outline": G, "color": G, "font": F}
def tip(key, ax, ay, aw, ah):   # tooltip window (canvas px) for an anchor rect in pt
    ox, oy, w, h = J(f"tt_{key}"); x, y = ax + aw / 2 + ox, ay + ah + oy
    return {"src": f"captures/tt_{key}{SUF}.png", "chrome": "none", "x": x * 2, "y": y * 2, "w": w * 2}, (x * 2, y * 2, w * 2, h * 2)
def run(n, out, wins, ann, canvas=(1600, 600)):
    spec = {"out": f"{OUTDIR}/{out}", "canvas": list(canvas), "backdrop": BG, "windows": wins, "annotations": ann}
    lay = f"layouts/tooltip_{n}{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
full = lambda src, w=1600: {"src": src, "chrome": "none", "x": 0, "y": 0, "w": w}

# 03 when to use: new users, truncated text, alt text, disabled UI, keyboard shortcuts
s = J("tt_spec_wtu"); wins = [full(f"captures/tt_wtu{SUF}.png", 1612)]
wins.append({"src": f"captures/tl_scene{SUF}.png", "chrome": "none", "x": 1062, "y": 204, "w": 540, "crop": [int(530 * K), int(26 * K), 2050 - int(800 * K), 750 - int(51 * K)]})
wins.append({"src": f"captures/cp_apptb{SUF}.png", "chrome": "none", "x": 850, "y": 636, "w": 422, "crop": [int(1325 * K), 0, 0, 0]})
for key, (ax, ay, aw, ah) in (("fov", s["fov:label"]), ("dynres", s["dynres:label"]), ("audio", (531 + 571.6 - 530, 104, 28, 18)),
                               ("dirmon", s["dirmon:label"]), ("search", (425 + 1446 - 1325, 322, 28, 28))):
    wins.append(tip(key, ax, ay, aw, ah)[0])
ann = [L(530, 0, 530, 456, OUT, 1), L(1058, 0, 1058, 456, OUT, 1), L(0, 456, 1612, 456, OUT, 1), L(796, 456, 796, 880, OUT, 1),
       pill("New users", 40, 60), pill("Truncated text", 568, 60), pill("Alt text", 1096, 60), pill("Disabled UI", 40, 516), pill("Keyboard shortcuts", 832, 516)]
run("03", "tooltip-when-to-use.png", wins, ann, (1612, 880))

# 05 style / 10 colour: the longest tooltip on its own
lw, lh = J("tt_indirect")[2:]; X, Y = 800 - lw, 300 - lh - 30; W, H = lw * 2, lh * 2
one = [{"src": f"captures/tt_indirect{SUF}.png", "chrome": "none", "x": X, "y": Y, "w": W}]
ann = [gpill("Left aligned content", X - 30, Y + 30, True), L(X - 30, Y + 30, X + 8, Y + 30),
       {"t": "dot", "x": X + W, "y": Y + 30, "r": 4, "color": G}, L(X + W, Y + 30, X + W + 34, Y + 30), gpill("1px border", X + W + 34, Y + 30),
       L(X + W + 14, Y, X + W + 14, Y + H), L(X + W + 6, Y, X + W + 14, Y), L(X + W + 6, Y + H, X + W + 14, Y + H), L(X + W + 14, Y + 100, X + W + 34, Y + 100), gpill("Hugs contents", X + W + 34, Y + 100),
       L(X, Y + H + 14, X + W, Y + H + 14), L(X, Y + H + 6, X, Y + H + 14), L(X + W, Y + H + 6, X + W, Y + H + 14), L(X + W / 2, Y + H + 14, X + W / 2, Y + H + 40),
       gpill(f"Max width {round(lw)}px", X + W / 2 - (GF.getlength(f"Max width {round(lw)}px") + 24) / 2, Y + H + 58)]
run("05", "tooltip-style.png", one, ann)
ann = [gpill("1. text", X - 30, Y + 30, True), L(X - 30, Y + 30, X + 8, Y + 30), gpill("2. background-color", X - 30, Y + 120, True), L(X - 30, Y + 120, X + 20, Y + 120),
       {"t": "dot", "x": X + W, "y": Y + 120, "r": 4, "color": G}, L(X + W, Y + 120, X + W + 34, Y + 120), gpill("3. border-color", X + W + 34, Y + 120),
       L(X + W / 2, Y + H, X + W / 2, Y + H + 40), gpill("Shadow is rendered by OS", X + W / 2 - (GF.getlength("Shadow is rendered by OS") + 24) / 2, Y + H + 58)]
run("10", "tooltip-color.png", one, ann)

# 08 anatomy: 1 hovered element, 2 container, 3 tooltip text
s = J("tt_spec_anatomy"); lab = s["mode:label"]; tw, (tx, ty, tW, tH) = tip("mode", *lab)
lx, ly = lab[0] * 2, (lab[1] + lab[3] / 2) * 2
ann = [gpill("1. Hovered element", lx - 40, ly, True), L(lx - 40, ly, lx - 4, ly), {"t": "dot", "x": lx - 4, "y": ly, "r": 4, "color": G},
       L(tx + tW + 14, ty, tx + tW + 14, ty + tH), L(tx + tW + 6, ty, tx + tW + 14, ty), L(tx + tW + 6, ty + tH, tx + tW + 14, ty + tH),
       L(tx + tW + 14, ty + tH / 2, tx + tW + 36, ty + tH / 2), gpill("2. Container", tx + tW + 36, ty + tH / 2),
       gpill("3. Tooltip text", tx - 30, ty + 30, True), L(tx - 30, ty + 30, tx + 8, ty + 30)]
run("08", "tooltip-anatomy.png", [full(f"captures/tt_anatomy{SUF}.png"), tw], ann)

# 09 positioning: 10 px below the hovered element, horizontally centred on it
s = J("tt_spec_layout"); lab = s["pos:label"]; tw, (tx, ty, tW, tH) = tip("position", *lab)
cx = (lab[0] + lab[2] / 2) * 2; by = (lab[1] + lab[3]) * 2
ann = [{"t": "vdash", "x": cx, "y1": 0, "y2": 600, "color": [110, 110, 110], "w": 1}, {"t": "hdash", "x1": 0, "x2": 1600, "y": by, "color": [110, 110, 110], "w": 1},
       {"t": "rect", "x": lab[0] * 2, "y": lab[1] * 2, "w_": lab[2] * 2, "h": lab[3] * 2, "color": G, "w": 0, "fill": 0.25, "dash": False},
       L(tx - 24, by, tx - 24, ty, G, 2), L(tx - 30, by, tx - 18, by), L(tx - 30, ty, tx - 18, ty),
       {"t": "text", "s": "10 px", "x": tx - 36, "y": (by + ty) / 2, "anchor": "rm", "size": 20, "color": G, "font": F},
       L(cx, ty + tH, cx, ty + tH + 44), gpill("Horizontally centered", cx - (GF.getlength("Horizontally centered") + 24) / 2, ty + tH + 62)]
run("09", "tooltip-layout.png", [full(f"captures/tt_layout{SUF}.png"), tw], ann)


# 01 overview (two themes, built on the light run): the Position label and its real tooltip in each skin, centred in each half
if LIGHT:
    from PIL import Image
    canvas = Image.new("RGB", (1600, 600), (194, 194, 194)); canvas.paste((56, 56, 56), (800, 0, 1600, 600))
    for suf, ox, bg in (("_l", 0, (194, 194, 194)), ("", 800, (56, 56, 56))):
        lab = json.load(open(f"{_EFDS_TMP}/tt_spec_layout{suf}.json"))["pos:labeltext"]; cap = Image.open(f"captures/tt_layout{suf}.png").convert("RGB")
        canvas.paste(cap.getpixel((4, 4)), (ox, 0, ox + 800, 600))   # each half in its skin's window background
        crop = cap.crop(tuple(int(round(v * K)) for v in (lab[0] - 2, lab[1], lab[0] + lab[2] + 2, lab[1] + lab[3])))
        crop = crop.resize((round(crop.width * 2 / K), round(crop.height * 2 / K)), Image.LANCZOS)
        lx, ly = ox + 400 - crop.width // 2, 280; canvas.paste(crop, (lx, ly))
        tip = Image.open(f"captures/tt_position{suf}.png").convert("RGB"); tip = tip.resize((round(tip.width * 2 / K), round(tip.height * 2 / K)), Image.LANCZOS)
        canvas.paste(tip, (ox + 400 - tip.width // 2, ly + crop.height + 20))
    canvas.save("captures/tt_overview_both.png")
    spec = {"out": f"{OUTDIR}/tooltip-overview.png", "canvas": [1600, 600], "backdrop": [194, 194, 194],
            "windows": [{"src": "captures/tt_overview_both.png", "chrome": "none", "x": 0, "y": 0, "w": 1600}], "annotations": []}
    json.dump(spec, open("layouts/tooltip_01.json", "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", "layouts/tooltip_01.json"], check=True, stdout=subprocess.DEVNULL)
