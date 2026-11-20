"""Check what the open Unity Editor and project can retake, before you start.
Reports the Editor version, skin and UI scale, the fixture and sample content present, and how many recipes in recipes.json
have everything they need. Read-only: it changes nothing in the project.

Usage (from capture/):  python doctor.py [--project <path>] [--id <recipe id or part of one>]
Needs the Unity CLI (`unity`) and an open Editor with the com.unity.pipeline package."""
import json, os, subprocess, sys
HERE = os.path.dirname(os.path.abspath(__file__))
args = sys.argv[1:]
proj = args[args.index("--project") + 1] if "--project" in args else os.environ.get("EFDS_PROJECT")
pick = args[args.index("--id") + 1] if "--id" in args else None
env = dict(os.environ, UNITY_NO_PAGER="1", UNITY_NO_BANNER="1")


def cli(*a):
    r = subprocess.run(["unity", "--json", *a], capture_output=True, text=True, env=env, encoding="utf-8")
    try:
        return json.loads(r.stdout)
    except Exception:
        sys.exit("unity CLI failed:\n" + (r.stdout + r.stderr)[-800:])


if not proj:
    ready = [i for i in cli("status").get("data", {}).get("instances", []) if i.get("state") == "ready"]
    if len(ready) != 1:
        sys.exit("Open exactly one Unity Editor, or pass --project / set EFDS_PROJECT. Ready Editors: " + ", ".join(i["project"] for i in ready)
                 if ready else "No ready Unity Editor found. Open a project that has the com.unity.pipeline package (unity pipeline install).")
    proj = ready[0]["project"]
r = cli("command", "eval", "--project-path", proj, "--file", os.path.join(HERE, "unity", "efds_doctor.cs"))
try:
    info = json.loads(r["data"]["result"])
except Exception:
    sys.exit("doctor script failed: " + json.dumps(r)[:800])

have, content = set(info["packages"]), set(info["content"])


def ok_content(c):
    return {"fixtures": info["fixtures_compiled"] and "Assets/EFDS_Screenshots" in content,
            "sample:Atrio": "Assets/Atrio" in content,
            "sample:URP template ExampleAssets": "Assets/ExampleAssets" in content,
            "sample:Timeline Gameplay Sequence Demo": info["timeline_demo"],
            "sample:SampleScene": "Assets/Scenes/SampleScene.unity" in content}.get(c, False)


K = json.load(open(os.path.join(HERE, "recipes.json"), encoding="utf-8"))
R = [x for x in K["recipes"] if not pick or pick.lower() in x["id"].lower()]
ready, blocked = [], {}
for x in R:
    miss = [c for c in x["requires"]["content"] if not ok_content(c)] + [p for p in x["requires"]["packages"] if p not in have]
    (blocked.setdefault(", ".join(miss), []) if miss else ready).append(x)
print(f"Project:  {proj}\nUnity:    {info['unity']}   (recipes were made on {K['tested_on']})")
scale = info["pixelsPerPoint"]
print(f"Skin:     {info['skin']}   UI scale: {scale} px/pt" + ("" if abs(scale - 2.5) < 0.01 else
      "   <- layouts expect 2.5 (Preferences > UI Scaling 250%). Other scales work, but compose will resample."))
print(f"Fixtures: {'installed' if ok_content('fixtures') else 'not installed (python fixtures.py install)'}")
print(f"\n{len(ready)} of {len(R)} recipes have what they need.")
for k, v in sorted(blocked.items(), key=lambda kv: -len(kv[1])):
    print(f"  {len(v):3} need: {k}")
if pick:
    for x in R:
        print(f"\n{x['id']}  [{x['level']}]  {x['method']}\n  requires: {x['requires']}")
if not info["unity"].startswith("7000"):
    print("\nThis Editor is not the version the recipes were made on. Setup scripts that reach into Editor internals may need a small\n"
          "change (a renamed window type or field). See CAPTURE.md > 'Other projects and Unity versions'.")
