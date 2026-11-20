// Fundamentals > Interactions state specimens: real UI Toolkit controls in a plain host window (EFDS.EFDSPopupHost, no content),
// one theme half (400pt wide) of the original, with each control's interaction state forced through UI Toolkit's own pseudo-states
// (Hover / Active / Focus / Checked / Disabled), so Unity's stylesheets draw the real state. No mouse pointer is involved.
// WHICH: buttons | hover | focus | disabled | pressed | checked | highlighted. Rows are placed at the original's centre y (pt).
using UnityEditor; using UnityEditor.UIElements; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection;
using System.Collections.Generic;
string WHICH = "buttons";
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var H = new Dictionary<string, float>{ {"buttons",364},{"hover",300},{"focus",300},{"disabled",300},{"pressed",260},{"checked",260},{"highlighted",300} }[WHICH];
var hostT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSPopupHost")).First(x => x != null);
foreach (var o in Resources.FindObjectsOfTypeAll(hostT).Cast<EditorWindow>().ToList()) o.Close();
var w = (EditorWindow)ScriptableObject.CreateInstance(hostT); w.titleContent = new GUIContent("EFDSPopupHost"); w.ShowUtility(); w.position = new Rect(400, 200, 400, H);
var root = w.rootVisualElement; root.Clear();
var ps = typeof(VisualElement).GetProperty("pseudoStates", BF); var psT = ps.PropertyType;
void State(VisualElement e, int s, bool deep = true) { if (e == null) return; ps.SetValue(e, System.Enum.ToObject(psT, (int)System.Convert.ToInt32(ps.GetValue(e)) | s)); if (deep) foreach (var c in e.Children()) State(c, s, true); }
const int ACTIVE = 1, HOVER = 2, CHECKED = 8, FOCUS = 64;
VisualElement Row(float cy, float h, params VisualElement[] items) {
  var r = new VisualElement(); r.style.position = Position.Absolute; r.style.left = 0; r.style.right = 0; r.style.top = cy - h / 2; r.style.height = h;
  r.style.flexDirection = FlexDirection.Row; r.style.justifyContent = Justify.Center; r.style.alignItems = Align.Center;
  foreach (var i in items) r.Add(i); root.Add(r); return r; }
VisualElement Box(float top, float h, VisualElement child) {     // fixed 200pt-wide box (x 100..300) at the original's top
  var r = new VisualElement(); r.style.position = Position.Absolute; r.style.left = 100; r.style.width = 200; r.style.top = top; r.style.height = h;
  child.style.flexGrow = 1; r.Add(child); root.Add(r); return r; }
ListView List(string title) {
  var items = Enumerable.Range(1, 8).Select(i => i + ". List View Item").ToList();
  var lv = new ListView(items, -1, () => new Label(), (e, i) => ((Label)e).text = items[i]) { showFoldoutHeader = true, headerTitle = title, showBorder = true, showBoundCollectionSize = false };
  var sv = lv.Q<ScrollView>(); sv.style.height = 86; sv.style.flexGrow = 0;
  return lv; }
var after = new List<System.Action>();
var sched = new List<System.Action>();
if (WHICH == "buttons") {
  (string t, int s, bool en)[] B = { ("Default", 0, true), ("Hover", HOVER, true), ("Pressed", ACTIVE, true), ("Pressed + Focused", ACTIVE | FOCUS, true),
    ("Focused", FOCUS, true), ("Hover Focused", HOVER | FOCUS, true), ("Disabled", 0, false) };
  for (int i = 0; i < B.Length; i++) { var b = new Button { text = B[i].t }; b.SetEnabled(B[i].en); Row(50 + 44 * i, 24, b); var s = B[i].s; var bb = b; if (s != 0) after.Add(() => State(bb, s, false)); }
} else if (WHICH == "hover" || WHICH == "focus") {
  int s = WHICH == "hover" ? HOVER : FOCUS;
  var f = new Foldout { text = "Foldout", value = false }; Box(85 - 10, 20, f);
  var tf = new TextField { value = "Input field" }; Box(143 - 10, 20, tf);
  var lv = List("List View Foldout"); Box(191, 108, lv);
  after.Add(() => { State(f.Q<Toggle>(), s); State(tf, s); });
  if (WHICH == "focus") after.Add(() => { lv.selectedIndex = 1; State(lv, FOCUS, false); State(lv.Q<ScrollView>(), FOCUS, false); });
  sched.Add(() => { var rows = lv.Query(className: "unity-list-view__item").ToList().OrderBy(r => r.worldBound.y).ToList(); if (rows.Count > 1 && WHICH == "hover") State(rows[1], HOVER); if (WHICH == "focus") { State(lv.Q(className: "unity-collection-view__scroll-view"), FOCUS, false); foreach (var r in rows) State(r, FOCUS, false); } });
} else if (WHICH == "disabled") {
  var b = new Button { text = "Button label" }; b.SetEnabled(false); Box(85 - 10, 20, b);
  var tf = new TextField { value = "Input field" }; tf.SetEnabled(false); Box(143 - 10, 20, tf);
  var cf = new ColorField { value = new Color32(100, 120, 170, 255), showEyeDropper = true }; cf.SetEnabled(false); Box(199 - 10, 20, cf);
} else if (WHICH == "pressed") {
  Texture2D T(string n) => (EditorGUIUtility.Load((EditorGUIUtility.isProSkin ? "d_" : "") + n) as Texture2D) ?? (EditorGUIUtility.Load(n) as Texture2D);
  var g = new ToggleButtonGroup { isMultipleSelection = false, allowEmptySelection = false };
  foreach (var n in new[]{ "align_horizontally_left", "align_horizontally_center", "align_horizontally_right" }) g.Add(new Button { iconImage = Background.FromTexture2D(T(n)) });
  g.value = ToggleButtonGroupState.CreateFromOptions(new List<bool>{ true, false, false }); Row(90, 24, g);
  var p = new Button { text = "Pressed" }; Row(148, 24, p); after.Add(() => State(p, ACTIVE, false));
  var g2 = new ToggleButtonGroup { isMultipleSelection = false, allowEmptySelection = false };
  foreach (var n in new[]{ "Agents", "Areas", "Bake", "Object" }) g2.Add(new Button { text = n });
  g2.value = ToggleButtonGroupState.CreateFromOptions(new List<bool>{ false, false, true, false }); Row(206, 24, g2);
} else if (WHICH == "checked") {
  var t1 = new Toggle { value = false }; var t2 = new Toggle { value = true }; t2.style.marginLeft = 28;
  var rr = Row(40, 20, t1, t2); rr.style.justifyContent = Justify.FlexStart; rr.style.left = 80;
  var g = new ToggleButtonGroup { isMultipleSelection = true, allowEmptySelection = true };
  foreach (var n in new[]{ "Toggle", "Checked toggle", "Toggle" }) g.Add(new Button { text = n });
  g.value = ToggleButtonGroupState.CreateFromOptions(new List<bool>{ false, true, false });
  var r2 = Row(97, 24, g); r2.style.justifyContent = Justify.FlexStart; r2.style.left = 80;
} else if (WHICH == "highlighted") {
  var lv = List("List View Foldout"); Box(191, 108, lv);
  after.Add(() => { lv.selectedIndex = 1; });
}
root.schedule.Execute(() => { foreach (var sv in root.Query<ScrollView>().ToList()) sv.scrollOffset = Vector2.zero; foreach (var lv in root.Query<ListView>().ToList()) lv.ScrollToItem(0); foreach (var a in after) a(); }).StartingIn(200);
root.schedule.Execute(() => { foreach (var a in sched) a(); w.Repaint(); }).StartingIn(600);
w.Repaint();
return WHICH + " pro=" + EditorGUIUtility.isProSkin;
