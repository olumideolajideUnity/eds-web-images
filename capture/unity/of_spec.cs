// Components > Object field specimens: real UI Toolkit ObjectFields (Mesh) in a plain host window, 800 x 300pt, dark.
// WHICH: anatomy (focused 'Mesh' field with the built-in Cube mesh, 2x) | variations (unlabelled empty and assigned fields) |
// dragdrop (labelled field in its accept-drop state). Rects (pt) to C:/tmp/of_spec.json.
using UnityEditor; using UnityEditor.UIElements; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection; using System.Collections.Generic;
string WHICH = "anatomy";
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var hostT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSPopupHost")).First(x => x != null);
foreach (var o in Resources.FindObjectsOfTypeAll(hostT).Cast<EditorWindow>().ToList()) o.Close();
var w = (EditorWindow)ScriptableObject.CreateInstance(hostT); w.titleContent = new GUIContent("EFDSPopupHost"); w.ShowUtility(); w.position = new Rect(200, 150, 800, 300);
var root = w.rootVisualElement; root.Clear();
var cube = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
var ps = typeof(VisualElement).GetProperty("pseudoStates", BF);
var named = new Dictionary<string, VisualElement>();
ObjectField OF(string key, string label, Object val, float x, float y, float wd, float scale = 1, float lw = 0) {
  var f = label == null ? new ObjectField { objectType = typeof(Mesh), value = val } : new ObjectField(label) { objectType = typeof(Mesh), value = val };
  f.style.position = Position.Absolute; f.style.left = x; f.style.top = y; f.style.width = wd;
  if (scale != 1) { f.style.scale = new Scale(new Vector3(scale, scale, 1)); f.style.transformOrigin = new TransformOrigin(0, 0); }
  if (lw > 0) root.schedule.Execute(() => { var l = f.Q(className: "unity-base-field__label"); l.style.minWidth = lw; l.style.width = lw; });
  root.Add(f); named[key] = f; return f; }
void Focus(VisualElement f) { root.schedule.Execute(() => { foreach (var e in new VisualElement[]{ f, f.Q(className: "unity-object-field__input"), f.Q<Label>(className: "unity-base-field__label") }) if (e != null) ps.SetValue(e, System.Enum.ToObject(ps.PropertyType, 64)); }).StartingIn(150); }
if (WHICH == "anatomy") { var f = OF("f", "Mesh", cube, 100, 212 / 2f, 250, 2f, 40); Focus(f); }
else if (WHICH == "variations") { OF("none", null, null, 150, 141, 200); OF("cube", null, cube, 450, 141, 200); }
else { var f = OF("f", "Mesh", null, 20, 132, 340, 1, 104);
  root.schedule.Execute(() => { f.Q(className: "unity-object-field-display")?.AddToClassList("unity-object-field-display--accept-drop"); var l = f.Q<Label>(className: "unity-base-field__label"); ps.SetValue(l, System.Enum.ToObject(ps.PropertyType, 64)); }).StartingIn(150); }
root.schedule.Execute(() => {
  var r = new Dictionary<string, Rect>();
  foreach (var kv in named) { r[kv.Key] = kv.Value.worldBound;
    foreach (var cls in new[]{ "unity-base-field__label", "unity-object-field__input", "unity-object-field-display__icon", "unity-object-field-display__label", "unity-object-field__selector" }) { var e = kv.Value.Q(className: cls); if (e != null) r[kv.Key + ":" + cls.Replace("unity-", "")] = e.worldBound; } }
  System.IO.File.WriteAllText("C:/tmp/of_spec.json", "{" + string.Join(",", r.Select(kv => "\"" + kv.Key + "\":[" + kv.Value.x + "," + kv.Value.y + "," + kv.Value.width + "," + kv.Value.height + "]")) + "}");
}).StartingIn(500);
w.Repaint(); return WHICH;
