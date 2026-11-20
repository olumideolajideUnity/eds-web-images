"""Content organization #7 (2-1-1-light_headers.png): Inspector layer schematic, matched to the original's layout.
Real Unity 7 pieces (Inspector tab strip, object icon, colors) come from captures/co_header.png; boxes and callouts
follow the original's geometry so each callout points at its own nesting level.
"""
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
from collections import Counter
from PIL import Image, ImageDraw

ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
os.chdir(ROOT)
S = 2                       # compose renders at 2x; build the base at 2x too
W, H = 780, 874
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, themed
cap = Image.open(f"captures/co_header{SUF}.png").convert("RGB")
d = json.load(open(f"captures/co_header{SUF}.json"))
ppp = d["ppp"]; K = 2.0     # canvas px per Editor point (originals show the UI at 2x)

def mode(x1, y1, x2, y2):
    reg = cap.crop((int(x1 * ppp), int(y1 * ppp), int(x2 * ppp), int(y2 * ppp)))
    return Counter(reg.get_flattened_data() if hasattr(reg, "get_flattened_data") else reg.getdata()).most_common(1)[0][0]

panel = mode(0, 28, d["win"][2], 100)                       # Unity 7 Inspector background (#323232)
edge = (160, 160, 160) if LIGHT else (24, 24, 24)         # box outline, close to Unity 7 separators
hexv = "#%02X%02X%02X" % panel

PG = (203, 203, 203) if LIGHT else (25, 25, 25)
base = Image.new("RGB", (W * S, H * S), PG)
dr = ImageDraw.Draw(base)
# Inspector tab strip from the real capture (26 pt tall), placed where the original's tab sits
tab = cap.crop((0, 0, cap.width, int(26 * ppp)))
tab = tab.resize((round(tab.width / ppp * K * S), round(26 * K * S)), Image.LANCZOS)
x0, y0 = 200, 80
base.paste(tab, (x0 * S, y0 * S))
top = y0 + 26 * K                                           # panel starts below the tab strip
dr.rectangle([x0 * S, top * S, W * S, H * S], fill=panel)
# real object icon (cube) from the capture's object header
IX1, IY1, IX2, IY2 = 6, 36, 27.0, 57.8                  # cube bounds in pt (measured); its dropdown arrow starts at x=27.6
icon = cap.crop((int(IX1 * ppp), int(IY1 * ppp), int(IX2 * ppp), int(IY2 * ppp)))
# clear the dropdown arrow's anti-aliased edge: the cube's outline never reaches below y=54pt right of x=24pt
ImageDraw.Draw(icon).rectangle([int((24 - IX1) * ppp), int((54.5 - IY1) * ppp), icon.width, icon.height], fill=panel)
icon = icon.resize((round((IX2 - IX1) * K * S), round((IY2 - IY1) * K * S)), Image.LANCZOS)
base.paste(icon, (round((x0 + IX1 * K) * S), round((y0 + IY1 * K) * S)))

def box(x, y, y2, r=6):
    dr.rounded_rectangle([x * S, y * S, (W + 20) * S, y2 * S], r * S, fill=panel, outline=edge, width=round(1.5 * S))

# nested levels, same geometry as the original
box(220, 281, H + 20)            # Component
box(240, 337, 851)               # Main controls
box(260, 477, 851 - 20)          # Sub controls: foldout header row ...
dr.line([260 * S, 528 * S, (W + 20) * S, 528 * S], fill=edge, width=round(1.5 * S))   # ... and its content
os.makedirs("captures", exist_ok=True)
base.save(f"captures/co07_base{SUF}.png")

white = [9, 9, 9] if LIGHT else [245, 245, 245]   # the hex labels
NL = chr(10)
ann = [{"t": "rect", "x": 199, "y": top - 1, "w_": 700, "h": 900, "dash": False, "solid": True, "w": 2}]   # Object outline
for label, y, dot_x, hex_y in (("Object", 164, 199, 203), ("Component", 303, 220, 308),
                               ("Main" + NL + "controls", 434, 241, 433), ("Sub" + NL + "controls", 680, 260, 681)):
    two = NL in label
    ann += [{"t": "text", "s": label, "x": 158, "y": y - (12 if two else 0), "anchor": "rm", "size": 22},
            {"t": "line", "x1": 168, "y1": y, "x2": dot_x, "y2": y}, {"t": "dot", "x": dot_x, "y": y},
            {"t": "text", "s": hexv, "x": 534, "y": hex_y, "anchor": "mm", "size": 20, "color": white,
             "font": _efds_font("segoeuib.ttf")}]

spec = {"out": "replacements/patterns/content-organization/2-1-1-light_headers.png", "canvas": [W, H], "backdrop": list(PG),
        "windows": [{"src": f"captures/co07_base{SUF}.png", "chrome": "none", "x": 0, "y": 0, "w": W}], "annotations": ann}
if LIGHT:
    for a in ann: a.setdefault("color", [120, 80, 210])
lay = f"layouts/content-organization_2-1-1-light_headers{LAY}.json"
json.dump(themed(spec) if LIGHT else spec, open(lay, "w"), indent=1)
subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
print("#7 built; Unity 7 background", hexv)
