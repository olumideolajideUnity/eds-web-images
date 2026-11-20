"""Build a site GIF from a user screen recording (incoming/gifs/<name>.mp4) with ffmpeg.
Usage: python scripts/rec_build.py layouts/<layout>.json
Spec: {"out": "replacements/<page>/<file>.gif", "source": "incoming/gifs/<file>.mp4", "crop": [x, y, w, h] (px in the recording),
"trim": [start, end] (s), "size": [w, h] (1x px; @2x is double), "fps": 20}.
Writes <base>.gif, <base>@2x.gif and identical --windows copies (a real recording carries the Windows cursor for both). One palette
per output (palettegen stats_mode=diff), no dithering, so UI flats stay clean."""
import json, os, shutil, subprocess, sys
ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__))); os.chdir(ROOT)
s = json.load(open(sys.argv[1], encoding="utf-8"))
x, y, w, h = s["crop"]; W, H = s["size"]; t0, t1 = s["trim"]; fps = s.get("fps", 20)
base, ext = os.path.splitext(s["out"]); os.makedirs(os.path.dirname(s["out"]), exist_ok=True)
for k, tag in ((1, ""), (2, "@2x")):
    o = f"{base}{tag}{ext}"
    vf = (f"crop={w}:{h}:{x}:{y},fps={fps},scale={W * k}:{H * k}:flags=lanczos,split[a][b];"
          f"[a]palettegen=max_colors=256:stats_mode=diff[p];[b][p]paletteuse=dither=none:diff_mode=rectangle")
    subprocess.run(["ffmpeg", "-v", "error", "-y", "-ss", str(t0), "-to", str(t1), "-i", s["source"], "-filter_complex", vf, "-loop", "0", o], check=True)
    shutil.copyfile(o, f"{base}--windows{tag}{ext}")
    print("wrote", o, f"{W * k}x{H * k}", round(os.path.getsize(o) / 1e6, 2), "MB")
