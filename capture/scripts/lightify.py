"""Patch a per-page compose script (scripts/pages/<name>.py) for THEME=light using scripts/pages/theme.py:
light captures / rects get the '_l' suffix, layouts get '_light', callout / pill / text colours come from theme.py.
Prints each kind of change it made so the result can be reviewed. Usage: python scripts/lightify.py scripts/pages/lv_compose.py"""
import os as _os; _EFDS_TMP = _os.environ.get("EFDS_TMP", "C:/tmp")
import re, sys
p = sys.argv[1]; s = open(p, encoding="utf-8").read(); log = []
def sub(pat, rep, what, flags=0):
    global s
    s2, n = re.subn(pat, rep, s, flags=flags)
    if n: log.append(f"{what}: {n}"); s = s2
if "from theme import" not in s:
    sub(r'from ov_callouts import G, F as FILL', 'from theme import LIGHT, SUF, LAY, G, FILL, OUT as TOUT, BG as TBG, INK, TXT, PAGE as TPAGE, themed', "theme import")
    sub(r'^(OUT, BG(?:, RED)?(?:, GRN)?) = \[86, 87, 88\], \[50, 50, 50\]', r'\1 = TOUT, TBG', "OUT/BG", re.M)
    sub(r'(\bOUT, BG, RED) = TOUT, TBG(, )?', r'\1 = TOUT, TBG\2', "noop")
    sub(r'; OUT, BG = \[86, 87, 88\], \[50, 50, 50\]', '; OUT, BG = TOUT, TBG', "OUT/BG inline")
    sub(r'; BG = \[50, 50, 50\]', '; BG = TBG', "BG inline")
sub(r'json\.load\(open\(f"{_EFDS_TMP}/(\w+?)_\{(\w+)\}\.json"\)\)', r'json.load(open(f"{_EFDS_TMP}/\1_{\2}{SUF}.json"))', "rect files")
sub(r'f"captures/(\w+?)_\{(\w+)\}\.png"', r'f"captures/\1_{\2}{SUF}.png"', "capture files (f-string)")
sub(r'(?<!f)"captures/(\w+)\.png"', r'f"captures/\1{SUF}.png"', "capture files (literal)")
sub(r'json\.dump\(spec, open\(f"layouts/([\w-]+)_\{n\}\.json", "w"\), indent=1\)\n(\s+)subprocess\.run\(\[sys\.executable, "scripts/compose\.py", f"layouts/\1_\{n\}\.json"\]',
    r'lay = f"layouts/\1_{n}{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)\n\2subprocess.run([sys.executable, "scripts/compose.py", lay]', "layout write")
sub(r'"color": \[255, 255, 255\]', '"color": INK', "white text")
sub(r'"color": \[230, 230, 230\]', '"color": TXT', "light text")
open(p, "w", encoding="utf-8").write(s); print(p, "->", "; ".join(log) or "nothing changed")
