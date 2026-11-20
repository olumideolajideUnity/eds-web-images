// Components > Multi-column views: the Unity manual's planet examples as real dockable windows (EFDS.EFDSPopupHost shown with Show(),
// no PopupWindowContent) holding a MultiColumnListView or MultiColumnTreeView (Name + 'Populated?' toggle column), W x H pt.
// WHICH: list | tree
using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Collections.Generic;
string WHICH = "list"; float W = 280, H = 205;
var hostT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSPopupHost")).First(x => x != null);
foreach (var o in Resources.FindObjectsOfTypeAll(hostT).Cast<EditorWindow>().ToList()) o.Close();
var w = (EditorWindow)ScriptableObject.CreateInstance(hostT); w.titleContent = new GUIContent(WHICH == "list" ? "Multi-column planet list" : "Multi-column planet tree"); w.Show();
var host = typeof(EditorWindow).GetField("m_Parent", System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic).GetValue(w);
var cw = host.GetType().GetProperty("window").GetValue(host); cw.GetType().GetProperty("position").SetValue(cw, new Rect(300, 200, W, H));
var root = w.rootVisualElement; root.Clear();
string[] inner = { "Mercury", "Venus", "Earth", "Mars" }, outer = { "Jupiter", "Saturn", "Uranus", "Neptune" };
bool Pop(string n) => n == "Earth";
Column Col(string title, float width, System.Func<VisualElement> make) => new Column { name = title, title = title, width = width, makeCell = make, resizable = true };
if (WHICH == "list") {
  var planets = inner.Concat(outer).ToList();
  var v = new MultiColumnListView { itemsSource = planets, showAlternatingRowBackgrounds = AlternatingRowBackground.All };
  v.columns.Add(Col("Name", 120, () => new Label())); v.columns.Add(Col("Populated?", 120, () => new Toggle()));
  v.columns["Name"].bindCell = (e, i) => ((Label)e).text = planets[i];
  v.columns["Populated?"].bindCell = (e, i) => ((Toggle)e).value = Pop(planets[i]);
  v.style.flexGrow = 1; root.Add(v);
} else {
  int id = 0;
  TreeViewItemData<string> G(string n, string[] kids) { var gid = id++; return new TreeViewItemData<string>(gid, n, kids.Select(k => new TreeViewItemData<string>(id++, k)).ToList()); }
  var items = new List<TreeViewItemData<string>>{ G("Inner planets", inner), G("Outer planets", outer) };
  var v = new MultiColumnTreeView(); v.SetRootItems(items);
  v.columns.Add(Col("Name", 120, () => new Label())); v.columns.Add(Col("Populated?", 120, () => new Toggle()));
  v.columns["Name"].bindCell = (e, i) => { var n = v.GetItemDataForIndex<string>(i); ((Label)e).text = n; e.style.unityFontStyleAndWeight = n.EndsWith("planets") ? FontStyle.Bold : FontStyle.Normal; };
  v.columns["Populated?"].bindCell = (e, i) => { var n = v.GetItemDataForIndex<string>(i); var t = (Toggle)e;
    if (n == "Inner planets") { t.value = false; t.showMixedValue = true; } else { t.showMixedValue = false; t.value = Pop(n); } };
  v.style.flexGrow = 1; root.Add(v); root.schedule.Execute(() => { v.ExpandAll(); v.Rebuild(); });
}
w.Repaint(); return WHICH;
