using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.DeclaredOnly;
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
var d = sv.overlayCanvas.overlays.First(o => o.displayName == "Tool Settings").rootVisualElement.Query<VisualElement>().ToList().First(e => e.GetType().Name == "SplineHandleSettingsDropdown");
string r = "";
for (var t = d.GetType(); t != null && t != typeof(VisualElement); t = t.BaseType)
  r += t.Name + ": " + string.Join(",", t.GetMethods(BF).Select(m => m.Name)) + " | F: " + string.Join(",", t.GetFields(BF).Select(f => f.Name)) + "\n";
return r;
