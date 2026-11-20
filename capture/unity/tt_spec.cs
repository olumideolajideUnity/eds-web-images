// Components > Tooltip specimens (dark): the real controls each tooltip anchors to (UI Toolkit DropdownField / Toggle / Vector3Field) in a
// plain host window; label rects (pt) to C:/tmp/tt_spec.json. Tooltips themselves are real TooltipView captures (unity/tt_show.cs).
// WHICH: wtu (806x440) | anatomy, layout (800x300)
using UnityEditor; using UnityEditor.UIElements; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Collections.Generic;
string WHICH = "wtu";
var hostT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSPopupHost")).First(x => x != null);
foreach (var o in Resources.FindObjectsOfTypeAll(hostT).Cast<EditorWindow>().ToList()) o.Close();
var size = WHICH == "wtu" ? new Vector2(806, 440) : new Vector2(800, 300);
var w = (EditorWindow)ScriptableObject.CreateInstance(hostT); w.titleContent = new GUIContent("EFDSPopupHost"); w.ShowUtility(); w.position = new Rect(200, 150, size.x, size.y);
var root = w.rootVisualElement; root.Clear();
var named = new Dictionary<string, VisualElement>();
T At<T>(string key, T e, float x, float y, float wd) where T : VisualElement { e.style.position = Position.Absolute; e.style.left = x; e.style.top = y; e.style.width = wd; root.Add(e); named[key] = e; return e; }
void LW(VisualElement f, float px) { root.schedule.Execute(() => { var l = f.Q(className: "unity-base-field__label"); if (l != null) { l.style.minWidth = px; l.style.width = px; } }); }
DropdownField DD(string label, params string[] ch) => new DropdownField(label, ch.ToList(), 0);
if (WHICH == "wtu") {
  var a = At("fov", DD("FOV Axis", "Vertical", "Horizontal"), 40, 104, 220); LW(a, 78);
  var b = At("dynres", new Toggle("Allow Dynamic Resolution"), 318, 104, 150); LW(b, 120);
  var d = At("dirmon", new Toggle("Directory Monitoring"), 120, 314, 170); LW(d, 140); d.SetEnabled(false); }
else if (WHICH == "anatomy") { var m = At("mode", DD("Mode", "Realtime", "Mixed", "Baked"), 266, 98, 280); LW(m, 120); }
else { var p = At("pos", new Vector3Field("Position"), 295, 120, 335); LW(p, 136); }
root.schedule.Execute(() => {
  var r = new Dictionary<string, Rect>();
  foreach (var kv in named) { r[kv.Key] = kv.Value.worldBound; var l = kv.Value.Q(className: "unity-base-field__label"); if (l != null) r[kv.Key + ":label"] = l.worldBound;
    var te = l?.Q<TextElement>() ?? l as TextElement; if (te != null) { var sz = te.MeasureTextSize(te.text, 0, VisualElement.MeasureMode.Undefined, 0, VisualElement.MeasureMode.Undefined); r[kv.Key + ":labeltext"] = new Rect(l.worldBound.x, l.worldBound.y, Mathf.Min(sz.x, l.worldBound.width), l.worldBound.height); } }
  System.IO.File.WriteAllText("C:/tmp/tt_spec.json", "{" + string.Join(",", r.Select(kv => "\"" + kv.Key + "\":[" + kv.Value.x + "," + kv.Value.y + "," + kv.Value.width + "," + kv.Value.height + "]")) + "}");
}).StartingIn(600);
w.Repaint(); return WHICH;
