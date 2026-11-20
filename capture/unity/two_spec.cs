// Two-theme component specimens (light half left / dark half right in the originals): real UI Toolkit controls in a plain host
// window (EFDS.EFDSPopupHost), one half per run in the current skin. Positions are the originals' (pt = half px / 2); where the
// originals mirror the dark half (row labels on the outer side), X(l, d) picks the dark position. States are forced through UI
// Toolkit pseudo-states (Hover 2, Active 1, Focus 64), as in sl_spec.cs. Rects (pt) of named controls to C:/tmp/two_spec.json.
// WHICH: btn_overview | btn_states | btn_tstates | cf_overview | cf_is | cf_color | dd_overview | dd_states | fo_overview | fo_color |
//        fo_states | hb_anatomy | hb_color | label_overview | lv_overview | lv_states | lv_color | nf_overview | nf_states | nf_color |
//        of_overview | of_states | of_color | rb_overview | rb_states
using UnityEditor; using UnityEditor.UIElements; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection;
using System.Collections.Generic;
string WHICH = "btn_overview"; string OF4 = "accept";   // of_states row 4 (blue 'selected' in the original): 'accept' = the drag-accept display class, or a pseudo-state
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
bool D = EditorGUIUtility.isProSkin; float X(float l, float d) => D ? d : l;
var tall = new Dictionary<string, float>{ {"btn_states", 440}, {"btn_tstates", 440}, {"fo_states", 425}, {"rb_states", 425}, {"lv_states", 370}, {"lv_color", 370} };
var size = new Vector2(400, tall.ContainsKey(WHICH) ? tall[WHICH] : 300);
var hostT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSPopupHost")).First(x => x != null);
foreach (var o in Resources.FindObjectsOfTypeAll(hostT).Cast<EditorWindow>().ToList()) o.Close();
var w = (EditorWindow)ScriptableObject.CreateInstance(hostT); w.titleContent = new GUIContent("EFDSPopupHost"); w.ShowUtility(); w.position = new Rect(300, 150, size.x, size.y);
var root = w.rootVisualElement; root.Clear();
var named = new Dictionary<string, VisualElement>();
var ps = typeof(VisualElement).GetProperty("pseudoStates", BF);
Texture2D Icon(string n) => (EditorGUIUtility.Load((D ? "d_" : "") + n) as Texture2D) ?? (EditorGUIUtility.Load(n) as Texture2D);
var folder = EditorGUIUtility.IconContent("FolderOpened Icon").image as Texture2D;
var folderClosed = EditorGUIUtility.IconContent("Folder Icon").image as Texture2D;
var cube = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
var objName = new Mesh { name = "Object Name" }; objName.hideFlags = HideFlags.DontSave;
var col = new Color32(52, 74, 112, 255);
// place: top-left (x, top) or centred (cx, cy); 'left' anchors x and centres vertically on cy
T Put<T>(string key, T e, float x, float cy, float wd = 0, float scale = 1, string anchor = "left") where T : VisualElement {
  e.style.position = Position.Absolute; if (wd > 0) e.style.width = wd; root.Add(e); named[key] = e;
  if (scale != 1) { e.style.scale = new Scale(new Vector3(scale, scale, 1)); e.style.transformOrigin = new TransformOrigin(anchor == "center" ? Length.Percent(50) : 0, Length.Percent(50)); }
  e.RegisterCallback<GeometryChangedEvent>(ev => { var r = e.layout; float nl = anchor == "center" ? x - r.width / 2 : x, nt = anchor == "top" ? cy : cy - r.height / 2;
    if (Mathf.Abs(e.resolvedStyle.left - nl) > 0.01f || Mathf.Abs(e.resolvedStyle.top - nt) > 0.01f) { e.style.left = nl; e.style.top = nt; } });
  return e; }
void LW(VisualElement f, float px) { root.schedule.Execute(() => { var l = f.Q(className: "unity-base-field__label"); if (l != null) { l.style.minWidth = px; l.style.width = px; } }); }
void Set(VisualElement e, int s) { if (e != null) ps.SetValue(e, System.Enum.ToObject(ps.PropertyType, System.Convert.ToInt32(ps.GetValue(e)) | s)); }   // add to the live states (keeps :checked)
// states: "" | hover | active | focus | hoverfocus | activefocus | disabled
void State(VisualElement e, string s, VisualElement target = null) {
  if (s == "") return; if (s == "disabled") { e.SetEnabled(false); return; }
  var t = target ?? e;
  root.schedule.Execute(() => {
    int f = (s.Contains("hover") ? 2 : 0) | (s.Contains("active") ? 1 : 0) | (s.Contains("focus") ? 64 : 0);
    var all = (s.Contains("focus") ? e : t).Query<VisualElement>().ToList();
    foreach (var x in all) Set(x, f);
    if (s.Contains("hover") && !s.Contains("focus")) { var p = t.parent; while (p != null && p != root) { Set(p, 2); p = p.parent; } }
  }).StartingIn(250);
}
VisualElement Input(VisualElement f) => f.Q(className: "unity-base-field__input") ?? f;
Button IconBtn(Texture2D t) { var b = new Button { iconImage = Background.FromTexture2D(t) }; b.style.width = 34; b.style.height = 24;
  b.RegisterCallback<GeometryChangedEvent>(e => { foreach (var im in b.Query<Image>().ToList()) { im.style.width = 16; im.style.height = 16; } }); return b; }
ListView LV(string[] items, float h, bool border) {
  var lv = new ListView(items.ToList(), 16, () => new Label(), (e, i) => ((Label)e).text = items[i]) { showBorder = border, selectionType = SelectionType.Single };
  lv.style.height = h; return lv; }
string[] st4 = { "", "hover", "focus", "disabled" };

if (WHICH == "btn_overview") {
  Put("basic", new Button { text = "Basic Button" }, 200, 89, 0, 1, "center");
  var g = new ToggleButtonGroup { isMultipleSelection = false, allowEmptySelection = false };
  foreach (var n in new[]{ "align_horizontally_left", "align_horizontally_center", "align_horizontally_right" }) g.Add(new Button { iconImage = Background.FromTexture2D(Icon(n)) });
  g.value = ToggleButtonGroupState.CreateFromOptions(new List<bool>{ true, false, false });
  Put("group", g, 200, 151, 0, 1, "center");
  Put("toolbar", new ToolbarButton { text = "Toolbar Button" }, 200, 211, 0, 1, "center");
}
else if (WHICH == "btn_states") {
  string[] t = { "Default", "Hover", "Pressed", "Pressed + Focused", "Focused", "Hover Focused", "Disabled" };
  string[] s = { "", "hover", "active", "activefocus", "focus", "hoverfocus", "disabled" };
  for (int i = 0; i < 7; i++) { var b = Put("b" + i, new Button { text = t[i] }, 140, 51 + 56 * i); State(b, s[i]); }
}
else if (WHICH == "btn_tstates") {
  string[] s = { "", "hover", "", "focus", "focus", "hoverfocus", "disabled" };
  for (int i = 0; i < 7; i++) {
    var g = new ToggleButtonGroup { isMultipleSelection = true, allowEmptySelection = true }; var b = IconBtn(folder); g.Add(b);
    g.value = ToggleButtonGroupState.CreateFromOptions(new List<bool>{ i == 2 || i == 3 });
    Put("b" + i, g, X(240, 160), 51 + 56 * i, 0, 1, "center"); State(b, s[i]); if (s[i] == "disabled") g.SetEnabled(false);
  }
}
else if (WHICH == "cf_overview") { Put("f", new ColorField { value = col }, 200, 150, 160, 1, "center"); }
else if (WHICH == "cf_is") { for (int i = 0; i < 4; i++) { var f = Put("f" + i, new ColorField { value = col }, 120, 92 + 38 * i, 160); State(f, st4[i], Input(f)); } }
else if (WHICH == "cf_color") { Put("f", new ColorField { value = col }, 200, 149, 160, 1, "center"); }
else if (WHICH == "dd_overview") {
  var ch = new List<string>{ "Spot", "Directional", "Point", "Area (Baked Only)" };
  var a = Put("a", new DropdownField("Light Type", ch, 1), 62, 73, 271); LW(a, 109);
  var b = Put("b", new DropdownField("Light Type", ch, 1), 62, 172, 271); LW(b, 109); State(b, "focus");
}
else if (WHICH == "dd_states") {
  string[] t = { "Default", "Hover", "Focused", "Hover Focused", "Disabled" }; string[] s = { "", "hover", "focus", "hoverfocus", "disabled" };
  for (int i = 0; i < 5; i++) { var f = Put("f" + i, new DropdownField(new List<string>{ t[i] }, 0), 120, 49 + 50 * i, 160); State(f, s[i], Input(f)); }
}
else if (WHICH == "fo_overview") {
  Foldout F(bool open) { var f = new Foldout { text = "Lighting", value = open }; f.Q<Toggle>().Q<Label>().style.unityFontStyleAndWeight = FontStyle.Bold; return f; }
  Put("closed", F(false), 46, 104, 300);
  var o = F(true); var dd = new DropdownField("Cast Shadows", new List<string>{ "On", "Off" }, 0); LW(dd, 120); o.Add(dd);
  o.Add(new Toggle("Receive Shadows") { value = true });
  root.schedule.Execute(() => { foreach (var f in o.contentContainer.Children()) { var l = f.Q(className: "unity-base-field__label"); if (l != null) { l.style.minWidth = 122; l.style.width = 122; } } });
  Put("open", o, 46, 149, 304, 1, "top");
}
else if (WHICH == "fo_color") {
  var f = new Foldout { text = "Foldout Label", value = true }; var tg = f.Q<Toggle>(); tg.Q<Label>().style.unityFontStyleAndWeight = FontStyle.Bold;
  var img = new Image { image = folder }; img.style.width = 16; img.style.height = 16; img.style.marginRight = 3;
  var inp = tg.Q(className: "unity-toggle__input"); inp.Insert(1, img);
  Put("f", f, 112, 150, 220, 1.5f);
}
else if (WHICH == "fo_states") {
  for (int r = 0; r < 4; r++) for (int c = 0; c < 2; c++) {
    var f = Put($"f{r}{c}", new Foldout { text = "Foldout", value = c == 0 }, c == 0 ? X(115, 37) : X(243, 167), 105 + 91 * r, 80);
    foreach (var x in f.contentContainer.Children().ToList()) x.RemoveFromHierarchy();
    State(f, st4[r], f.Q<Toggle>());
  }
}
else if (WHICH == "hb_anatomy") {
  var h = new HelpBox("This is a critical error message. Only use when the corresponding error would turn up in the Console log.", HelpBoxMessageType.Error) { buttonText = "Action", linkText = "Hyperlink", linkHref = "https://docs.unity3d.com" };
  Put("h", h, 40, 120, 320, 1, "top");
}
else if (WHICH == "hb_color") {
  var t = new[]{ ("info", "This is an info (log) message.", HelpBoxMessageType.Info), ("warn", "This is a warning message.", HelpBoxMessageType.Warning), ("err", "This is a critical error message.", HelpBoxMessageType.Error) };
  for (int i = 0; i < 3; i++) Put(t[i].Item1, new HelpBox(t[i].Item2, t[i].Item3) { buttonText = "Action", linkText = "Hyperlink", linkHref = "https://docs.unity3d.com" }, 50, 65 + 67 * i, 300, 1, "top");
}
else if (WHICH == "label_overview") {
  var h = Put("h", new Label("Shadow Type"), 131, 79); h.style.unityFontStyleAndWeight = FontStyle.Bold;
  var rows = new[]{ ("Baked Shadow Angle", 146), ("Realtime Shadows", 146), ("Strength", 161), ("Resolution", 161), ("Bias", 161), ("Normal Bias", 161), ("Near Plane", 161) };
  for (int i = 0; i < rows.Length; i++) Put("l" + i, new Label(rows[i].Item1), rows[i].Item2, 99 + 20 * i);
}
else if (WHICH == "lv_overview") {
  if (!D) { var lv0 = Put("list", LV(Enumerable.Range(0, 11).Select(i => i + ". List View Item").ToArray(), 180, true), 50, 60, 300, 1, "top"); lv0.Q<ScrollView>().verticalScrollerVisibility = ScrollerVisibility.Hidden; }
  else { var lv = LV(Enumerable.Range(1, 20).Select(i => i + ". List View Item").ToArray(), 180, true); lv.showFoldoutHeader = true; lv.headerTitle = "List View Foldout";
         lv.showAlternatingRowBackgrounds = AlternatingRowBackground.All; Put("list", lv, 50, 60, 300, 1, "top"); }
}
else if (WHICH == "lv_states" || WHICH == "lv_color") {
  bool color = WHICH == "lv_color";
  if (color) { var a = LV(Enumerable.Repeat("List View Item", 4).ToArray(), 64, false); a.showAlternatingRowBackgrounds = AlternatingRowBackground.All; Put("alt", a, X(160, 80), 81, 160, 1, "top"); }
  float[] ys = color ? new float[]{ 205, 262, 319 } : new float[]{ 88, 147, 204, 261, 318 };
  string[] s = color ? new[]{ "hover", "selected", "focused" } : new[]{ "", "hover", "selected", "focused", "disabled" };
  for (int i = 0; i < ys.Length; i++) {
    var lv = LV(new[]{ "List View Item" }, 16, false); Put("r" + i, lv, X(160, 80), ys[i] - 8, 160, 1, "top");
    if (s[i] == "selected" || s[i] == "focused") lv.SetSelectionWithoutNotify(new[]{ 0 });
    if (s[i] == "hover") root.schedule.Execute(() => { var it = lv.Q(className: "unity-collection-view__item"); var p = it; while (p != null && p != root) { Set(p, 2); p = p.parent; } }).StartingIn(250);
    if (s[i] == "focused") root.schedule.Execute(() => { foreach (var x in lv.Query<VisualElement>().ToList()) Set(x, 64); Set(lv, 64); }).StartingIn(250);
    if (s[i] == "disabled") lv.SetEnabled(false);
  }
}
else if (WHICH == "nf_overview") {
  var d = Put("depth", new IntegerField("Depth") { value = -1 }, 34, 80, 332); LW(d, 116);
  var r = Put("rect", new RectField("Viewport Rect"), 34, 149, 332); LW(r, 116);
  var p = Put("pos", new Vector3Field("Position"), 34, 218, 332); LW(p, 116);
}
else if (WHICH == "nf_states") { for (int i = 0; i < 4; i++) { var f = Put("f" + i, new IntegerField(), 100, 75 + 50 * i, 200); State(f, st4[i], Input(f)); } }
else if (WHICH == "nf_color") { Put("f", new IntegerField(), 200, 149, 200, 1.5f, "center"); }
else if (WHICH == "of_overview") {
  var a = Put("a", new ObjectField("Mesh") { objectType = typeof(Mesh) }, 43, 120, 310); LW(a, 109);
  var b = Put("b", new ObjectField("Mesh") { objectType = typeof(Mesh), value = cube }, 43, 178, 310); LW(b, 109); State(b, "focus");
}
else if (WHICH == "of_states") {
  string[] s = { "", "hover", "focus", OF4, "disabled" };
  for (int i = 0; i < 5; i++) { var f = Put("f" + i, new ObjectField { objectType = typeof(Mesh), value = objName }, 100, 50 + 50 * i, 200);
    if (s[i] == "accept") f.Q(className: "unity-object-field-display")?.AddToClassList("unity-object-field-display--accept-drop"); else State(f, s[i], Input(f)); }
}
else if (WHICH == "of_color") { Put("f", new ObjectField { objectType = typeof(Mesh), value = objName }, 200, 150, 200, 1.5f, "center"); }
else if (WHICH == "rb_overview") {
  var box = new VisualElement(); box.style.position = Position.Absolute; box.style.left = 88; box.style.top = 110; box.style.width = 230; root.Add(box);
  for (int i = 0; i < 4; i++) { var r = new RadioButton("Option " + (i + 1)) { text = "Value", value = i == 1 }; box.Add(r); named["r" + i] = r; r.style.height = 20;
    root.schedule.Execute(() => { var l = r.Q(className: "unity-base-field__label"); l.style.minWidth = 154; l.style.width = 154; }); }
  State(named["r1"], "focus");
}
else if (WHICH == "rb_states") {
  for (int r = 0; r < 4; r++) for (int c = 0; c < 2; c++) {
    var holder = new VisualElement(); holder.style.position = Position.Absolute; holder.style.left = 0; holder.style.top = 0; root.Add(holder);
    var rb = new RadioButton { value = c == 0 }; rb.style.position = Position.Absolute; holder.Add(rb); named[$"r{r}{c}"] = rb;
    float cx = c == 0 ? X(183, 88) : X(310, 215), cy = 105 + 91 * r;
    rb.RegisterCallback<GeometryChangedEvent>(ev => { var L = rb.layout; float nl = cx - L.width / 2, nt = cy - L.height / 2; if (Mathf.Abs(rb.resolvedStyle.left - nl) > 0.01f || Mathf.Abs(rb.resolvedStyle.top - nt) > 0.01f) { rb.style.left = nl; rb.style.top = nt; } });
    State(rb, st4[r], Input(rb));
  }
}
root.schedule.Execute(() => {
  var r = new Dictionary<string, Rect>();
  foreach (var kv in named) { r[kv.Key] = kv.Value.worldBound; var i = kv.Value.Q(className: "unity-base-field__input"); if (i != null) r[kv.Key + ":input"] = i.worldBound;
    var l = kv.Value.Q(className: "unity-base-field__label"); if (l != null) r[kv.Key + ":label"] = l.worldBound;
    var te = kv.Value.Q(className: "unity-text-element--inner-input-field-component") ?? kv.Value.Q<TextElement>(); if (te != null) r[kv.Key + ":text"] = te.worldBound;
    foreach (var (cls, k) in new[]{ ("unity-color-field__eyedropper", "eye"), ("unity-object-field__selector", "sel"), ("unity-toggle__checkmark", "arrow"), ("unity-help-box__icon", "icon"),
                                    ("unity-help-box__button", "button"), ("unity-help-box__link", "link") }) { var e = kv.Value.Q(className: cls); if (e != null) r[kv.Key + ":" + k] = e.worldBound; }
    var im = kv.Value.Q<Image>(); if (im != null) r[kv.Key + ":image"] = im.worldBound; }
  System.IO.File.WriteAllText("C:/tmp/two_spec.json", "{" + string.Join(",", r.Select(kv => "\"" + kv.Key + "\":[" + kv.Value.x + "," + kv.Value.y + "," + kv.Value.width + "," + kv.Value.height + "]")) + "}");
}).StartingIn(700);
w.Repaint(); return WHICH + " " + size;
