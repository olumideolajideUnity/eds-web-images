"""Typography > a11y-11 (type scale in use): Unity 7 Project Settings searched for "Editor" (captures/ty_settings.png;
unity/ty_settings.cs SCROLL=1185) and the Profiler's CPU Usage module after a Play mode recording (captures/ty_profiler.png;
unity/ty_profiler.cs, ty_prof_sel.cs, ty_prof_refresh.cs, ty_hover.cs), with the original's size pills. Sizes were read
live (ty_fonts.cs, ty_prof_fonts.cs): settings title 19, search 11, help box 10, bold label 12, module title 14; the
Profiler's selected-frame value labels are 12 in Unity 7 (the original's 'tiny: 9px')."""
import json, os, subprocess, sys
from PIL import ImageFont
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, G, FILL as F, INK, OUT, themed
LX = 8.8 if LIGHT else 0   # selected frame's value label x shift (light recording: frame 80 back, lowest CPU time near the dark one's spot)
FS = "fonts/Inter-SemiBold.ttf"; PF = ImageFont.truetype(FS, 26); PAD, PH = 22, 48
K = 2
L = lambda x1, y1, x2, y2: {"t": "line", "x1": x1, "y1": y1, "x2": x2, "y2": y2, "color": G, "w": 2}
def pill(s, cx=None, cy=0, left=None):
    w = PF.getlength(s) + 2 * PAD; x = left if left is not None else cx - w / 2
    return {"t": "pill", "s": s, "x": x, "y": cy, "h": PH, "pad": PAD, "size": 26, "bg": F, "outline": G, "color": G, "font": FS}, (x, x + w)
def box(X, Y, x0, y0, x1, y1, px=4.5, py=3.5):
    a, b, c, d = X + (x0 - px) * K, Y + (y0 - py) * K, X + (x1 + px) * K, Y + (y1 + py) * K
    return {"t": "rect", "x": a, "y": b, "w_": c - a, "h": d - b, "color": G, "w": 2, "radius": 3, "dash": False, "solid": True}, (a, b, c, d)
ann = [{"t": "text", "s": "Project Settings Window", "x": 34, "y": 47, "size": 24, "color": INK, "font": "fonts/Inter-Regular.ttf"},
       {"t": "line", "x1": 0, "y1": 646.5, "x2": 1600, "y2": 646.5, "color": OUT, "w": 2},
       {"t": "text", "s": "Profiler Window", "x": 34, "y": 674, "size": 24, "color": INK, "font": "fonts/Inter-Regular.ttf"}]
SX, SY, SH = 46, 240, 203          # Project Settings window (pt height shown)
PX, PY, PHT = 170, 768, 262        # Profiler window
# very-big: 19px - the page title
r, (a, b, c, d) = box(SX, SY, 211.6, 57.6, 264.4, 72); cx = (a + c) / 2
p, (pl, pr) = pill("very-big: 19px", cx=cx, cy=160); ann += [r, p, L(cx, 184, cx, b)]
# semi-small: 11px - the search field text (pill nudged right of the first one, elbow down to the field)
r, (a, b, c, d) = box(SX, SY, 345.2, 32.8, 374.4, 40.4, px=3.5, py=3); cx2 = (a + c) / 2
p, (ql, qr) = pill("semi-small: 11px", left=pr + 24, cy=160); qc = (ql + qr) / 2
ann += [r, p, L(qc, 184, qc, 214), L(qc, 214, cx2, 214), L(cx2, 214, cx2, b)]
# small: 10px - help box text
r, (a, b, c, d) = box(SX, SY, 241.6, 105.6, 564.8, 126.8); my = (b + d) / 2
p, _ = pill("small: 10px", left=1340, cy=my); ann += [r, p, L(c, my, 1340, my)]
# default: 12px - the bold section header
r, (a, b, c, d) = box(SX, SY, 212.4, 170.4, 266.4, 181.2); my = (b + d) / 2
p, _ = pill("default: 12px", left=1340, cy=my); ann += [r, p, L(c, my, 1340, my)]
# big: 14px - the CPU Usage module title
r, (a, b, c, d) = box(PX, PY, 30, 56.8, 103.6, 69.2); cx = (a + c) / 2
p, _ = pill("big: 14px", cx=cx, cy=716); ann += [r, p, L(cx, 740, cx, b)]
# the selected frame's value label: 12px in Unity 7 (the original's 'tiny: 9px')
r, (a, b, c, d) = box(PX, PY, 542.8 + LX, 246.4, 582.8 + LX, 254.8, py=3); cx = (a + c) / 2
p, _ = pill("default: 12px", cx=cx, cy=716); ann += [r, p, L(cx, 740, cx, b)]
spec = {"out": "replacements/fundamentals/typography/a11y-11.png", "canvas": [1600, 1296], "backdrop": [203, 203, 203] if LIGHT else [25, 26, 27],
        "windows": [{"src": f"captures/ty_settings{SUF}.png", "chrome": "none", "x": SX, "y": SY, "w": 630 * K, "h": SH * K},
                    {"src": f"captures/ty_profiler{SUF}.png", "chrome": "none", "x": PX, "y": PY, "w": 630 * K, "h": PHT * K}], "annotations": ann}
lay = f"layouts/typography_02{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
