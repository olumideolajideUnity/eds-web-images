// Components > Button specimens in a plain host window (EFDS.EFDSPopupHost), dark skin. IMGUI parts use Unity's own button styles
// (miniButton, button, LargeButton, toolbarbutton, ButtonLeft/Mid/Right); UI Toolkit parts are real Button / ToolbarButton /
// ToggleButtonGroup. Zoomed specimens use style.scale (vector re-render, stays crisp). Reports rects (pt) to C:/tmp/btn_spec.json.
// WHICH: variations (800x300) | basic (420x100) | toggleDe (420x100) | sizes (400x300) | grouping (400x300) | export (800x300)
//        | anatomy (800x300) | basic_theme | toggle_theme | toolbar_theme (800x300)
using UnityEditor; using UnityEditor.UIElements; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection;
using System.Collections.Generic;
string WHICH = "variations";
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var sizes = new Dictionary<string, Vector2>{ {"variations", new Vector2(800,300)}, {"basic", new Vector2(420,100)}, {"toggleDe", new Vector2(420,100)}, {"sizes", new Vector2(800,300)},
  {"grouping", new Vector2(800,300)}, {"export", new Vector2(800,300)}, {"layout", new Vector2(800,300)}, {"empty", new Vector2(413,190)}, {"anatomy", new Vector2(800,300)}, {"basic_theme", new Vector2(800,300)}, {"toggle_theme", new Vector2(800,300)}, {"toolbar_theme", new Vector2(800,300)} };
var size = sizes[WHICH];
var hostT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSPopupHost")).First(x => x != null);
foreach (var o in Resources.FindObjectsOfTypeAll(hostT).Cast<EditorWindow>().ToList()) o.Close();
var w = (EditorWindow)ScriptableObject.CreateInstance(hostT); w.titleContent = new GUIContent("EFDSPopupHost"); w.ShowUtility(); w.position = new Rect(300, 150, size.x, size.y);
var root = w.rootVisualElement; root.Clear();
var rects = new Dictionary<string, Rect>(); var named = new Dictionary<string, VisualElement>();
Texture2D Icon(string n) => (EditorGUIUtility.Load((EditorGUIUtility.isProSkin ? "d_" : "") + n) as Texture2D) ?? (EditorGUIUtility.Load(n) as Texture2D);
var folder = EditorGUIUtility.IconContent("FolderOpened Icon").image as Texture2D;
string[] alignIcons = { "align_horizontally_left", "align_horizontally_center", "align_horizontally_right" };
// center an element at (cx, cy) pt, optionally scaled
T C<T>(string key, T e, float cx, float cy, float scale = 1) where T : VisualElement {
  var holder = new VisualElement(); holder.style.position = Position.Absolute; holder.style.left = 0; holder.style.top = 0; holder.style.width = size.x; holder.style.height = size.y;
  holder.pickingMode = PickingMode.Ignore; root.Add(holder);
  e.style.position = Position.Absolute; holder.Add(e); named[key] = e;
  e.RegisterCallback<GeometryChangedEvent>(ev => { var r = e.layout; var nl = cx - r.width / 2; var nt = cy - r.height / 2; if (Mathf.Abs(e.resolvedStyle.left - nl) > 0.01f || Mathf.Abs(e.resolvedStyle.top - nt) > 0.01f) { e.style.left = nl; e.style.top = nt; } });
  if (scale != 1) { e.style.scale = new Scale(new Vector3(scale, scale, 1)); e.style.transformOrigin = new TransformOrigin(Length.Percent(50), Length.Percent(50)); }
  return e; }
Button IconBtn(Texture2D t, float wpt = 34, float hpt = 24) { var b = new Button { iconImage = Background.FromTexture2D(t) }; b.style.width = wpt; b.style.height = hpt;
  b.RegisterCallback<GeometryChangedEvent>(e => { foreach (var im in b.Query<Image>().ToList()) { im.style.width = 16; im.style.height = 16; } }); return b; }
ToggleButtonGroup Group(string[] texts, bool[] on, bool multi, bool icons) {
  var g = new ToggleButtonGroup { isMultipleSelection = multi, allowEmptySelection = multi };
  foreach (var t in texts) g.Add(icons ? new Button { iconImage = Background.FromTexture2D(Icon(t)) } : new Button { text = t });
  g.value = ToggleButtonGroupState.CreateFromOptions(on.ToList()); return g; }
// IMGUI: draw a list of (key, rect, content, style, on) each repaint, recording rects
var imgui = new List<(string key, Rect r, GUIContent c, string style, bool on, bool focus)>();
var imc = new IMGUIContainer(() => {
  foreach (var it in imgui) {
    var st = new GUIStyle(it.style);
    if (Event.current.type == EventType.Repaint) st.Draw(it.r, it.c, false, false, it.on, it.focus);
    rects[it.key] = it.r; } });
imc.style.position = Position.Absolute; imc.style.left = 0; imc.style.top = 0; imc.style.width = size.x; imc.style.height = size.y; root.Add(imc);
Rect CR(float cx, float cy, string style, GUIContent c, float fixedW = 0) { var st = new GUIStyle(style); var sz = st.CalcSize(c); if (fixedW > 0) sz.x = fixedW; var h = st.fixedHeight > 0 ? st.fixedHeight : Mathf.Round(st.CalcHeight(new GUIContent("Ag"), sz.x)); return new Rect(Mathf.Round(cx - sz.x / 2), Mathf.Round(cy - h / 2), Mathf.Round(sz.x), h); }
void IM(string key, float cx, float cy, string style, GUIContent c, bool on = false, float fixedW = 0, bool focus = false) => imgui.Add((key, CR(cx, cy, style, c, fixedW), c, style, on, focus));
if (WHICH == "variations") {
  C("add", new Button { text = "Add Component" }, 200, 121);
  IM("mute", 200, 180, "toolbarbutton", new GUIContent("Mute Audio"));
  C("biu", Group(new[]{ "B", "I", "U", "S" }, new[]{ true, false, true, false }, true, false), 600, 77);
  IM("al0", 578, 227, "ButtonLeft", new GUIContent(Icon(alignIcons[0])), true, 24); IM("al1", 600, 227, "ButtonMid", new GUIContent(Icon(alignIcons[1])), false, 22); IM("al2", 622, 227, "ButtonRight", new GUIContent(Icon(alignIcons[2])), false, 24);
} else if (WHICH == "basic") {
  C("basic", new Button { text = "Basic Button" }, 132, 50, 1.25f); C("toolbar", new ToolbarButton { text = "Toolbar Button" }, 289, 50, 1.25f);
} else if (WHICH == "toggleDe") {
  C("label", new Label("Alignment"), 144, 50, 1.25f); C("group", Group(alignIcons, new[]{ true, false, false }, false, true), 268, 50, 1.25f);
} else if (WHICH == "sizes") {
  IM("mini", 337, 88, "miniButton", new GUIContent("Mini Button")); IM("default", 337, 147, "button", new GUIContent("Default Button")); IM("large", 337, 208, "LargeButton", new GUIContent("Large Button"));
  IM("mini_i", 500, 88, "miniButton", new GUIContent(folder), false, 24); IM("default_i", 500, 147, "button", new GUIContent(folder), false, 28); IM("large_i", 500, 208, "LargeButton", new GUIContent(folder), false, 34);
} else if (WHICH == "grouping") {
  foreach (var (k, x, s) in new[]{ ("sl", 281f, "Leftmost"), ("sm", 400f, "Middle"), ("sr", 519f, "Rightmost") }) IM(k, x, 112, "button", new GUIContent(s), false, 78);
  foreach (var (k, x, s, st) in new[]{ ("gl", 281f, "Leftmost", "ButtonLeft"), ("gm1", 360f, "Middle", "ButtonMid"), ("gm2", 439f, "Middle", "ButtonMid"), ("gr", 518f, "Rightmost", "ButtonRight") }) IM(k, x, 190, st, new GUIContent(s), false, 79);
} else if (WHICH == "export") {
  var eb = C("export", new Button { text = "Export Coordinates..." }, 173, 130);
  root.schedule.Execute(() => { var ps = typeof(VisualElement).GetProperty("pseudoStates", BF); ps.SetValue(eb, System.Enum.ToObject(ps.PropertyType, 64)); }).StartingIn(150);   // focused
} else if (WHICH == "layout") {
  IM("mini", 390, 88, "miniButton", new GUIContent("Mini Button")); IM("default", 390, 147, "button", new GUIContent("Default Button")); IM("large", 390, 208, "LargeButton", new GUIContent("Large Button"));
} else if (WHICH == "anatomy") {
  C("large", new Button { text = "Large Button" }, 282, 125, 2.2f); C("icon", IconBtn(folder), 472, 125, 2.2f);
} else if (WHICH == "basic_theme") {
  C("b", new Button { text = "Basic Button" }, 400, 125, 2.4f);
} else if (WHICH == "toggle_theme") {
  var tg = new ToggleButtonGroup { isMultipleSelection = true, allowEmptySelection = true }; tg.Add(IconBtn(folder)); C("b", tg, 400, 125, 2.4f);
} else if (WHICH == "toolbar_theme") {
  C("b", new ToolbarButton { text = "Toolbar Button" }, 400, 125, 2.4f);
}
root.schedule.Execute(() => {
  foreach (var kv in named) { var e = kv.Value; var wb = e.worldBound; rects[kv.Key] = wb; }
  System.IO.File.WriteAllText("C:/tmp/btn_spec.json", "{" + string.Join(",", rects.Select(kv => "\"" + kv.Key + "\":[" + kv.Value.x + "," + kv.Value.y + "," + kv.Value.width + "," + kv.Value.height + "]")) + "}");
}).StartingIn(400);
w.Repaint();
return WHICH;
