"""Build the Content organization spec images (annotated Inspector specimens) from Unity captures + recorded row rects.
Run from audit/:  python scripts/pages/content_organization.py
"""
import json, os, subprocess, sys
from PIL import Image

ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, themed
K = 2.0                      # canvas px per Editor point (the originals show the UI at 2x)
PANEL = (56, 56, 56)
OUT = "replacements/patterns/content-organization/"


class Cap:
    """A specimen capture: crops the component (header to content end) and maps Editor rects to canvas px."""
    def __init__(self, name, x0, y0):
        self.d = json.load(open(f"captures/{name}{SUF}.json"))
        self.im = Image.open(f"captures/{name}{SUF}.png").convert("RGB")
        self.ppp = self.d["ppp"]; self.wx, self.wy = self.d["win"][:2]
        self.x0, self.y0 = x0, y0
        body_top = min(r[1] for r in self.d["rects"].values()) - self.wy
        body_bot = max(r[1] + r[3] for r in self.d["rects"].values()) - self.wy
        self.top = self._scan(body_top - 22, -1)   # header row is ~22pt tall: scan up from inside it to the separator above
        self.bottom = self._scan(body_bot, +1)     # separator below the component
        self.name = name

    def _scan(self, y_pt, step):
        x = int(3 * self.ppp); y = int(y_pt * self.ppp)
        while 0 < y < self.im.height - 1:
            p = self.im.getpixel((x, y))
            if (sum(p) / 3 >= 230) if LIGHT else (sum(p) / 3 <= 30): return y / self.ppp   # component separator: 20 on a 50 panel (dark), 235 on 215 (light)
            y += step
        return y / self.ppp

    def r(self, key):
        """Editor rect -> canvas (x, y, w, h)."""
        x, y, w, h = self.d["rects"][key]
        return (self.x0 + (x - self.wx) * K, self.y0 + (y - self.wy - self.top) * K, w * K, h * K)

    def save(self, extend_to=None):
        """Crop header..content end; optionally extend the panel color down to `extend_to` canvas px."""
        crop = self.im.crop((0, int(self.top * self.ppp) + 1, self.im.width, int(self.bottom * self.ppp)))
        if extend_to:
            need = int((extend_to - self.y0) / K * self.ppp)
            if need > crop.height:
                panel = crop.getpixel((int(2 * self.ppp), crop.height - 3))   # the capture's own panel color
                ext = Image.new("RGB", (crop.width, need), panel); ext.paste(crop, (0, 0)); crop = ext
        p = f"captures/{self.name}_crop{SUF}.png"; crop.save(p); return p

    @property
    def right(self): return self.x0 + self.im.width / self.ppp * K


def layout(name, canvas, cap, src, ann):
    if LIGHT:   # annotations on the light page: compose's default lilac -> a darker violet, light-grey dashes -> mid grey
        for a in ann:
            if "color" not in a: a["color"] = [120, 80, 210]
            elif list(a["color"]) == [200, 200, 200]: a["color"] = [90, 90, 90]
    spec = {"out": OUT + name, "canvas": canvas, "backdrop": [203, 203, 203] if LIGHT else [25, 25, 25],
            "windows": [{"src": src, "chrome": "none", "x": cap.x0, "y": cap.y0, "w": cap.im.width / cap.ppp * K}],
            "annotations": ann}
    f = f"layouts/content-organization_{name.split('.')[0]}{LAY}.json"
    json.dump(themed(spec) if LIGHT else spec, open(f, "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", f], check=True, stdout=subprocess.DEVNULL)
    return f


def gap(c, a, b):
    """y (canvas) halfway between the bottom of row a and the top of row b."""
    ra, rb = c.r(a), c.r(b)
    return (ra[1] + ra[3] + rb[1]) / 2


def leader(text, ty, tx, dx, dy):
    return [{"t": "text", "s": text, "x": tx, "y": ty, "anchor": "rm", "size": 20},
            {"t": "line", "x1": tx + 8, "y1": ty, "x2": dx, "y2": dy}, {"t": "dot", "x": dx, "y": dy}]


made = {}

# 1 - Component Header padding (18 left, 4 right, top/bottom)
c = Cap("co_header", 30, 140)
body = c.r("body"); hb = c.y0 + (c.d["rects"]["f1"][1] - c.wy - c.top) * K - 8 * K   # 8pt above first row
outer = (c.x0, hb, c.right - c.x0, body[1] + body[3] + 8 * K - hb)
measured = {"left_pt": round((c.d["rects"]["body"][0] - c.wx), 1), "right_pt": round(c.d["win"][2] - (c.d["rects"]["body"][0] - c.wx) - c.d["rects"]["body"][2], 1)}
ann = [{"t": "rect", "x": outer[0], "y": outer[1], "w_": body[0] - outer[0], "h": outer[3], "fill": .22, "dash": False},
       {"t": "rect", "x": body[0] + body[2], "y": outer[1], "w_": outer[0] + outer[2] - body[0] - body[2], "h": outer[3], "fill": .22, "dash": False},
       {"t": "rect", "x": body[0], "y": outer[1], "w_": body[2], "h": body[1] - outer[1], "fill": .22, "dash": False},
       {"t": "rect", "x": body[0], "y": body[1] + body[3], "w_": body[2], "h": outer[1] + outer[3] - body[1] - body[3], "fill": .22, "dash": False},
       {"t": "rect", "x": outer[0], "y": outer[1], "w_": outer[2], "h": outer[3]},
       {"t": "text", "s": f"{measured['left_pt']:g} px (left)", "x": outer[0] - 6, "y": 72, "anchor": "lm"},
       {"t": "dot", "x": outer[0], "y": 104}, {"t": "line", "x1": outer[0], "y1": 104, "x2": outer[0], "y2": outer[1]},
       {"t": "text", "s": f"(right) {measured['right_pt']:g} px", "x": outer[0] + outer[2] + 6, "y": 72, "anchor": "rm"},
       {"t": "dot", "x": outer[0] + outer[2], "y": 104}, {"t": "line", "x1": outer[0] + outer[2], "y1": 104, "x2": outer[0] + outer[2], "y2": outer[1]},
       {"t": "line", "x1": 378, "y1": outer[1] + outer[3], "x2": 378, "y2": outer[1] + outer[3] + 30}, {"t": "dot", "x": 378, "y": outer[1] + outer[3] + 30},
       {"t": "text", "s": "8 px (top and bottom)", "x": 364, "y": outer[1] + outer[3] + 50, "anchor": "mm"}]
made["1-3-window-pads (1).png"] = layout("1-3-window-pads (1).png", [780, 630], c, c.save(), ann)
print("measured padding", measured)

# 2 - Height Noise Layer: 1px field spacing
c = Cap("co_noise", 200, 80)
lines = [gap(c, "blend", "res"), gap(c, "res", "type"), gap(c, "global", "seed"), gap(c, "seed", "voronoi"), gap(c, "voronoi", "freq"), gap(c, "freq", "height")]
hdr_bot = c.r("blend")[1] - 2
ann = [{"t": "hdash", "x1": 200, "x2": 780, "y": y} for y in [hdr_bot] + lines]
ann += leader("1px", hdr_bot, 158, 200, hdr_bot) + leader("1px", lines[0], 158, 200, lines[0])
ann += [{"t": "rect", "x": 24, "y": hdr_bot - 12, "w_": 84, "h": 34, "fill": 0, "dash": True, "color": [200, 200, 200], "w": 1},
        {"t": "rect", "x": 24, "y": lines[0] - 2, "w_": 84, "h": 40, "fill": 0, "dash": True, "color": [200, 200, 200], "w": 1}]
made["1-2-prop_fields-1.png"] = layout("1-2-prop_fields-1.png", [780, 604], c, c.save(extend_to=604), ann)

# 3 - Height Noise Layer: 1px fields, 8px groups
band = lambda a, b: (c.r(a)[1] + c.r(a)[3], c.r(b)[1])
bands = [band("type", "global"), band("height", "complexity"), band("complexity", "warm")]
w = c.r("warm"); bands.append((w[1] + w[3], w[1] + w[3] + 8 * K))
l1 = gap(c, "res", "type"); l2 = gap(c, "freq", "height")
ann = [{"t": "hdash", "x1": 200, "x2": 780, "y": y} for y in (l1, l2)]
for a, b in bands:
    ann += [{"t": "rect", "x": 200, "y": a, "w_": 580, "h": b - a, "fill": .35, "dash": False},
            {"t": "hdash", "x1": 200, "x2": 780, "y": a}, {"t": "hdash", "x1": 200, "x2": 780, "y": b}]
ann += leader("1px", l1, 152, 192, l1) + leader("8px", sum(bands[0]) / 2, 152, 192, sum(bands[0]) / 2)
ann += [{"t": "rect", "x": 24, "y": y, "w_": 78, "h": 38, "fill": 0, "dash": True, "color": [200, 200, 200], "w": 1} for y in (118, 160, 205, 256, 300)]
ann += [{"t": "rect", "x": 24, "y": 246, "w_": 78, "h": 10, "fill": .6, "dash": False}]
made["1-2-prop_fields-8.png"] = layout("1-2-prop_fields-8.png", [780, 604], c, c.save(extend_to=604), ann)

# 5 - Mesh Renderer with a reorderable list (no annotations in the original)
c = Cap("co_mesh", 80, 80)
made["1-3-container.png"] = layout("1-3-container.png", [780, 604], c, c.save(extend_to=604), [])

# 6 - Rect Transform help boxes, outlined
c = Cap("co_rect", 80, 80)
ann = []
for k in ("driven", "warning"):
    x, y, ww, hh = c.r(k); ann.append({"t": "rect", "x": x, "y": y, "w_": 780 - x + 10, "h": hh, "dash": False, "solid": True})
made["1-4-container.png"] = layout("1-4-container.png", [780, 464], c, c.save(extend_to=464), ann)

# 9 - Single fields / grouped fields / collapsed groups
c = Cap("co_noise", 200, 80)
cy = lambda k: c.r(k)[1] + c.r(k)[3] / 2
ann = []
for a, b in ((c.r("type")[1] + c.r("type")[3], c.r("global")[1]), (c.r("height")[1] + c.r("height")[3], c.r("complexity")[1]),
             (c.r("complexity")[1] + c.r("complexity")[3], c.r("warm")[1])):
    ann += [{"t": "hdash", "x1": 200, "x2": 780, "y": a}, {"t": "hdash", "x1": 200, "x2": 780, "y": b}]
def bracket(y1, y2, dots, label):
    out = [{"t": "line", "x1": 180, "y1": y1, "x2": 180, "y2": y2}, {"t": "line", "x1": 166, "y1": (y1 + y2) / 2, "x2": 180, "y2": (y1 + y2) / 2},
           {"t": "text", "s": label, "x": 156, "y": (y1 + y2) / 2 - 12, "anchor": "rm"}]
    for y in dots: out += [{"t": "line", "x1": 180, "y1": y, "x2": 204, "y2": y}, {"t": "dot", "x": 204, "y": y}]
    return out
ann += bracket(cy("blend"), cy("type"), [cy("blend"), cy("res"), cy("type")], "Single\nfields")
g1, g2 = c.r("global")[1], c.r("height")[1] + c.r("height")[3]
ann += bracket(g1, g2, [], "Grouped\nfields") + [{"t": "line", "x1": 180, "y1": g1, "x2": 204, "y2": g1}, {"t": "line", "x1": 180, "y1": g2, "x2": 204, "y2": g2}]
ann += [{"t": "text", "s": "Collapsed\ngroups", "x": 156, "y": cy("complexity") - 4, "anchor": "rm"}]
for k in ("complexity", "warm"): ann += [{"t": "line", "x1": 178, "y1": cy(k), "x2": 200, "y2": cy(k)}, {"t": "dot", "x": 200, "y": cy(k)}]
made["3-1-header.png"] = layout("3-1-header.png", [780, 604], c, c.save(extend_to=604), ann)

# 10 - Material Layer: main header, sub-header, sub-foldout
c = Cap("co_layer", 200, 80)
ann = []
for key, label in (("mainheader", "Main\nheader"), ("subheader", "Sub-\nheader"), ("subfoldout", "Sub-\nfoldout")):
    x, y, ww, hh = c.r(key); yy = y + hh / 2
    dx = x - (30 if key == "subfoldout" else 10)   # a foldout's arrow is drawn left of its indented rect
    ann += [{"t": "text", "s": label, "x": 160, "y": yy - 12, "anchor": "rm"}, {"t": "line", "x1": 168, "y1": yy, "x2": dx, "y2": yy}, {"t": "dot", "x": dx, "y": yy}]
made["3-2-foldout.png"] = layout("3-2-foldout.png", [780, 604], c, c.save(extend_to=604), ann)

# 7 - Inspector layer schematic: built by co07_schematic.py to match the original's nested-box layout
import runpy; runpy.run_path(os.path.join(ROOT, "scripts", "pages", "co07_schematic.py"))
made["2-1-1-light_headers.png"] = "layouts/content-organization_2-1-1-light_headers.json"

json.dump(made, open(f"captures/_co_made{SUF}.json", "w"), indent=1)
print("built", len(made))
