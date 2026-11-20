# Source after efds.sh. gm <mode> <x> <y> [TYPE]: real mouse event into a floating window (unity/gif_mouse.cs).
# gf <name>: capture the floating Inspector-style window MATCH (default InspectorWindow) into captures/gif/<name>.png and wait for the file.
gm(){ ueval "$AUD/unity/gif_mouse.cs" "s/string TYPE = \"InspectorWindow\"; string MODE = \"move\"; float X = 0, Y = 0; string KEYS = \"\";/string TYPE = \"${4:-InspectorWindow}\"; string MODE = \"$1\"; float X = $2, Y = $3; string KEYS = \"\";/" >/dev/null; }
gf(){ local i t f="$AUD/captures/gif/$1.png"; rm -f "$f"; for t in 1 2 3; do capdock "${2:-InspectorWindow}" "gif/$1" >/dev/null; for i in $(seq 1 30); do [ -f "$f" ] && break; sleep 0.5; done; [ -f "$f" ] && break; done; sleep 0.5; }
# gk <chars> [TYPE]: type characters as real KeyDown events into the floating window
gk(){ ueval "$AUD/unity/gif_mouse.cs" "s/string TYPE = \"InspectorWindow\"; string MODE = \"move\"; float X = 0, Y = 0; string KEYS = \"\";/string TYPE = \"${2:-InspectorWindow}\"; string MODE = \"key\"; float X = 0, Y = 0; string KEYS = \"$1\";/" >/dev/null; }
# gq <name> [TYPE]: quick capture (no tool rebuild, no wait) for short-lived states such as the Project window's ping highlight
gq(){ ueval "$AUD/unity/common_capture_dock.cs" "s#string MATCH = \"Inspector\"; string OUT = \"$EFDS_TMP/out.png\";#string MATCH = \"${2:-InspectorWindow}\"; string OUT = \"$(cygpath -m "$AUD/captures/gif/$1.png")\";#" >/dev/null; }
