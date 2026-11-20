using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq;
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
Selection.activeGameObject = GameObject.Find("Spline");
var ts = sv.overlayCanvas.overlays.First(o => o.displayName == "Tool Settings");
return string.Join(" | ", ts.rootVisualElement.Query<VisualElement>().ToList().Where(e => e.GetType().Name.Contains("Dropdown") || e.GetType().Name.Contains("Button")).Select(e => e.GetType().FullName + "#" + e.name + ":" + e.tooltip));
