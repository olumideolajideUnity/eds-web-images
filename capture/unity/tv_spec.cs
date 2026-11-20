// Components > Tree view state specimens: one-item UI Toolkit TreeViews ("Tree View Item" with a folder icon and a child, so it has a
// foldout) in a plain host window (400 x H pt, one theme half). Rects (pt) to C:/tmp/tv_spec.json.
// WHICH: states (400x370: columns expanded / collapsed, rows default / hover / selected / focused / disabled) |
//        color (400x300: one column, rows default / hover / selected / focused). HOV / FOC = cell index that gets real hover / real focus.
using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Collections.Generic;
string WHICH = "states"; int HOV = -1, FOC = -1;
var hostT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSPopupHost")).First(x => x != null);
foreach (var o in Resources.FindObjectsOfTypeAll(hostT).Cast<EditorWindow>().ToList()) o.Close();
var w = (EditorWindow)ScriptableObject.CreateInstance(hostT); w.titleContent = new GUIContent("EFDSPopupHost"); w.ShowUtility(); w.position = new Rect(200, 150, 400, WHICH == "states" ? 370 : 300);
var root = w.rootVisualElement; root.Clear();
bool pro = EditorGUIUtility.isProSkin;
var folder = EditorGUIUtility.IconContent("Folder Icon").image; var open = EditorGUIUtility.IconContent("FolderOpened Icon").image;
var named = new Dictionary<string, VisualElement>();
TreeView Item(bool expanded) {
  var tv = new TreeView { showBorder = true, fixedItemHeight = 20 };
  tv.SetRootItems(new List<TreeViewItemData<string>>{ new TreeViewItemData<string>(1, "Tree View Item", new List<TreeViewItemData<string>>{ new TreeViewItemData<string>(2, "Child") }) });
  tv.makeItem = () => { var r = new VisualElement { style = { flexDirection = FlexDirection.Row, alignItems = Align.Center } }; var im = new Image(); im.style.width = 16; im.style.height = 16; im.style.marginRight = 3; r.Add(im); r.Add(new Label()); return r; };
  tv.bindItem = (e, i) => { e.Q<Image>().image = expanded ? open : folder; e.Q<Label>().text = tv.GetItemDataForIndex<string>(i); };
  if (expanded) tv.schedule.Execute(() => tv.ExpandItem(1));
  return tv; }
var cells = new List<(string key, float x, float y, bool exp, string state)>();
if (WHICH == "states") {
  float[] xs = pro ? new[]{ 28f, 179f } : new[]{ 96f, 246f }; float[] ys = { 80f, 138f, 196f, 252f, 309f }; string[] st = { "default", "hover", "selected", "focused", "disabled" };
  for (int r = 0; r < 5; r++) for (int c = 0; c < 2; c++) cells.Add(($"r{r}c{c}", xs[c], ys[r], c == 0, st[r])); }
else { float[] ys = { 57f, 121f, 185f, 249f }; string[] st = { "default", "hover", "selected", "focused" }; for (int r = 0; r < 4; r++) cells.Add(($"r{r}c0", 138f, ys[r], false, st[r])); }
for (int i = 0; i < cells.Count; i++) {
  var (key, x, y, exp, state) = cells[i]; var tv = Item(exp);
  tv.style.position = Position.Absolute; tv.style.left = x; tv.style.top = y - 11; tv.style.width = 140; tv.style.height = 23; tv.style.minHeight = 23; tv.style.maxHeight = 23;
  var sv = tv.Q<ScrollView>(); if (sv != null) { sv.verticalScrollerVisibility = ScrollerVisibility.Hidden; sv.horizontalScrollerVisibility = ScrollerVisibility.Hidden; }   // one row per cell
  root.Add(tv); named[key] = tv; int idx = i;
  if (state == "disabled") tv.SetEnabled(false);
  if (state == "selected" || state == "focused") tv.schedule.Execute(() => tv.SetSelectionWithoutNotify(new[]{ 0 })).StartingIn(100);
  if (state == "focused" && FOC == idx) root.schedule.Execute(() => { w.Focus(); tv.Q(className: "unity-collection-view__scroll-view")?.Focus(); }).StartingIn(300);
  if (state == "hover" && HOV == idx) root.schedule.Execute(() => { var row = tv.Q(className: "unity-collection-view__item"); w.wantsMouseMove = true; w.Focus();
    w.SendEvent(new Event { type = EventType.MouseMove, mousePosition = row.worldBound.center }); }).StartingIn(200); }
root.schedule.Execute(() => {
  var r = new Dictionary<string, Rect>();
  foreach (var kv in named) { r[kv.Key] = kv.Value.worldBound; var row = kv.Value.Q(className: "unity-collection-view__item"); if (row != null) r[kv.Key + ":row"] = row.worldBound;
    var tg = kv.Value.Q(className: "unity-tree-view__item-toggle"); if (tg != null) r[kv.Key + ":toggle"] = tg.worldBound;
    var im = kv.Value.Q<Image>(); if (im != null) r[kv.Key + ":icon"] = im.worldBound; var lb = kv.Value.Q<Label>(); if (lb != null) r[kv.Key + ":label"] = lb.worldBound; }
  System.IO.File.WriteAllText("C:/tmp/tv_spec.json", "{" + string.Join(",", r.Select(kv => "\"" + kv.Key + "\":[" + kv.Value.x + "," + kv.Value.y + "," + kv.Value.width + "," + kv.Value.height + "]")) + "}");
}).StartingIn(700);
w.Repaint(); return WHICH;
