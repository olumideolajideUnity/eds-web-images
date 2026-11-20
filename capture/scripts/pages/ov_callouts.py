"""Shared: numbered green callouts in the original Overlays style (badge in a left margin, elbow leader line to a target point)."""
import os, sys; sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
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
from theme import G, FILL as F   # dark: [178, 255, 89] / [48, 60, 36]; THEME=light gives the light callout pair
def badge(cx, cy, n): return [{"t": "dot", "x": cx, "y": cy, "r": 20, "color": G}, {"t": "dot", "x": cx, "y": cy, "r": 18, "color": F},
                             {"t": "text", "s": str(n), "x": cx, "y": cy, "anchor": "mm", "size": 22, "color": G, "font": _efds_font("segoeui.ttf")}]
def callout(n, bx, by, tx, ty, elbow=None):
    ex = elbow if elbow is not None else bx + 50
    return badge(bx, by, n) + [{"t": "line", "x1": bx + 20, "y1": by, "x2": ex, "y2": by, "color": G, "w": 2},
                               {"t": "line", "x1": ex, "y1": by, "x2": tx, "y2": ty, "color": G, "w": 2}, {"t": "dot", "x": tx, "y": ty, "r": 5, "color": G}]
