"""Install or remove the EFDS fixture content (scenes, specimen components and editor windows) in the open project.

  python fixtures.py install [--project <path>]   copies fixtures/EFDS_Screenshots to <project>/Assets/EFDS_Screenshots
  python fixtures.py remove  [--project <path>]   deletes <project>/Assets/EFDS_Screenshots (and its .meta)

This WRITES to the project: Unity imports and compiles the fixture scripts (a short domain reload). Commit or back up first if the
project is under version control. If your Unity version reports compile errors in the fixtures, run `remove`.
Recipes that need the Timeline "Gameplay Sequence Demo" expect it imported from Package Manager > Timeline > Samples."""
import json, os, shutil, subprocess, sys
HERE = os.path.dirname(os.path.abspath(__file__))
if len(sys.argv) < 2 or sys.argv[1] not in ("install", "remove"):
    sys.exit(__doc__)
args = sys.argv[2:]
proj = args[args.index("--project") + 1] if "--project" in args else os.environ.get("EFDS_PROJECT")
env = dict(os.environ, UNITY_NO_PAGER="1", UNITY_NO_BANNER="1")


def cli(*a):
    r = subprocess.run(["unity", "--json", *a], capture_output=True, text=True, env=env, encoding="utf-8")
    try:
        return json.loads(r.stdout)
    except Exception:
        return {}


if not proj:
    ready = [i for i in cli("status").get("data", {}).get("instances", []) if i.get("state") == "ready"]
    if len(ready) != 1:
        sys.exit("Open exactly one Unity Editor, or pass --project / set EFDS_PROJECT.")
    proj = ready[0]["project"]
dst = os.path.join(proj, "Assets", "EFDS_Screenshots")
if sys.argv[1] == "install":
    if os.path.exists(dst):
        sys.exit(f"Already there: {dst}  (run remove first to reinstall)")
    shutil.copytree(os.path.join(HERE, "fixtures", "EFDS_Screenshots"), dst)
    shutil.copy(os.path.join(HERE, "fixtures", "EFDS_Screenshots.meta"), dst + ".meta")
else:
    if not os.path.exists(dst):
        sys.exit(f"Not installed: {dst}")
    shutil.rmtree(dst)
    if os.path.exists(dst + ".meta"):
        os.remove(dst + ".meta")
cli("command", "eval", "--project-path", proj, "--code", 'UnityEditor.AssetDatabase.Refresh(); return "refreshed";')
print(f"{sys.argv[1]}ed fixtures in {proj}. Wait for Unity to finish compiling, then run: python doctor.py")
