// Build a window's pane (⋮) GenericMenu exactly like HostView.PopupGenericMenu and dump it in native_menu.py format to OUT.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection; using System.Text; using System.Collections;
string WIN = "SceneHierarchyWindow"; string OUT = "C:/tmp/pane.txt";
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var view = Resources.FindObjectsOfTypeAll<EditorWindow>().First(w => w.GetType().Name == WIN && w.docked);
var host = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(view);
var menu = new GenericMenu();
(view as IHasCustomMenu)?.AddItemsToMenu(menu);
host.GetType().GetMethod("AddDefaultItemsToMenu", BF).Invoke(host, new object[]{ menu, view });
host.GetType().GetMethod("AddWindowActionMenu", BF)?.Invoke(host, new object[]{ menu, view });
var items = (IList)typeof(GenericMenu).GetField("m_MenuItems", BF).GetValue(menu);
var sb = new StringBuilder();
foreach (var it in items) {
  var t = it.GetType(); var c = (GUIContent)t.GetField("content", BF).GetValue(it);
  bool sep = (bool)t.GetField("separator", BF).GetValue(it);
  bool en = t.GetField("func", BF).GetValue(it) != null || t.GetField("func2", BF).GetValue(it) != null;
  if (sep) { sb.Append("---|" + c.text + (char)10); continue; }
  bool on = (bool)t.GetField("on", BF).GetValue(it);
  sb.Append(c.text + "|" + (en ? (on ? "Checked" : "Normal") : "Disabled") + (char)10);
}
System.IO.File.WriteAllText(OUT, sb.ToString());
return items.Count + " items";
