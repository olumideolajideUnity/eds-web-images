"""Components > Label alignments: a real Unity 7 Inspector (Point Light, URP Light inspector; captures/lb_light.png from
unity/lb_light.cs; the window came out 275pt wide, its minimum) and the real Timeline window's empty state (captures/lb_tl.png, unity/es_timeline.cs MODE 1 at
520pt), at 2x, with the original's orange guides: Inspector labels left-aligned on one line (41.6pt) and fields on another
(143.2pt); the Timeline message and button centred in the content area."""
import json, os, subprocess, sys
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
P = 2.5; OR = [255, 110, 64]
L = lambda a, b, c, d: {"t": "line", "x1": a, "y1": b, "x2": c, "y2": d, "color": OR, "w": 2}
IX, IY, TOP, BOT, IW = 16, 24, 116.8, 392.0, 255     # inspector capture is 275.2pt wide; keep 255pt
XI = lambda pt: IX + pt * 2; YI = lambda pt: IY + (pt - TOP) * 2
TX, TY = 548, 24; XT = lambda pt: TX + pt * 2; YT = lambda pt: TY + pt * 2
c0, c1, r0, r1 = 19.2, 500.8, 149.6, 178.0               # Timeline message + button bounds (pt), measured
area_top, area_bot, area_r = 72, 276, 520
cx, cy = (c0 + c1) / 2, (r0 + r1) / 2
rows = [(162.8, 201.8), (227.8, 307.8), (361.8, 381.8)]       # field rows (pt): Type-Mode, Light Appearance-Indirect Multiplier, Range
ann = [L(XI(x), YI(a), XI(x), YI(b)) for x in (40.6, 142.2) for a, b in rows] + [
       {"t": "rect", "x": XT(c0 - 4), "y": YT(r0 - 4), "w_": (c1 - c0 + 8) * 2, "h": (r1 - r0 + 8) * 2, "color": OR, "w": 0, "fill": 0.22, "dash": False},
       L(XT(cx), YT(area_top), XT(cx), YT(r0 - 4)), L(XT(cx), YT(r1 + 4), XT(cx), YT(area_bot)),
       L(XT(0), YT(cy), XT(c0 - 4), YT(cy)), L(XT(c1 + 4), YT(cy), XT(area_r), YT(cy))]
wins = [{"src": "captures/lb_light.png", "chrome": "none", "x": IX, "y": IY, "w": IW * 2, "crop": [0, round(TOP * P), 688 - round(IW * P), 1050 - round(BOT * P)]},
        {"src": "captures/lb_tl.png", "chrome": "none", "x": TX, "y": TY, "w": 1040}]
spec = {"out": "replacements/components/label/label-alignments.png", "canvas": [1600, 600], "backdrop": [40, 40, 40], "windows": wins, "annotations": ann}
json.dump(spec, open("layouts/label_03.json", "w"), indent=1)
subprocess.run([sys.executable, "scripts/compose.py", "layouts/label_03.json"], check=True, stdout=subprocess.DEVNULL)
