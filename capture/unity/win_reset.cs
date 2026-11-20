// Reset the floating Scene view's overlays to their defaults (OverlayCanvas.ResetOverlay on each), as "Reset All" does.
using UnityEditor; using UnityEditor.Overlays; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
var m = typeof(OverlayCanvas).GetMethods(BF).Where(x => x.Name == "ResetOverlay").ToList();
string sig = string.Join(" | ", m.Select(x => string.Join(",", x.GetParameters().Select(p => p.ParameterType.Name))));
var one = m.First(x => x.GetParameters().Length == 1 && x.GetParameters()[0].ParameterType == typeof(Overlay));
foreach (var o in sv.overlayCanvas.overlays.ToList()) { o.rootVisualElement.style.display = StyleKeyword.Null; one.Invoke(sv.overlayCanvas, new object[]{ o }); }
sv.Repaint();
return sig + " -> shown=" + string.Join(",", sv.overlayCanvas.overlays.Where(o => o.displayed).Select(o => o.displayName));
