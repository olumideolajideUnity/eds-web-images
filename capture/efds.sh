# EFDS capture helpers. Source from the capture/ folder:   cd capture && source efds.sh
# Drives the Unity Editor that is open right now through the Unity CLI (`unity command eval` / `editor_window`).
# Every capture is read from the Editor's own view buffers; nothing reads the screen.
#
# Which Editor: $EFDS_PROJECT if set, otherwise the one Editor that `unity status` reports as ready.
# Where files go: captures/ (raw grabs), replacements/ (composed images), $EFDS_TMP (rect JSON, temp files).
export UNITY_NO_PAGER=1 UNITY_NO_BANNER=1
AUD="$(cd "$(dirname "${BASH_SOURCE[0]:-$0}")" && pwd)"
mkdir -p "$AUD/captures/gif" "$AUD/replacements" "$AUD/work/tmp"

# cygpath exists in Git Bash on Windows; on macOS/Linux paths are already native.
if ! command -v cygpath >/dev/null 2>&1; then cygpath(){ local a; for a in "$@"; do :; done; case "$a" in /*) echo "$a";; *) echo "$(pwd)/$a";; esac; }; fi

export EFDS_TMP="${EFDS_TMP:-$(cygpath -m "$AUD/work/tmp")}"
CAPW="$(cygpath -w "$AUD/captures")"

efds_project(){
  if [ -n "$EFDS_PROJECT" ]; then P="$EFDS_PROJECT"; return 0; fi
  local list n
  list=$(unity --json status 2>/dev/null | python -c "import sys,json
d=json.load(sys.stdin).get('data',{})
for i in d.get('instances',[]):
    if i.get('state')=='ready': print(i['project']+'|'+i.get('version',''))")
  n=$(printf "%s" "$list" | grep -c .)
  if [ "$n" = 1 ]; then P="${list%%|*}"; EFDS_EDITOR="${list##*|}"; return 0; fi
  if [ "$n" = 0 ]; then echo "efds: no ready Unity Editor found. Open a project (with com.unity.pipeline installed) or set EFDS_PROJECT." >&2
  else echo "efds: more than one Editor is open; set EFDS_PROJECT to one of:" >&2; printf "%s\n" "$list" | sed 's/|.*//; s/^/  /' >&2; fi
  return 1
}
efds_project && echo "efds: driving $P${EFDS_EDITOR:+ (Unity $EFDS_EDITOR)}"

# ueval <file.cs> [sed-expr]  - run a C# file (optionally patched with sed) and print its result.
# C:/tmp/ in any script is redirected to $EFDS_TMP, so the scripts run unchanged on any machine.
ueval(){ local f="$1" t="$EFDS_TMP/_ue_$$.cs"
  if [ -n "$2" ]; then sed -e "$2" -e "s#C:/tmp/#$EFDS_TMP/#g" "$f" > "$t"; else sed -e "s#C:/tmp/#$EFDS_TMP/#g" "$f" > "$t"; fi
  unity --json command eval --project-path "$P" --file "$(cygpath -wa "$t")" 2>&1 | grep -E '"result"|"message"' | cut -c1-400; }
# ucode '<C# statements>'
ucode(){ unity --json command eval --project-path "$P" --code "$1" 2>&1 | grep -E '"result"|"message"' | cut -c1-400; }
# capwin <window title> <name>: capture a floating window by title into captures/<name>.png
capwin(){ unity --json command editor_window --project-path "$P" --window "$1" --output-path "$CAPW/$2.png" 2>&1 | grep -E '"success"|"message"' | head -1; }
# capinsp <name>: the floating Inspector
capinsp(){
  ucode 'var t=typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.InspectorWindow"); var w=UnityEngine.Resources.FindObjectsOfTypeAll(t).Cast<UnityEditor.EditorWindow>().First(x=>!x.docked); ((UnityEditor.ActiveEditorTracker)t.GetProperty("tracker", System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic).GetValue(w)).ForceRebuild(); w.Repaint(); return "rebuilt";' >/dev/null; sleep 2
  ucode 'var t=typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.InspectorWindow"); var w=UnityEngine.Resources.FindObjectsOfTypeAll(t).Cast<UnityEditor.EditorWindow>().First(x=>!x.docked); w.titleContent=new UnityEngine.GUIContent("EFDSCapture"); return "titled";' >/dev/null; sleep 1
  capwin EFDSCapture "$1"; }
# capscene <name> / capscenefull <name>: the Scene view as drawn
capscene(){ unity --json command scene_view --project-path "$P" --source screen --output-path "$CAPW/$1.png" 2>&1 | grep -E '"success"|"message"' | head -1; }
capscenefull(){ unity --json command scene_view --project-path "$P" --source screen --max-resolution 8192 --output-path "$CAPW/$1.png" 2>&1 | grep -E '"success"|"message"' | head -1; }
# capsv <name>: the FLOATING Scene view, by window title
capsv(){ ucode 'var w=UnityEngine.Resources.FindObjectsOfTypeAll<UnityEditor.SceneView>().First(x=>x.position.x > -1000); w.titleContent=new UnityEngine.GUIContent("EFDSScene"); w.Repaint(); return "titled";' >/dev/null; sleep 1; capwin EFDSScene "$1"; }
# caphier <name>: the FLOATING Hierarchy
caphier(){ ucode 'var t=typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.SceneHierarchyWindow"); var w=UnityEngine.Resources.FindObjectsOfTypeAll(t).Cast<UnityEditor.EditorWindow>().First(x=>!x.docked); w.titleContent=new UnityEngine.GUIContent("EFDSHier"); w.Repaint(); return "titled";' >/dev/null; sleep 1; capwin EFDSHier "$1"; }
# capprops <name>: the floating Properties (Focused Inspector) window
capprops(){
  ucode 'var BF=System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic; var pe=UnityEngine.Resources.FindObjectsOfTypeAll<UnityEditor.EditorWindow>().First(w=>w.GetType().Name=="PropertyEditor"); ((UnityEditor.ActiveEditorTracker)pe.GetType().GetProperty("tracker",BF).GetValue(pe)).ForceRebuild(); pe.Repaint(); return "rebuilt";' >/dev/null; sleep 2
  ucode 'var pe=UnityEngine.Resources.FindObjectsOfTypeAll<UnityEditor.EditorWindow>().First(w=>w.GetType().Name=="PropertyEditor"); pe.titleContent=new UnityEngine.GUIContent("EFDSProps"); return "titled";' >/dev/null; sleep 1
  capwin PropertyEditor "$1"; }
# capdock <window title or type> <name>: tab strip + content of a floating window, from its view buffer
capdock(){ ueval "$AUD/unity/common_rebuild_tools.cs" >/dev/null; sleep 2; ueval "$AUD/unity/common_capture_dock.cs" "s#string MATCH = \"Inspector\"; string OUT = \"C:/tmp/out.png\";#string MATCH = \"$1\"; string OUT = \"$(cygpath -m "$AUD/captures/$2.png")\";#"; }
# capdock2 <Type> <name>: like capdock, for tabbed floating windows
capdock2(){ ueval "$AUD/unity/common_rebuild_tools.cs" >/dev/null; sleep 2; ueval "$AUD/unity/common_capture_dock2.cs" "s#string MATCH = \"Inspector\"; string OUT = \"C:/tmp/out.png\";#string MATCH = \"$1\"; string OUT = \"$(cygpath -m "$AUD/captures/$2.png")\";#"; }
# capview <GUIView type> <name>: a GUIView such as UnityEditor.Toolbar, from its own buffer
capview(){ ueval "$AUD/unity/common_capture_view.cs" "s#string VIEW = \"UnityEditor.Toolbar\"; string TYPE = \"\"; string MSG = \"\"; string OUT = \"C:/tmp/sb.png\";#string VIEW = \"$1\"; string TYPE = \"\"; string MSG = \"\"; string OUT = \"$(cygpath -m "$AUD/captures/$2.png")\";#"; }
# spec <ObjectName> <W> <H> <capname>: a specimen Inspector + its row rects (captures/<capname>.png/.json)
spec(){ ueval "$AUD/unity/spec_select.cs" "s/string NAME = \"HeightNoiseLayer\"; float W = 360, H = 420;/string NAME = \"$1\"; float W = $2, H = $3;/" >/dev/null; sleep 3
  capdock InspectorWindow "$4" >/dev/null; sleep 4
  ueval "$AUD/unity/spec_dump.cs" "s#C:/tmp/out.json#$(cygpath -m "$AUD/captures/$4.json")#"; }

# skin: print the Editor's current skin (dark|light). Switch it in Preferences > General > Editor Theme, then retake with the _l suffix.
skin(){ ucode 'return UnityEditor.EditorGUIUtility.isProSkin ? "dark" : "light";' | grep -o 'dark\|light' | head -1; }
