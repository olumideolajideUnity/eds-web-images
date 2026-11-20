// Show the Tool Context toolbar overlay on the floating Scene view (it heads the top toolbar in the dark captures).
using UnityEditor; using UnityEditor.Overlays; using UnityEngine; using System.Linq;
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First();
sv.TryGetOverlay("unity-tool-contexts-toolbar", out Overlay o); if (o == null) return "none";
o.displayed = true; sv.Repaint(); return "ok " + o.displayed;
