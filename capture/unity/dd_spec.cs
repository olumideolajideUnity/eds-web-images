// Components > Dropdown field specimens: real UI Toolkit DropdownFields (and labels) in a plain host window (800 x H pt, dark),
// zoomed with style.scale where the original is zoomed. Rects (pt, after scaling) go to C:/tmp/dd_spec.json.
// WHICH: anatomy | themes | position | variations
using UnityEditor; using UnityEditor.UIElements; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection;
using System.Collections.Generic;
string WHICH = "anatomy";
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
float H = WHICH == "position" ? 311 : 300;
var hostT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSPopupHost")).First(x => x != null);
foreach (var o in Resources.FindObjectsOfTypeAll(hostT).Cast<EditorWindow>().ToList()) o.Close();
var w = (EditorWindow)ScriptableObject.CreateInstance(hostT); w.titleContent = new GUIContent("EFDSPopupHost"); w.ShowUtility(); w.position = new Rect(200, 150, 800, H);
var root = w.rootVisualElement; root.Clear();
var named = new Dictionary<string, VisualElement>();
var ps = typeof(VisualElement).GetProperty("pseudoStates", BF);
// field at (x, y) top-left in pt, unscaled width wpt, then scaled about its top-left by s
DropdownField F(string key, string value, float x, float y, float wpt, float s = 1, string label = null, float labelW = 0, bool focus = false) {
  var d = label == null ? new DropdownField(new List<string>{ value }, 0) : new DropdownField(label, new List<string>{ value }, 0);
  d.style.position = Position.Absolute; d.style.left = x; d.style.top = y; d.style.width = wpt; d.style.marginLeft = 0; d.style.marginTop = 0;
  if (s != 1) { d.style.scale = new Scale(new Vector3(s, s, 1)); d.style.transformOrigin = new TransformOrigin(0, 0); }
  if (labelW > 0) root.schedule.Execute(() => { var l = d.Q(className: "unity-base-field__label"); if (l != null) { l.style.minWidth = labelW; l.style.width = labelW; } });
  if (focus) root.schedule.Execute(() => { foreach (var e in new VisualElement[]{ d, d.Q(className: "unity-base-popup-field__input") }) if (e != null) ps.SetValue(e, System.Enum.ToObject(ps.PropertyType, 64)); }).StartingIn(150);
  root.Add(d); named[key] = d; return d; }
if (WHICH == "anatomy") F("field", "Checked option", 276, 90, 138, 1.8f);
else if (WHICH == "themes") F("field", "Dropdown label", 282, 138, 118, 2.0f);
else if (WHICH == "position") { F("left", "Directional", 42, 101, 172, 1.2f); F("mid", "Directional", 300, 101, 172, 1.2f); F("right", "Directional", 553, 101, 172, 1.2f); }
else if (WHICH == "variations") {
  F("os", "OS Menu Dropdown", 40, 106, 220, 1, "Label", 50); F("os2", "OS Menu Dropdown", 40, 134, 220, 1, "Label", 50, true);
  F("cu", "Custom Menu Dropdown", 440, 106, 220, 1, "Label", 50); F("cu2", "Custom Menu Dropdown", 440, 134, 220, 1, "Label", 50, true);
}
root.schedule.Execute(() => System.IO.File.WriteAllText("C:/tmp/dd_spec.json", "{" + string.Join(",", named.Select(kv => { var e = kv.Value; var inp = e.Q(className: "unity-base-popup-field__input") ?? e; var b = inp.worldBound; return "\"" + kv.Key + "\":[" + b.x + "," + b.y + "," + b.width + "," + b.height + "]"; })) + "}")).StartingIn(400);
w.Repaint();
return WHICH;
