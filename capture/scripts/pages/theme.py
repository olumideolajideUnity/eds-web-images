"""Shared light/dark switch for the per-page compose scripts. Run a compose script with THEME=light to read the light-skin captures
(captures/<name>_l.png, rects C:/tmp/<name>_l.json), write layouts/<name>_light.json and the '--light' variants.
Light callout colours come from the originals' light halves (green 77,140,80 on 182,191,182)."""
import os as _os; _EFDS_TMP = _os.environ.get("EFDS_TMP", "C:/tmp")
import os
LIGHT = os.environ.get("THEME") == "light"
SUF = "_l" if LIGHT else ""                    # capture / rect file suffix
LAY = "_light" if LIGHT else ""                # layout file suffix
G = [77, 140, 80] if LIGHT else [178, 255, 89]           # callout green
FILL = [182, 191, 182] if LIGHT else [48, 60, 36]       # callout badge / pill fill
OUT = [160, 160, 160] if LIGHT else [86, 87, 88]        # pill outline, dividers
BG = [215, 215, 215] if LIGHT else [50, 50, 50]         # backdrop = the host window background (light: #D7D7D7 window-background)
INK = [9, 9, 9] if LIGHT else [255, 255, 255]           # pill / label text
TXT = [40, 40, 40] if LIGHT else [230, 230, 230]        # plain annotation text
PAGE = [194, 194, 194] if LIGHT else [40, 40, 40]       # darker page around panels (light: the originals' light halves)
def themed(spec):
    if LIGHT: spec["theme"] = "light"
    return spec
