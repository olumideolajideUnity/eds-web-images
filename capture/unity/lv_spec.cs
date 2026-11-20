// Components > List view specimens: real UI Toolkit ListView / TreeView (default row heights) in a plain host window, 800 x 300pt, dark.
// WHICH: anatomy | foldout | rowheight. Rects (pt) of lists, their header, first rows and scrollbar go to C:/tmp/lv_spec.json.
using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Collections.Generic;
string WHICH = "anatomy";
var hostT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSPopupHost")).First(x => x != null);
foreach (var o in Resources.FindObjectsOfTypeAll(hostT).Cast<EditorWindow>().ToList()) o.Close();
var w = (EditorWindow)ScriptableObject.CreateInstance(hostT); w.titleContent = new GUIContent("EFDSPopupHost"); w.ShowUtility(); w.position = new Rect(200, 150, 800, 300);
var root = w.rootVisualElement; root.Clear();
var named = new Dictionary<string, VisualElement>();
T At<T>(string key, T e, float x, float y, float wd, float h) where T : VisualElement { e.style.position = Position.Absolute; e.style.left = x; e.style.top = y; e.style.width = wd; e.style.height = h; root.Add(e); named[key] = e; return e; }
ListView LV(int n, int start, bool header, bool numbered = true) {
  var items = Enumerable.Range(start, n).Select(i => numbered ? i + ". List View Item" : "List View Item").ToList();
  var lv = new ListView(items, -1, () => new Label(), (e, i) => ((Label)e).text = items[i]) { showBorder = true, showFoldoutHeader = header, headerTitle = "List View Foldout", showBoundCollectionSize = false };
  return lv; }
var folder = EditorGUIUtility.IconContent("Folder Icon").image as Texture2D; var folderOpen = EditorGUIUtility.IconContent("FolderOpened Icon").image as Texture2D;
if (WHICH == "anatomy") { var lv = At("list", LV(20, 1, true), 180, 38, 440, 280); lv.showAlternatingRowBackgrounds = AlternatingRowBackground.All; }
else if (WHICH == "foldout") { At("plain", LV(20, 0, false), 60, 120, 300, 200); At("fold", LV(20, 1, true), 440, 110, 300, 210); }
else {
  var items = new List<TreeViewItemData<string>>(); int id = 100;
  TreeViewItemData<string> N(string n, params TreeViewItemData<string>[] kids) => new TreeViewItemData<string>(id++, n, kids.ToList());
  items.Add(N("Assets", N("_Recovery"), N("Atrio"), N("EFDS_Screenshots", N("GameplaySequenceDemo", N("Animation"), N("Audio"), N("Character", N("Materials"), N("Models"), N("Textures")))), N("Scenes")));
  var tv = new TreeView { showBorder = true }; tv.SetRootItems(items);
  tv.makeItem = () => { var r = new VisualElement { style = { flexDirection = FlexDirection.Row, alignItems = Align.Center } }; var im = new Image(); im.style.width = 16; im.style.height = 16; im.style.marginRight = 3; r.Add(im); r.Add(new Label()); return r; };
  tv.bindItem = (e, i) => { var kids = tv.GetChildrenIdsForIndex(i).Any(); e.Q<Image>().image = kids && tv.IsExpanded(tv.GetIdForIndex(i)) ? folderOpen : folder; e.Q<Label>().text = tv.GetItemDataForIndex<string>(i); };
  At("tree", tv, 122, 88, 226, 230); root.schedule.Execute(() => { tv.ExpandAll(); tv.Rebuild(); });
  At("list", LV(8, 0, false, false), 452, 88, 226, 230);
}
root.schedule.Execute(() => {
  var r = new Dictionary<string, Rect>();
  foreach (var kv in named) {
    r[kv.Key] = kv.Value.worldBound;
    var rows = kv.Value.Query(className: "unity-collection-view__item").ToList().OrderBy(e => e.worldBound.y).ToList();
    for (int i = 0; i < Mathf.Min(3, rows.Count); i++) r[kv.Key + ":row" + i] = rows[i].worldBound;
    var hd = kv.Value.Q(className: "unity-list-view__foldout-header"); if (hd != null) r[kv.Key + ":header"] = hd.worldBound;
    var sc = kv.Value.Q<Scroller>(className: "unity-scroller--vertical"); if (sc != null && sc.resolvedStyle.display == DisplayStyle.Flex) r[kv.Key + ":scroller"] = sc.worldBound;
  }
  System.IO.File.WriteAllText("C:/tmp/lv_spec.json", "{" + string.Join(",", r.Select(kv => "\"" + kv.Key + "\":[" + kv.Value.x + "," + kv.Value.y + "," + kv.Value.width + "," + kv.Value.height + "]")) + "}");
}).StartingIn(700);
w.Repaint(); return WHICH;
