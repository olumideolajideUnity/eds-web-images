// Windowing dd-800: show the floating Scene view's View Options overlay (was _tmp_vo.cs).
using UnityEditor; using UnityEngine; using System.Linq;
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
var vo = sv.overlayCanvas.overlays.First(o => o.displayName == "View Options"); vo.displayed = true; sv.Repaint();
return "vo floating=" + vo.floating + " " + vo.rootVisualElement.worldBound;
