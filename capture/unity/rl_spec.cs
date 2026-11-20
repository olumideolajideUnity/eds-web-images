// Components > Reorderable list specimens (dark): real IMGUI ReorderableLists (in IMGUIContainers) and real UI Toolkit reorderable
// ListViews in a plain host window. Rects (pt) to C:/tmp/rl_spec.json.
// WHICH: overview (400x300, dark half) | anatomy (800x300) | uitk (800x300) | spacing (800x300, zoomed) |
//        states (400x370, dark half; FOCUS true/false picks whether the UI Toolkit list has keyboard focus)
using UnityEditor; using UnityEditorInternal; using UnityEditor.UIElements; using UnityEngine; using UnityEngine.UIElements;
using System.Linq; using System.Reflection; using System.Collections.Generic;
string WHICH = "overview"; bool FOCUS = true;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var hostT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSPopupHost")).First(x => x != null);
foreach (var o in Resources.FindObjectsOfTypeAll(hostT).Cast<EditorWindow>().ToList()) o.Close();
var size = WHICH == "overview" ? new Vector2(400, 300) : WHICH == "states" ? new Vector2(400, 370) : new Vector2(800, 300);
var w = (EditorWindow)ScriptableObject.CreateInstance(hostT); w.titleContent = new GUIContent("EFDSPopupHost"); w.ShowUtility(); w.position = new Rect(200, 150, size.x, size.y);
var root = w.rootVisualElement; root.Clear();
var ps = typeof(VisualElement).GetProperty("pseudoStates", BF);
var named = new Dictionary<string, VisualElement>(); var imgui = new Dictionary<string, ReorderableList>();
var mesh = new Mesh { name = "Object Name" };
var el = new Dictionary<string, Rect>();   // IMGUI element / list rects (local pt, recorded on repaint)
T At<T>(string key, T e, float x, float y, float wd, float scale = 1) where T : VisualElement {
  e.style.position = Position.Absolute; e.style.left = x; e.style.top = y; e.style.width = wd;
  if (scale != 1) { e.style.scale = new Scale(new Vector3(scale, scale, 1)); e.style.transformOrigin = new TransformOrigin(0, 0); }
  root.Add(e); named[key] = e; return e; }

// IMGUI ReorderableList. labels: row label text; obj: draw an ObjectField after the label; bg: default background
// sel: rows really selected (multiSelect)
ReorderableList RL(string key, string[] labels, bool obj, bool bg = true, string header = "List header", HashSet<int> sel = null, HashSet<int> foc = null, float labelW = 0) {
  var rl = new ReorderableList(labels.ToList(), typeof(string), true, true, true, true) { showDefaultBackground = bg };
  rl.drawHeaderCallback = r => EditorGUI.LabelField(r, header);
  rl.drawElementCallback = (r, i, a, f) => {
    if (Event.current.type == EventType.Repaint) el[key + ":el" + i] = r;
    r.height = EditorGUIUtility.singleLineHeight; r.y += 1;
    if (!obj) { EditorGUI.LabelField(r, labels[i]); return; }
    var lw = labelW > 0 ? labelW : r.width * 0.42f;
    EditorGUI.LabelField(new Rect(r.x, r.y, lw, r.height), labels[i]);
    EditorGUI.ObjectField(new Rect(r.x + lw, r.y, r.width - lw, r.height), mesh, typeof(Mesh), false); };
  if (sel != null) { rl.multiSelect = true; bool first = true; foreach (var i in sel) { rl.Select(i, !first); first = false; } }   // real selection
  imgui[key] = rl; return rl; }
IMGUIContainer Host(ReorderableList rl, bool grab = false) => new IMGUIContainer(() => { if (grab) rl.GrabKeyboardFocus(); var r = GUILayoutUtility.GetRect(10, rl.GetHeight(), GUILayout.ExpandWidth(true)); rl.DoList(r); if (Event.current.type == EventType.Repaint) el["list"] = r; });

// UI Toolkit reorderable ListView with label + ObjectField rows
ListView LV(string[] items, bool header, bool handles = true, bool alt = false, bool border = true, bool objField = true, string title = "List header") {
  var src = items.ToList();
  var lv = new ListView(src, -1, () => { var r = new VisualElement { style = { flexDirection = FlexDirection.Row, alignItems = Align.Center, flexGrow = 1 } };
      var l = new Label(); l.style.width = Length.Percent(44); l.style.unityTextAlign = TextAnchor.MiddleLeft; r.Add(l);
      if (objField) { var of = new ObjectField { objectType = typeof(Mesh), value = mesh }; of.style.flexGrow = 1; of.style.marginRight = 2; r.Add(of); } return r; },
    (e, i) => e.Q<Label>().text = src[i]) {
    reorderable = true, reorderMode = handles ? ListViewReorderMode.Animated : ListViewReorderMode.Simple, showAddRemoveFooter = true,
    showBorder = border, showFoldoutHeader = header, headerTitle = title, showBoundCollectionSize = false,
    showAlternatingRowBackgrounds = alt ? AlternatingRowBackground.All : AlternatingRowBackground.None };
  return lv; }
string[] Items(string p, int n) => Enumerable.Range(1, n).Select(i => p + " " + i).ToArray();

if (WHICH == "overview") { At("list", Host(RL("list", Items("Element", 3), true, labelW: 103)), 51, 93, 297); }
else if (WHICH == "anatomy") {
  At("bg", Host(RL("bg", Items("Item", 8), false)), 86, 38, 279);
  At("nobg", Host(RL("nobg", Items("Item", 8), false, bg: false)), 470, 38, 275); }
else if (WHICH == "uitk") {
  At("tl", LV(Items("Item", 4), true), 96, 22, 249); At("tr", LV(Items("Item", 4), true, alt: true), 482, 22, 249);
  At("bl", LV(Items("Item", 3), false, border: false), 96, 182, 249); At("br", LV(Items("Item", 3), false, handles: false, alt: true), 482, 182, 249); }
else if (WHICH == "spacing") {
  At("imgui", Host(RL("imgui", Items("Item", 4), false, header: "List Header")), 47, 34, 194, 1.75f);
  At("uitk", LV(Items("Item", 4), true, objField: false, title: "List Header"), 413, 34, 194, 1.75f); }
else {   // states: IMGUI (Item 2 selected, Item 3 selected + focused) and UI Toolkit (alternate, selected, hover, focused)
  At("imgui", Host(RL("imgui", Items("Item", 3), false, sel: new HashSet<int>{ 1, 2 }), FOCUS), 110, 31, 170);   // FOCUS: list has keyboard focus (blue) or not (grey)
  var lv = At("uitk", LV(Items("Item", 5), true, alt: true, objField: false, title: "List Header"), 110, 200, 170);
  root.schedule.Execute(() => {
    lv.SetSelection(new[]{ 2, 4 });
    w.Focus(); if (FOCUS) lv.Q(className: "unity-collection-view__scroll-view")?.Focus();
    var rows = lv.Query(className: "unity-collection-view__item").ToList().OrderBy(e => e.worldBound.y).ToList();
    if (rows.Count > 3) ps.SetValue(rows[3], System.Enum.ToObject(ps.PropertyType, 2));   // hover on Item 4
  }).StartingIn(250);
}
root.schedule.Execute(() => {
  var r = new Dictionary<string, Rect>();
  foreach (var kv in named) { r[kv.Key] = kv.Value.worldBound;
    var rows = kv.Value.Query(className: "unity-collection-view__item").ToList().OrderBy(e => e.worldBound.y).ToList();
    for (int i = 0; i < rows.Count; i++) r[kv.Key + ":row" + i] = rows[i].worldBound;
    foreach (var (k, cls) in new[]{ ("header", "unity-list-view__foldout-header"), ("footer", "unity-list-view__footer"), ("scroll", "unity-collection-view__scroll-view"), ("content", "unity-scroll-view__content-container") }) {
      var e = kv.Value.Q(className: cls); if (e != null) r[kv.Key + ":" + k] = e.worldBound; } }
  foreach (var kv in imgui) { var rl = kv.Value; var b = named[kv.Key].worldBound; var s = named[kv.Key].resolvedStyle.scale.value.x;
    foreach (var e in el) if (e.Key.StartsWith(kv.Key + ":")) r[e.Key] = e.Value;
    r[kv.Key + ":metrics"] = new Rect(rl.headerHeight, rl.footerHeight, rl.elementHeight, rl.GetHeight()); r[kv.Key + ":scale"] = new Rect(s, 0, 0, 0); }
  System.IO.File.WriteAllText("C:/tmp/rl_spec.json", "{" + string.Join(",", r.Select(kv => "\"" + kv.Key + "\":[" + kv.Value.x + "," + kv.Value.y + "," + kv.Value.width + "," + kv.Value.height + "]")) + "}");
}).StartingIn(600);
w.Repaint(); return WHICH;
