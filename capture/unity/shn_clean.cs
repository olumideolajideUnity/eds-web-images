// MODE 0: docked Scene view without gizmos and with its Tools overlay hidden (runtime only); MODE 1: restore.
using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq;
int MODE = 0;
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(s => s.docked);
if (MODE == 0) { EditorPrefs.SetBool("EFDS.svGizmos", sv.drawGizmos); sv.drawGizmos = false; }
else sv.drawGizmos = EditorPrefs.GetBool("EFDS.svGizmos", true);
foreach (var o in sv.overlayCanvas.overlays) if (o.displayName == "Tools") o.rootVisualElement.style.display = MODE == 0 ? DisplayStyle.None : DisplayStyle.Flex;
sv.Repaint(); return "gizmos=" + sv.drawGizmos;
