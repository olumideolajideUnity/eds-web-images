// Hide the Overlay menu toolbar (shown along the bottom of the floating Scene view after the skin switch; hidden in the dark captures).
using UnityEditor; using UnityEditor.Overlays; using UnityEngine; using System.Linq;
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
sv.TryGetOverlay("Overlays/OverlayMenu", out Overlay o); o.displayed = false; sv.Repaint(); return "menu displayed=" + o.displayed;
