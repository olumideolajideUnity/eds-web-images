// Components > Tab specimens (dark): real UI Toolkit TabView / MultiColumnListView / MultiColumnTreeView and an IMGUI toolbar of toggle
// buttons (GUILayout.Toolbar "LargeButton", as the Navigation window) in a plain host window. Rects (pt) to C:/tmp/tb_spec.json.
// WHICH: wtu (800x300; the tree also opens in a docked host titled "Multi-column planet tree" when TREEWIN) | toolbars (800x300) |
//        reorder (800x230; DRAG > 0 drags the 3rd tab left by DRAG pt through window mouse events and leaves it mid-drag)
using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection; using System.Collections.Generic;
string WHICH = "wtu"; bool TREEWIN = false; float DRAG = 0;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var hostT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSPopupHost")).First(x => x != null);
foreach (var o in Resources.FindObjectsOfTypeAll(hostT).Cast<EditorWindow>().ToList()) o.Close();
var named = new Dictionary<string, VisualElement>();
MultiColumnTreeView Planets() {
  int id = 0; TreeViewItemData<(string, int)> P(string n, int pop, params TreeViewItemData<(string, int)>[] k) => new TreeViewItemData<(string, int)>(id++, (n, pop), k.ToList());
  var data = new List<TreeViewItemData<(string, int)>>{ P("Inner planets", 2, P("Mercury", 0), P("Venus", 0), P("Earth", 1), P("Mars", 2)),
                                                      P("Outer planets", 0, P("Jupiter", 0), P("Saturn", 0), P("Uranus", 0), P("Neptune", 0)) };
  var tv = new MultiColumnTreeView { showBorder = false };
  tv.columns.Add(new Column { name = "name", title = "Name", width = 110 }); tv.columns.Add(new Column { name = "pop", title = "Populated?", width = 100 });
  tv.SetRootItems(data);
  tv.columns["name"].makeCell = () => new Label(); tv.columns["name"].bindCell = (e, i) => { var d = tv.GetItemDataForIndex<(string, int)>(i); ((Label)e).text = d.Item1; ((Label)e).style.unityFontStyleAndWeight = tv.GetChildrenIdsForIndex(i).Any() ? FontStyle.Bold : FontStyle.Normal; };
  tv.columns["pop"].makeCell = () => new Toggle(); tv.columns["pop"].bindCell = (e, i) => { var t = (Toggle)e; var p = tv.GetItemDataForIndex<(string, int)>(i).Item2; t.SetValueWithoutNotify(p == 1); t.showMixedValue = p == 2; };
  tv.schedule.Execute(() => tv.ExpandAll()); return tv; }
Tab T(string label, Texture icon = null, bool close = false) { var t = new Tab(label, icon as Texture2D) { closeable = close }; return t; }
if (TREEWIN) {   // docked-style window (own tab strip) holding the planet tree
  var tw = (EditorWindow)ScriptableObject.CreateInstance(hostT); tw.titleContent = new GUIContent("Multi-column planet tree"); tw.Show(); tw.position = new Rect(1000, 150, 280, 182);
  var t = Planets(); t.style.flexGrow = 1; tw.rootVisualElement.Clear(); tw.rootVisualElement.Add(t); return "treewin"; }
var size = WHICH == "reorder" ? new Vector2(800, 230) : new Vector2(800, 300);
var w = (EditorWindow)ScriptableObject.CreateInstance(hostT); w.titleContent = new GUIContent("EFDSPopupHost"); w.ShowUtility(); w.position = new Rect(200, 150, size.x, size.y);
var root = w.rootVisualElement; root.Clear();
T2 At<T2>(string key, T2 e, float x, float y, float wd, float h = -1) where T2 : VisualElement { e.style.position = Position.Absolute; e.style.left = x; e.style.top = y; e.style.width = wd; if (h > 0) e.style.height = h; root.Add(e); named[key] = e; return e; }
string[] TABS = { "All 11", "Project 1", "Hierarchy 0", "Menus 9" };
if (WHICH == "wtu") {
  var tv = new TabView(); foreach (var n in TABS) tv.Add(T(n));
  var rows = AssetDatabase.FindAssets("t:MonoScript", new[]{ "Assets/Atrio" }).Select(AssetDatabase.GUIDToAssetPath).Take(9).ToList();
  var mc = new MultiColumnListView { itemsSource = rows, showBorder = false, showAlternatingRowBackgrounds = AlternatingRowBackground.ContentOnly };
  mc.columns.Add(new Column { name = "label", title = "Label", width = 175 }); mc.columns.Add(new Column { name = "desc", title = "Description", width = 120 });
  mc.columns["label"].makeCell = () => { var r = new VisualElement { style = { flexDirection = FlexDirection.Row, alignItems = Align.Center } }; var im = new Image(); im.style.width = 16; im.style.height = 16; im.style.marginRight = 3; r.Add(im); r.Add(new Label()); return r; };
  mc.columns["label"].bindCell = (e, i) => { e.Q<Image>().image = AssetDatabase.GetCachedIcon(rows[i]); e.Q<Label>().text = System.IO.Path.GetFileName(rows[i]); };
  mc.columns["desc"].makeCell = () => new Label(); mc.columns["desc"].bindCell = (e, i) => ((Label)e).text = rows[i];
  tv[0].Add(mc); mc.style.flexGrow = 1; tv[0].style.flexGrow = 1;
  At("tabview", tv, 61, 76, 365, 186); var tree = At("tree", Planets(), 460, 96, 280, 162); }
else if (WHICH == "toolbars") {
  var tv = new TabView(); foreach (var n in TABS.Concat(new[]{ "Settings 1" })) tv.Add(T(n)); At("tabs", tv, 40, 82, 632, 30);
  int sel = 0; var gui = new IMGUIContainer(() => { sel = GUILayout.Toolbar(sel, new[]{ "Agents", "Areas", "Bake", "Object" }, "LargeButton", GUI.ToolbarButtonSize.FitToContents); });
  At("imgui", gui, 40, 234, 300, 24); }
else {
  var cloud = EditorGUIUtility.IconContent("CloudConnect").image;
  var tv = new TabView { reorderable = true };
  foreach (var (n, c) in new[]{ ("Mountain Region", false), ("Mountain Tops Playlist", false), ("Editor Preview Playlist", true), ("Rainy", false) }) tv.Add(T(n, cloud, c));
  At("tabview", tv, 40, 66, 720, 164);
  if (DRAG > 0) root.schedule.Execute(() => {
    var hdr = tv.Query(className: "unity-tab__header").ToList()[2].worldBound; var p0 = hdr.center; w.Focus();
    w.SendEvent(new Event { type = EventType.MouseDown, mousePosition = p0, button = 0, clickCount = 1 });
    for (int i = 1; i <= 10; i++) w.SendEvent(new Event { type = EventType.MouseDrag, mousePosition = p0 - new Vector2(DRAG * i / 10f, 0), button = 0, delta = new Vector2(-DRAG / 10f, 0) });
  }).StartingIn(300); }
root.schedule.Execute(() => {
  var r = new Dictionary<string, Rect>();
  foreach (var kv in named) { r[kv.Key] = kv.Value.worldBound; int i = 0; foreach (var h in kv.Value.Query(className: "unity-tab__header").ToList()) r[kv.Key + ":tab" + i++] = h.worldBound; }
  System.IO.File.WriteAllText("C:/tmp/tb_spec.json", "{" + string.Join(",", r.Select(kv => "\"" + kv.Key + "\":[" + kv.Value.x + "," + kv.Value.y + "," + kv.Value.width + "," + kv.Value.height + "]")) + "}");
}).StartingIn(800);
w.Repaint(); return WHICH;
