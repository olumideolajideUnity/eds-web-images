"""Fundamentals > Interactions state specimens (control-states, hover, focus, disabled, pressed): real Unity 7 UI Toolkit controls
with their states forced through UI Toolkit's own pseudo-states (unity/int_states.cs), captured in the light skin (left half,
captures/int_<which>_l.png) and the dark skin (right half, _d), each half 400pt at 2x like the original. No pointer is shown."""
import json, os, subprocess, sys
ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))); os.chdir(ROOT)
for which, out, h, n in (("buttons", "control-states.png", 728, "09"), ("hover", "hover.png", 600, "10"), ("focus", "focus.png", 600, "11"),
                         ("disabled", "disabled.png", 600, "12"), ("pressed", "pressed.png", 520, "14")):
    spec = {"out": f"replacements/fundamentals/interactions/{out}", "canvas": [1600, h], "backdrop": [56, 56, 56],
            "windows": [{"src": f"captures/int_{which}_l.png", "chrome": "none", "x": 0, "y": 0, "w": 800},
                        {"src": f"captures/int_{which}_d.png", "chrome": "none", "x": 800, "y": 0, "w": 800}], "annotations": []}
    json.dump(spec, open(f"layouts/interactions_{n}.json", "w"), indent=1)
    subprocess.run([sys.executable, "scripts/compose.py", f"layouts/interactions_{n}.json"], check=True, stdout=subprocess.DEVNULL)
