"""Errors & messaging #2-4: Unity 7's own IMGUI help boxes (EFDS.EFDSMessageSpecimens mode 0), each cropped from the
capture and placed like the original (840x320, box 720 px wide, centred)."""
import json, os, subprocess, sys
from PIL import Image
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
sys.path.insert(0, "scripts/pages"); from theme import LIGHT, SUF, LAY, themed
rects = json.loads(sys.argv[1] if len(sys.argv) > 1 else "[[4,10,372,28],[4,52,372,28],[4,94,372,28]]"); ppp = 2.5; TAB = 26   # help box rects (pt, content) from unity/em_spec.cs
cap = Image.open(f"captures/em_helpboxes{SUF}.png").convert("RGB")
for (x, y, w, h), name, n in zip(rects, ("ea type-error", "ea type-warning", "ea type-log"), (2, 3, 4)):
    m = 1   # keep the 1pt border fully
    box = cap.crop((round((x - m) * ppp), round((y + TAB - m) * ppp), round((x + w + m) * ppp), round((y + TAB + h + m) * ppp)))
    src = f"captures/em_{name.split('-')[-1]}{SUF}.png"; box.save(src)
    W = 720 * (w + 2 * m) / w; H = W * box.height / box.width
    spec = {"out": f"replacements/patterns/errors-and-messaging/{name}.png", "canvas": [840, 320], "backdrop": [226, 226, 226] if LIGHT else [56, 56, 56],
            "windows": [{"src": src, "chrome": "none", "x": round((840 - W) / 2), "y": round(160 - H / 2), "w": round(W)}]}
    lay = f"layouts/errors-and-messaging_{n:02d}{LAY}.json"; json.dump(themed(spec), open(lay, "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", lay], check=True, stdout=subprocess.DEVNULL)
    print(name, box.size)
