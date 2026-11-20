// Hide overlays whose display name or id contains any of NAMES (after tools re-show them).
using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq;
string[] NAMES = { "Shape Settings", "Element Inspector", "unity-spline-inspector" };
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
int n = 0;
foreach (var o in sv.overlayCanvas.overlays) if (NAMES.Any(k => o.displayName.Contains(k) || o.id.Contains(k))) { o.rootVisualElement.style.display = DisplayStyle.None; n++; }
sv.Repaint(); return "hidden " + n;
