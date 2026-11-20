# tv_grab <suffix>: capture the tree-view state specimens (unity/tv_spec.cs) in the current skin and splice hover / focus cells.
tv_grab(){ local S=$1
  for W in states color; do
    ueval unity/tv_spec.cs "s/string WHICH = \"states\";/string WHICH = \"$W\";/" >/dev/null; sleep 3; capwin EFDSPopupHost tv_${W}_base$S >/dev/null; cp $EFDS_TMP/tv_spec.json $EFDS_TMP/tv_${W}$S.json
    if [ $W = states ]; then H="2 3"; FO="6 7"; else H="1"; FO="3"; fi
    for i in $H; do ueval unity/tv_spec.cs "s/string WHICH = \"states\"; int HOV = -1, FOC = -1;/string WHICH = \"$W\"; int HOV = $i, FOC = -1;/" >/dev/null; sleep 3; capwin EFDSPopupHost tv_${W}_c${i}$S >/dev/null; done
    for i in $FO; do ueval unity/tv_spec.cs "s/string WHICH = \"states\"; int HOV = -1, FOC = -1;/string WHICH = \"$W\"; int HOV = -1, FOC = $i;/" >/dev/null; sleep 3; capwin EFDSPopupHost tv_${W}_c${i}$S >/dev/null; done
  done
  python - "$S" <<'PY'
import json, sys
from PIL import Image
S = sys.argv[1]; K = 2.5
for W, cells in (("states", {2: "r1c0", 3: "r1c1", 6: "r3c0", 7: "r3c1"}), ("color", {1: "r1c0", 3: "r3c0"})):
    r = json.load(open(f"C:/tmp/tv_{W}{S}.json")); base = Image.open(f"captures/tv_{W}_base{S}.png").convert("RGB")
    for i, key in cells.items():
        x, y, w, h = r[key]; box = tuple(int(round(v * K)) for v in (x - 3, y - 3, x + w + 3, y + h + 3))
        base.paste(Image.open(f"captures/tv_{W}_c{i}{S}.png").convert("RGB").crop(box), box[:2])
    base.save(f"captures/tv_{W}{S}.png")
PY
}
