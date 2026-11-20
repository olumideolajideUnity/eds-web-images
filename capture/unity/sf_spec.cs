// Components > Search field specimens (dark): real UI Toolkit ToolbarSearchField / ToolbarPopupSearchField in Toolbars with a "+" ToolbarMenu,
// in a plain host window. States forced with pseudoStates (Hover 2, Focus 64) or SetEnabled(false). Rects (pt) to C:/tmp/sf_spec.json.
// WHICH: overview (800x300) | single, popup (420x100) | length (800x300) | anatomy (800x300) | states (400x300, dark half) | color (400x300, dark half)
using UnityEditor; using UnityEditor.UIElements; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection; using System.Collections.Generic;
string WHICH = "overview";
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var hostT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSPopupHost")).First(x => x != null);
foreach (var o in Resources.FindObjectsOfTypeAll(hostT).Cast<EditorWindow>().ToList()) o.Close();
var size = WHICH == "single" || WHICH == "popup" ? new Vector2(420, 100) : WHICH == "states" || WHICH == "color" ? new Vector2(400, 300) : new Vector2(800, 300);
var w = (EditorWindow)ScriptableObject.CreateInstance(hostT); w.titleContent = new GUIContent("EFDSPopupHost"); w.ShowUtility(); w.position = new Rect(200, 150, size.x, size.y);
var root = w.rootVisualElement; root.Clear();
var ps = typeof(VisualElement).GetProperty("pseudoStates", BF);
var named = new Dictionary<string, VisualElement>();
void PS(VisualElement e, int s) { if (e != null) ps.SetValue(e, System.Enum.ToObject(ps.PropertyType, s)); }
T At<T>(string key, T e, float x, float y, float wd, float scale = 1) where T : VisualElement {
  e.style.position = Position.Absolute; e.style.left = x; e.style.top = y; e.style.width = wd;
  if (scale != 1) { e.style.scale = new Scale(new Vector3(scale, scale, 1)); e.style.transformOrigin = new TransformOrigin(0, 0); }
  root.Add(e); named[key] = e; return e; }
ToolbarSearchField SF(string val = "", string ph = null) { var f = new ToolbarSearchField { value = val }; f.style.flexGrow = 1; f.style.width = StyleKeyword.Auto; if (ph != null) f.placeholderText = ph; return f; }
ToolbarPopupSearchField PF(string val = "") { var f = new ToolbarPopupSearchField { value = val }; f.placeholderText = "All"; f.style.flexGrow = 1; f.style.width = StyleKeyword.Auto;
  foreach (var (n, on) in new[]{ ("All", true), ("Name", false), ("Type", false) }) f.menu.AppendAction(n, a => { }, on ? DropdownMenuAction.Status.Checked : DropdownMenuAction.Status.Normal); return f; }
Toolbar TB(VisualElement field) { var tb = new Toolbar(); var plus = new ToolbarMenu(); plus.AddToClassList("unity-toolbar-menu--popup");
  var ic = new Image { image = EditorGUIUtility.IconContent("Toolbar Plus").image }; ic.style.width = 16; ic.style.height = 16; plus.Insert(0, ic); tb.Add(plus); tb.Add(field); return tb; }
void Focus(VisualElement f) { root.schedule.Execute(() => { PS(f, 64); PS(f.Q(className: "unity-text-field__input") ?? f.Q(className: "unity-base-text-field__input"), 64); }).StartingIn(150); }
void Hover(VisualElement f) { root.schedule.Execute(() => { PS(f, 2); PS(f.Q(className: "unity-text-field__input") ?? f.Q(className: "unity-base-text-field__input"), 2); }).StartingIn(150); }
if (WHICH == "overview") {
  At("r1", TB(SF()), 24, 106, 352); var t2 = At("r2", TB(SF("Search term")), 24, 166, 352); Focus(t2.Q<ToolbarSearchField>());
  At("p1", TB(PF()), 424, 106, 352); var p2 = At("p2", TB(PF()), 424, 166, 352); Focus(p2.Q<ToolbarPopupSearchField>()); }
else if (WHICH == "single") At("f", SF(), 56, 42, 308);
else if (WHICH == "popup") At("f", PF(), 56, 42, 308);
else if (WHICH == "length") { var f = At("f", SF("The quick brown fox"), 325, 140, 150); Focus(f); }
else if (WHICH == "anatomy") {
  var a = At("user", SF("User input"), 190, 40, 190, 1.5f); Focus(a);
  At("popup", PF(), 190, 80, 190, 1.5f);
  At("cond", SF("", "Placeholder text"), 190, 214, 170, 1.5f); }
else if (WHICH == "states") {
  At("default", SF(), 100, 58, 200); var h = At("hover", SF(), 100, 114, 200); Hover(h);
  var f = At("focus", SF("User input"), 100, 170, 200); Focus(f); var d = At("disabled", SF(), 100, 226, 200); d.SetEnabled(false); }
else { var f = At("f", SF("User input"), 50, 140, 150, 2f); Focus(f); }
root.schedule.Execute(() => {
  var r = new Dictionary<string, Rect>();
  foreach (var kv in named) { r[kv.Key] = kv.Value.worldBound;
    foreach (var (k, cls) in new[]{ ("input", "unity-text-field__input"), ("search", "unity-search-field-base__search-button"), ("cancel", "unity-search-field-base__cancel-button"),
                                    ("text", "unity-text-element--inner-input-field-component"), ("plus", "unity-toolbar-menu") }) {
      var e = kv.Value.Q(className: cls); if (e != null) r[kv.Key + ":" + k] = e.worldBound; } }
  System.IO.File.WriteAllText("C:/tmp/sf_spec.json", "{" + string.Join(",", r.Select(kv => "\"" + kv.Key + "\":[" + kv.Value.x + "," + kv.Value.y + "," + kv.Value.width + "," + kv.Value.height + "]")) + "}");
}).StartingIn(600);
w.Repaint(); return WHICH;
