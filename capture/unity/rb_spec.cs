// Components > Radio button specimens: real UI Toolkit RadioButtons / RadioButtonGroup / DropdownField in a plain host window, 800 x 300pt, dark.
// WHICH: labeling (radio buttons with and without value labels, in two boxes) | vsdd (radio group vs dropdown). Rects to C:/tmp/rb_spec.json.
using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection; using System.Collections.Generic;
string WHICH = "labeling";
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var hostT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSPopupHost")).First(x => x != null);
foreach (var o in Resources.FindObjectsOfTypeAll(hostT).Cast<EditorWindow>().ToList()) o.Close();
var w = (EditorWindow)ScriptableObject.CreateInstance(hostT); w.titleContent = new GUIContent("EFDSPopupHost"); w.ShowUtility(); w.position = new Rect(200, 150, 800, 300);
var root = w.rootVisualElement; root.Clear();
var named = new Dictionary<string, VisualElement>();
VisualElement Box(string key, float x, float y, float wd, float h) { var b = new VisualElement(); b.style.position = Position.Absolute; b.style.left = x; b.style.top = y; b.style.width = wd; b.style.height = h;
  b.style.backgroundColor = EditorGUIUtility.isProSkin ? (Color)new Color32(56, 56, 56, 255) : (Color)new Color32(200, 200, 200, 255); root.Add(b); named[key] = b; return b; }
void Group(VisualElement box, float x, float y, float wd, bool values, int sel, float lw) {
  for (int i = 0; i < 4; i++) {
    var r = new RadioButton("Option " + (i + 1)) { value = i == sel }; if (values) r.text = "Value";
    r.style.position = Position.Absolute; r.style.left = x; r.style.top = y + i * 20; r.style.width = wd;
    root.schedule.Execute(() => { var l = r.Q(className: "unity-base-field__label"); l.style.minWidth = lw; l.style.width = lw; });
    box.Add(r); } }
if (WHICH == "labeling") {
  var a = Box("a", 100, 94, 280, 144); Group(a, 56, 32, 200, false, 1, 150);
  var b = Box("b", 420, 94, 280, 144); Group(b, 38, 32, 220, true, 1, 152);
} else {
  var a = Box("a", 100, 78, 280, 222); Group(a, 56, 32, 200, false, 1, 150);
  var b = Box("b", 420, 78, 280, 222);
  var dd = new DropdownField(new List<string>{ "Option 1", "Option 2", "Option 3", "Option 4" }, -1) { value = "Options" };
  dd.style.position = Position.Absolute; dd.style.left = 56; dd.style.top = 32; dd.style.width = 160; b.Add(dd); named["dd"] = dd;
}
root.schedule.Execute(() => System.IO.File.WriteAllText("C:/tmp/rb_spec.json", "{" + string.Join(",", named.Select(kv => "\"" + kv.Key + "\":[" + kv.Value.worldBound.x + "," + kv.Value.worldBound.y + "," + kv.Value.worldBound.width + "," + kv.Value.worldBound.height + "]")) + "}")).StartingIn(400);
w.Repaint(); return WHICH;
