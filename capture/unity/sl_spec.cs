// Components > Slider specimens (dark): real UI Toolkit Slider / MinMaxSlider (+ IntegerFields, as Unity's own min/max rows) in a plain host
// window. Focus forced with pseudoStates (Focus 64), hover with Hover 2. Rects (pt) to C:/tmp/sl_spec.json.
// WHICH: variations (800x300) | basic, minmax (420x100) | style (800x300, 2.5x zoom) | states (400x300, dark half) | themes (400x300, dark half)
using UnityEditor; using UnityEditor.UIElements; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection; using System.Collections.Generic;
string WHICH = "variations"; string FOCUSKEY = "s2";   // variations: which row gets real keyboard focus (s2 or m2); captures are spliced
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var hostT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSPopupHost")).First(x => x != null);
foreach (var o in Resources.FindObjectsOfTypeAll(hostT).Cast<EditorWindow>().ToList()) o.Close();
var size = WHICH == "basic" || WHICH == "minmax" ? new Vector2(420, 100) : WHICH == "states" || WHICH == "themes" ? new Vector2(400, 300) : new Vector2(800, 300);
var w = (EditorWindow)ScriptableObject.CreateInstance(hostT); w.titleContent = new GUIContent("EFDSPopupHost"); w.ShowUtility(); w.position = new Rect(200, 150, size.x, size.y);
var root = w.rootVisualElement; root.Clear();
var ps = typeof(VisualElement).GetProperty("pseudoStates", BF);
var named = new Dictionary<string, VisualElement>();
void PS(VisualElement e, int s) { if (e != null) ps.SetValue(e, System.Enum.ToObject(ps.PropertyType, s)); }
T At<T>(string key, T e, float x, float y, float wd, float scale = 1, VisualElement parent = null) where T : VisualElement {
  e.style.position = Position.Absolute; e.style.left = x; e.style.top = y; if (wd > 0) e.style.width = wd;
  if (scale != 1) { e.style.scale = new Scale(new Vector3(scale, scale, 1)); e.style.transformOrigin = new TransformOrigin(0, 0); }
  (parent ?? root).Add(e); named[key] = e; return e; }
void LW(VisualElement f, float px) { root.schedule.Execute(() => { var l = f.Q(className: "unity-base-field__label"); if (l != null) { l.style.minWidth = px; l.style.width = px; } }); }
void Focus(VisualElement f) { root.schedule.Execute(() => { foreach (var e in new[]{ f, f.Q(className: "unity-base-field__label"), f.Q(className: "unity-base-slider__dragger"),
    f.Q(className: "unity-base-slider__drag-container"), f.Q(className: "unity-base-text-field__input") }) PS(e, 64); }).StartingIn(150); }
void RealFocus(VisualElement e) { root.schedule.Execute(() => { w.Focus(); e.Focus(); }).StartingIn(250); }
void HoverAt(VisualElement e) { root.schedule.Execute(() => { var d = e.Q(className: "unity-base-slider__dragger").worldBound;
  w.wantsMouseMove = true; w.Focus(); w.SendEvent(new Event { type = EventType.MouseMove, mousePosition = d.center }); }).StartingIn(120); }   // window event, not the OS pointer
VisualElement MinMaxRow(string label, int lo, int hi, float lw) {   // Unity's own min/max rows: label, min field, MinMaxSlider, max field
  var row = new VisualElement { style = { flexDirection = FlexDirection.Row, alignItems = Align.Center } };
  var lab = new Label(label); lab.AddToClassList("unity-base-field__label"); lab.style.width = lw; lab.style.minWidth = lw; lab.style.marginLeft = 3; row.Add(lab);
  var a = new IntegerField { value = lo }; a.style.width = 30; row.Add(a);
  var mm = new MinMaxSlider(lo, hi, 0, 100); mm.style.flexGrow = 1; row.Add(mm);
  var b = new IntegerField { value = hi }; b.style.width = 30; row.Add(b); return row; }
if (WHICH == "variations") {
  var s1 = At("s1", new Slider("Basic Slider", 0, 100) { value = 20, showInputField = true }, 74, 110, 303); LW(s1, 82);
  var s2 = At("s2", new Slider("Basic Slider", 0, 100) { value = 50, showInputField = true }, 74, 170, 303); LW(s2, 82); if (FOCUSKEY == "s2") RealFocus(s2);
  At("m1", MinMaxRow("Min/Max Slider", 20, 30, 90), 432, 110, 309);
  var m2 = At("m2", MinMaxRow("Min/Max Slider", 20, 60, 90), 432, 170, 309);
  if (FOCUSKEY == "m2") RealFocus(m2.Q<MinMaxSlider>()); }
else if (WHICH == "basic") At("s", new Slider(0, 100) { value = 15 }, 130, 41, 160);
else if (WHICH == "minmax") At("s", new MinMaxSlider(13, 24, 0, 100), 130, 41, 160);
else if (WHICH == "style") { var s = At("s", new Slider("Field of view", 0, 300) { value = 100, showInputField = true }, 120, 135, 220, 2.5f); LW(s, 84); }
else if (WHICH == "states") {
  At("default", new Slider(0, 100) { value = 25 }, 50, 50, 200); var h = At("hover", new Slider(0, 100) { value = 25 }, 50, 110, 200);
  HoverAt(h);
  var f = At("focus", new Slider(0, 100) { value = 25 }, 50, 170, 200); RealFocus(f); }
else { var f = At("s", new Slider(0, 100) { value = 25 }, 100, 140, 200); RealFocus(f); }
root.schedule.Execute(() => {
  var r = new Dictionary<string, Rect>();
  foreach (var kv in named) { r[kv.Key] = kv.Value.worldBound;
    foreach (var (k, cls) in new[]{ ("label", "unity-base-field__label"), ("tracker", "unity-base-slider__tracker"), ("dragger", "unity-base-slider__dragger"),
                                    ("input", "unity-base-text-field__input"), ("drag", "unity-base-slider__drag-container") }) {
      var e = kv.Value.Q(className: cls); if (e != null) r[kv.Key + ":" + k] = e.worldBound; } }
  System.IO.File.WriteAllText("C:/tmp/sl_spec.json", "{" + string.Join(",", r.Select(kv => "\"" + kv.Key + "\":[" + kv.Value.x + "," + kv.Value.y + "," + kv.Value.width + "," + kv.Value.height + "]")) + "}");
}).StartingIn(600);
w.Repaint(); return WHICH;
