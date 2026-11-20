// Components > Scroll view specimens (dark): real UI Toolkit ScrollViews / a TreeView of the project's own folders and files (with
// horizontalScrollingEnabled, since Unity 7's Project and Hierarchy trees never scroll horizontally). Host 800x300pt. Rects (pt) to C:/tmp/sv_spec.json.
// WHICH: variations (vertical-only and horizontal-only ScrollViews) | tree (TreeView at X,Y,TW,TH) | hover (tree + :hover on the
// vertical scroller's down button). FULL = true sizes the tree to its whole content (no scrolling) for the faded "content container".
using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection; using System.Collections.Generic;
string WHICH = "tree"; float X = 260, Y = 8, TW = 220, TH = 191; bool FULL = false;
string ROOT = "Assets/Atrio/Atrio-Packages"; var PK = ROOT + "/com.unity.building-blocks.gameplay";
string[] CHAIN = { ROOT, PK, PK + "/Playground" };
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var hostT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSPopupHost")).First(x => x != null);
foreach (var o in Resources.FindObjectsOfTypeAll(hostT).Cast<EditorWindow>().ToList()) o.Close();
var w = (EditorWindow)ScriptableObject.CreateInstance(hostT); w.titleContent = new GUIContent("EFDSPopupHost"); w.ShowUtility(); w.position = new Rect(200, 150, 800, 300);
var root = w.rootVisualElement; root.Clear();
var ps = typeof(VisualElement).GetProperty("pseudoStates", BF);
var named = new Dictionary<string, VisualElement>();
T At<T>(string key, T e, float x, float y, float wd, float h) where T : VisualElement { e.style.position = Position.Absolute; e.style.left = x; e.style.top = y; e.style.width = wd; e.style.height = h; root.Add(e); named[key] = e; return e; }
ScrollView SV(ScrollViewMode mode) {
  var sv = new ScrollView(mode) { verticalScrollerVisibility = mode == ScrollViewMode.Horizontal ? ScrollerVisibility.Hidden : ScrollerVisibility.AlwaysVisible,
                                  horizontalScrollerVisibility = mode == ScrollViewMode.Vertical ? ScrollerVisibility.Hidden : ScrollerVisibility.AlwaysVisible };
  sv.AddToClassList("unity-collection-view__scroll-view"); sv.AddToClassList("unity-collection-view--with-border");   // the collection views' own bordered scroll view style
  var c = new VisualElement(); c.style.width = 1200; c.style.height = 1200; sv.Add(c); return sv; }
if (WHICH == "variations") { At("v", SV(ScrollViewMode.Vertical), 50, 29, 300, 241); At("h", SV(ScrollViewMode.Horizontal), 450, 29, 300, 241); }
else {
  int id = 1; var expand = new List<int>();
  TreeViewItemData<string> Node(string path) {
    var kids = new List<TreeViewItemData<string>>(); int my = id++;
    if (CHAIN.Contains(path)) { expand.Add(my);
      foreach (var sub in AssetDatabase.GetSubFolders(path).OrderBy(s => s, System.StringComparer.OrdinalIgnoreCase)) kids.Add(Node(sub));
      foreach (var f in System.IO.Directory.GetFiles(path).Where(f => !f.EndsWith(".meta")).OrderBy(s => s)) kids.Add(new TreeViewItemData<string>(id++, f.Replace('\\', '/'))); }
    else if (AssetDatabase.IsValidFolder(path) && (AssetDatabase.GetSubFolders(path).Length > 0 || System.IO.Directory.GetFiles(path).Any(f => !f.EndsWith(".meta"))))
      kids.Add(new TreeViewItemData<string>(id++, path + "/_"));   // placeholder child so collapsed folders get a foldout arrow
    return new TreeViewItemData<string>(my, path, kids); }
  var tv = new TreeView { horizontalScrollingEnabled = true, showBorder = true };
  tv.SetRootItems(new List<TreeViewItemData<string>>{ Node(ROOT) });
  tv.makeItem = () => { var r = new VisualElement { style = { flexDirection = FlexDirection.Row, alignItems = Align.Center } }; var im = new Image(); im.style.width = 16; im.style.height = 16; im.style.marginRight = 3; im.style.flexShrink = 0; r.Add(im); var l = new Label(); l.style.flexShrink = 0; r.Add(l); return r; };
  tv.bindItem = (e, i) => { var p = tv.GetItemDataForIndex<string>(i); bool open = tv.IsExpanded(tv.GetIdForIndex(i));
    e.Q<Image>().image = AssetDatabase.IsValidFolder(p) ? EditorGUIUtility.IconContent(open ? "FolderOpened Icon" : "Folder Icon").image : AssetDatabase.GetCachedIcon(p);
    var lab = e.Q<Label>(); lab.text = AssetDatabase.IsValidFolder(p) ? System.IO.Path.GetFileName(p) : System.IO.Path.GetFileNameWithoutExtension(p);
    e.style.minWidth = 19 + lab.MeasureTextSize(lab.text, 0, VisualElement.MeasureMode.Undefined, 0, VisualElement.MeasureMode.Undefined).x + 6; };   // row as wide as its text, so the view scrolls horizontally
  At("tree", tv, X, Y, TW, TH);
  root.schedule.Execute(() => { foreach (var x in expand) tv.ExpandItem(x, false, false); tv.Rebuild(); }).StartingIn(50);
  if (FULL) root.schedule.Execute(() => { var cc = tv.Q(className: "unity-scroll-view__content-container"); tv.style.width = cc.layout.width + 40; tv.style.height = cc.layout.height + 40; }).StartingIn(250);
  if (FULL) root.schedule.Execute(() => { var cc = tv.Q(className: "unity-scroll-view__content-container"); tv.style.width = cc.layout.width + 40; tv.style.height = cc.layout.height + 40; }).StartingIn(450);
  if (WHICH == "hover") root.schedule.Execute(() => {
    var low = tv.Q<Scroller>(className: "unity-scroller--vertical")?.Q(className: "unity-scroller__high-button"); if (low != null) ps.SetValue(low, System.Enum.ToObject(ps.PropertyType, 2)); }).StartingIn(300);
}
root.schedule.Execute(() => {
  var r = new Dictionary<string, Rect>();
  foreach (var kv in named) { r[kv.Key] = kv.Value.worldBound;
    foreach (var (k, cls) in new[]{ ("viewport", "unity-scroll-view__content-viewport"), ("content", "unity-scroll-view__content-container"), ("vscroll", "unity-scroller--vertical"), ("hscroll", "unity-scroller--horizontal") }) {
      var e = kv.Value.Q(className: cls); if (e != null && e.resolvedStyle.display == DisplayStyle.Flex && e.worldBound.width > 0) r[kv.Key + ":" + k] = e.worldBound; }
    var vs = kv.Value.Q<Scroller>(className: "unity-scroller--vertical");
    if (vs != null) foreach (var (k, cls) in new[]{ ("up", "unity-scroller__low-button"), ("down", "unity-scroller__high-button"), ("thumb", "unity-base-slider__dragger"), ("groove", "unity-base-slider__tracker") }) {
      var e = vs.Q(className: cls); if (e != null) r[kv.Key + ":v" + k] = e.worldBound; } }
  System.IO.File.WriteAllText("C:/tmp/sv_spec.json", "{" + string.Join(",", r.Select(kv => "\"" + kv.Key + "\":[" + kv.Value.x + "," + kv.Value.y + "," + kv.Value.width + "," + kv.Value.height + "]")) + "}");
}).StartingIn(700);
w.Repaint(); return WHICH;
