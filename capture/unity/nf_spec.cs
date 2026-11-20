// Components > Numeric field specimens: real UI Toolkit numeric fields in a plain host window (dark). Rects (pt) to C:/tmp/nf_spec.json.
// WHICH: single (420x100) | multi (420x100) | anatomy | layout | positioning | variations (800x300)
using UnityEditor; using UnityEditor.UIElements; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Collections.Generic;
string WHICH = "single";
bool small = WHICH == "single" || WHICH == "multi";
var hostT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSPopupHost")).First(x => x != null);
foreach (var o in Resources.FindObjectsOfTypeAll(hostT).Cast<EditorWindow>().ToList()) o.Close();
var w = (EditorWindow)ScriptableObject.CreateInstance(hostT); w.titleContent = new GUIContent("EFDSPopupHost"); w.ShowUtility(); w.position = new Rect(200, 150, small ? 420 : 800, small ? 100 : 300);
var root = w.rootVisualElement; root.Clear();
var named = new Dictionary<string, VisualElement>();
T At<T>(string key, T e, float x, float y, float wd = -1, float scale = 1) where T : VisualElement {
  e.style.position = Position.Absolute; e.style.left = x; e.style.top = y; if (wd > 0) e.style.width = wd;
  if (scale != 1) { e.style.scale = new Scale(new Vector3(scale, scale, 1)); e.style.transformOrigin = new TransformOrigin(0, 0); }
  root.Add(e); named[key] = e; return e; }
void LW(VisualElement f, float px) { root.schedule.Execute(() => { var l = f.Q(className: "unity-base-field__label"); if (l != null) { l.style.minWidth = px; l.style.width = px; } }); }
if (WHICH == "single") { var f = At("f", new FloatField("Depth") { value = -1 }, 40, 38, 270, 1.25f); LW(f, 96); }
else if (WHICH == "multi") { var f = At("f", new RectField("Viewport Rect"), 40, 22, 270, 1.25f); LW(f, 96); }
else if (WHICH == "anatomy") { var f = At("f", new FloatField("Depth") { value = -1 }, 220, 161, 178, 2f); LW(f, 74); }
else if (WHICH == "variations") {
  var a = At("depth", new FloatField("Depth") { value = -1 }, 34, 129, 332); LW(a, 117);
  var b = At("rect", new RectField("Viewport Rect"), 434, 128, 332); LW(b, 117);
} else if (WHICH == "positioning") {
  var s = At("fov", new Slider("Field of View", 1, 179) { value = 20, showInputField = true }, 17, 140, 340); LW(s, 117);
  var row = new VisualElement { style = { flexDirection = FlexDirection.Row, alignItems = Align.Center } };
  var lab = new Label("Min/Max Slider"); lab.style.width = 114; lab.style.marginLeft = 3; row.Add(lab);
  var lo = new IntegerField { value = 20 }; lo.style.width = 30; row.Add(lo);
  var mm = new MinMaxSlider(20, 30, 0, 100); mm.style.flexGrow = 1; row.Add(mm);   // (minValue, maxValue, lowLimit, highLimit)
  var hi = new IntegerField { value = 30 }; hi.style.width = 30; row.Add(hi);
  At("minmax", row, 417, 140, 340);
} else if (WHICH == "layout") {
  foreach (var (k, x, wd) in new[]{ ("narrow", 80f, 225f), ("wide", 345f, 371f) }) {
    var box = new VisualElement(); box.AddToClassList("unity-inspector-element");
    box.style.borderTopWidth = box.style.borderBottomWidth = box.style.borderLeftWidth = box.style.borderRightWidth = 1;
    box.style.borderTopColor = box.style.borderBottomColor = box.style.borderLeftColor = box.style.borderRightColor = EditorGUIUtility.isProSkin ? (Color)new Color32(25, 25, 25, 255) : (Color)new Color32(153, 153, 153, 255);
    box.style.paddingTop = 58;
    var f1 = new FloatField("Depth") { value = -1 }; f1.AddToClassList("unity-base-field__aligned"); box.Add(f1);
    var f2 = new FloatField("Depth") { value = -1 }; f2.AddToClassList("unity-base-field__aligned"); f2.style.marginTop = 26; box.Add(f2);
    At(k, box, x, 77, wd); box.style.height = 183; named[k + "_f2"] = f2;
  }
}
root.schedule.Execute(() => {
  var r = new Dictionary<string, Rect>();
  foreach (var kv in named) { r[kv.Key] = kv.Value.worldBound;
    var l = kv.Value.Q(className: "unity-base-field__label"); if (l != null) r[kv.Key + ":label"] = l.worldBound;
    var inp = kv.Value.Q(className: "unity-base-text-field__input") ?? kv.Value.Q(className: "unity-base-field__input"); if (inp != null) r[kv.Key + ":input"] = inp.worldBound;
    var te = kv.Value.Q(className: "unity-text-element--inner-input-field-component"); if (te != null) r[kv.Key + ":text"] = te.worldBound; }
  System.IO.File.WriteAllText("C:/tmp/nf_spec.json", "{" + string.Join(",", r.Select(kv => "\"" + kv.Key + "\":[" + kv.Value.x + "," + kv.Value.y + "," + kv.Value.width + "," + kv.Value.height + "]")) + "}");
}).StartingIn(500);
w.Repaint(); return WHICH;
