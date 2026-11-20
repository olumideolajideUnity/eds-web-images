"""Copy freshly composed images from capture/replacements/ into the repo's images/ folder and update the manifests.

  python promote.py <recipe id or part of one> [...]    e.g.  python promote.py "components/button/buttons-basic.png"
  python promote.py --all                                every recipe whose outputs exist in capture/replacements/

For each matching recipe and variant (macOS/Windows frame x dark/light) whose 1x and @2x files both exist, the files replace
images/<page>/<variant>/<file>, and manifest.json / manifest.csv get the new sizes and SHA-256s. Variants you did not
regenerate are left as they are. Review `git diff --stat` before committing."""
import csv, hashlib, json, os, shutil, sys
from PIL import Image
HERE = os.path.dirname(os.path.abspath(__file__))
REPO = os.path.dirname(HERE)
if len(sys.argv) < 2:
    sys.exit(__doc__)
R = json.load(open(os.path.join(HERE, "recipes.json"), encoding="utf-8"))["recipes"]
want = R if sys.argv[1] == "--all" else [x for x in R if any(a.lower() in x["id"].lower() for a in sys.argv[1:])]
M = json.load(open(os.path.join(REPO, "manifest.json"), encoding="utf-8"))
rows = {(r["id"], r["variant"]): r for r in M["images"]}


def sha(p):
    return hashlib.sha256(open(p, "rb").read()).hexdigest()


n = 0
for x in want:
    for var, o in x["outputs"].items():
        s1, s2 = os.path.join(HERE, o["1x"]), os.path.join(HERE, o["2x"])
        row = rows.get((x["id"], var))
        if not row or not (os.path.exists(s1) and os.path.exists(s2)):
            continue
        for src, key in ((s1, "replacement_1x"), (s2, "replacement_2x")):
            dst = os.path.join(REPO, row[key])
            shutil.copy(src, dst)
            w, h = Image.open(dst).size
            row[key + "_size"], row[key + "_sha256"] = f"{w}x{h}", sha(dst)
        row["size_changed"] = row["replacement_1x_size"] != row["original_size"]
        n += 1
        print("promoted", x["id"], var)
json.dump(M, open(os.path.join(REPO, "manifest.json"), "w", encoding="utf-8"), indent=1, ensure_ascii=False)
with open(os.path.join(REPO, "manifest.csv"), "w", newline="", encoding="utf-8-sig") as f:
    w = csv.DictWriter(f, fieldnames=list(M["images"][0].keys()))
    w.writeheader()
    w.writerows(M["images"])
print(f"{n} variant(s) promoted." if n else "Nothing promoted: no matching outputs in capture/replacements/.")
