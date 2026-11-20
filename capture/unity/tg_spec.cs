// Components > Toggle specimens (dark): real UI Toolkit Toggle / ToolbarToggle / ToggleButtonGroup in a plain host window.
// Rects (pt) to C:/tmp/tg_spec.json.
// WHICH: variations, vs, style, behavior, anatomy, prefs (800x300) | toggle, tbtoggle (420x100) | allowhdr, hdr (390x240)
using UnityEditor; using UnityEditor.UIElements; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection; using System.Collections.Generic;
string WHICH = "variations"; int HOV = -1, FOC = -1;   // states: column (0 unchecked, 1 checked, 2 mixed) that gets real hover / real focus
var hostT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSPopupHost")).First(x => x != null);
foreach (var o in Resources.FindObjectsOfTypeAll(hostT).Cast<EditorWindow>().ToList()) o.Close();
var size = WHICH == "toggle" || WHICH == "tbtoggle" ? new Vector2(420, 100) : WHICH == "allowhdr" || WHICH == "hdr" ? new Vector2(390, 240) : WHICH == "states" ? new Vector2(400, 424) : WHICH == "overview" || WHICH == "themes" ? new Vector2(400, 300) : new Vector2(800, 300);
var w = (EditorWindow)ScriptableObject.CreateInstance(hostT); w.titleContent = new GUIContent("EFDSPopupHost"); w.ShowUtility(); w.position = new Rect(200, 150, size.x, size.y);
var root = w.rootVisualElement; root.Clear();
var named = new Dictionary<string, VisualElement>();
T At<T>(string key, T e, float x, float y, float wd, float scale = 1) where T : VisualElement {
  e.style.position = Position.Absolute; e.style.left = x; e.style.top = y; if (wd > 0) e.style.width = wd;
  if (scale != 1) { e.style.scale = new Scale(new Vector3(scale, scale, 1)); e.style.transformOrigin = new TransformOrigin(0, 0); }
  root.Add(e); named[key] = e; return e; }
void LW(VisualElement f, float px) { root.schedule.Execute(() => { var l = f.Q(className: "unity-base-field__label"); if (l != null) { l.style.minWidth = px; l.style.width = px; } }); }
Toggle TG(string label, bool v, bool mixed = false) { var t = new Toggle(label) { value = v }; t.showMixedValue = mixed; return t; }
Toolbar TB(params VisualElement[] items) { var tb = new Toolbar(); foreach (var i in items) tb.Add(i); return tb; }
if (WHICH == "variations") { var t = At("toggle", TG("Toggle (Checkbox)", false), 91, 135, 140, 1.6f); LW(t, 118);
  At("tb", TB(new ToolbarToggle { text = "Toolbar Toggle" }), 529, 135, 104, 1.6f); }
else if (WHICH == "toggle") { var t = At("t", TG("Toggle Label", false), 156, 40, 120); LW(t, 98); }
else if (WHICH == "tbtoggle") { var tb = TB(new ToolbarToggle { text = "Toolbar Toggle" }); At("tb", tb, 92, 40, 240); }
else if (WHICH == "allowhdr") { var t = At("t", TG("Allow HDR", false), 124, 118, 100, 1.6f); LW(t, 75); }
else if (WHICH == "hdr") { var t = At("t", TG("HDR", false), 151, 118, 60, 1.6f); LW(t, 42); }
else if (WHICH == "vs") {
  foreach (var (p, x, y, label, v, m) in new[]{ ("parent", 139f, 122f, "Parent option", false, true), ("c1", 149f, 142f, "Child option 1", true, false), ("c2", 149f, 162f, "Child option 2", false, false) }) {
    var t = At(p, TG(label, v, m), x, y, 165); LW(t, 144 - (x - 139)); }
  var g = new ToggleButtonGroup("Font Style") { isMultipleSelection = true, allowEmptySelection = true };
  foreach (var s in new[]{ "B", "I", "U", "S" }) g.Add(new Button { text = s });
  var st = ToggleButtonGroupState.CreateFromOptions(new List<bool>{ false, true, false, false }); g.value = st;
  At("group", g, 476, 102, 210); LW(g, 112);
  var tb = TB(new ToolbarToggle { text = "Maximize On Play" }); At("toolbar", tb, 440, 203, 281); }
else if (WHICH == "style") {
  var a = At("checked", TG("Checked", true), 68, 141, 96, 1.5f); LW(a, 72);
  var b = At("mixed", TG("Indeterminate\n(Mixed)", false, true), 312, 135, 128, 1.5f); LW(b, 102);
  var c = At("unchecked", TG("Unchecked", false), 592, 141, 110, 1.5f); LW(c, 86); }
else if (WHICH == "behavior") {
  foreach (var (p, x, y, label, v, m) in new[]{ ("parent", 272f, 103f, "Parent option", false, true), ("c1", 282f, 123f, "Child option 1", true, false), ("c2", 282f, 143f, "Child option 2", false, false),
                                               ("c3", 282f, 163f, "Child option 3", true, false), ("c4", 282f, 183f, "Child option 4", false, false) }) {
    var t = At(p, TG(label, v, m), x, y, 270 - (x - 272)); LW(t, 250 - (x - 272)); } }
else if (WHICH == "anatomy") { var t = At("t", TG("Checked", true), 274, 123, 90, 3f); LW(t, 68); }
else if (WHICH == "overview") { var t = At("toggle", TG("Toggle (Checkbox)", false), 89, 98, 140, 1.6f); LW(t, 118); At("tb", TB(new ToolbarToggle { text = "Toolbar Toggle" }), 118, 172, 104, 1.6f); }   // 104pt: Unity 7's font needs it (92 truncated the label)
else if (WHICH == "themes") { var t = At("t", TG(null, true), 180, 128, 20, 3f); }
else if (WHICH == "states") {   // grid: columns unchecked / checked / mixed (x from the original's layout per skin), rows default / hover / focused / disabled
  float[] cols = EditorGUIUtility.isProSkin ? new[]{ 63f, 153f, 244f } : new[]{ 150f, 241f, 330f }; float[] rows = { 102f, 192f, 283f, 374f };
  for (int r = 0; r < 4; r++) for (int c = 0; c < 3; c++) {
    var t = At($"r{r}c{c}", new Toggle { value = c == 1 }, cols[c] - 7, rows[r] - 8, 16); t.showMixedValue = c == 2; if (r == 3) t.SetEnabled(false);
    if (r == 1 && c == HOV) root.schedule.Execute(() => { var b = t.Q(className: "unity-toggle__input").worldBound; w.wantsMouseMove = true; w.Focus(); w.SendEvent(new Event { type = EventType.MouseMove, mousePosition = b.center }); }).StartingIn(120);
    if (r == 2 && c == FOC) root.schedule.Execute(() => { w.Focus(); t.Focus(); }).StartingIn(250); } }
else if (WHICH == "prefs") {   // regular checkboxes under the UI Viewport toolbar (Toggles.png)
  foreach (var (k, x, y, label, v, m) in new[]{ ("auto", 330f, 176f, "Auto Refresh", true, false), ("mixed", 330f, 200f, "Mixed Setting", false, true),
                                              ("yes", 340f, 216f, "Mixed Setting: Yes", true, false), ("no", 340f, 232f, "Mixed Setting: No", false, false) }) {
    var t = At(k, TG(label, v, m), x, y, 190 - (x - 330)); LW(t, 170 - (x - 330)); } }
root.schedule.Execute(() => {
  var r = new Dictionary<string, Rect>();
  foreach (var kv in named) { r[kv.Key] = kv.Value.worldBound;
    foreach (var (k, cls) in new[]{ ("label", "unity-base-field__label"), ("input", "unity-toggle__input"), ("check", "unity-toggle__checkmark") }) {
      var e = kv.Value.Q(className: cls); if (e != null) r[kv.Key + ":" + k] = e.worldBound; } }
  System.IO.File.WriteAllText("C:/tmp/tg_spec.json", "{" + string.Join(",", r.Select(kv => "\"" + kv.Key + "\":[" + kv.Value.x + "," + kv.Value.y + "," + kv.Value.width + "," + kv.Value.height + "]")) + "}");
}).StartingIn(700);
w.Repaint(); return WHICH;
