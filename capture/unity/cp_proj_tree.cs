// Color palette > Window borders (light pass): expand Favorites, collapse Packages in the floating Project browser's folder tree, scroll it to the top
// and give it keyboard focus with Assets/Scenes selected (matches the dark capture's state).
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var t = typeof(Editor).Assembly.GetType("UnityEditor.ProjectBrowser");
var w = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().First(x => !x.docked);
var tree = t.GetField("m_FolderTree", BF).GetValue(w);
var data = tree.GetType().GetProperty("data", BF).GetValue(tree);
var state = tree.GetType().GetProperty("state", BF).GetValue(tree);
var root = data.GetType().GetProperty("root", BF).GetValue(data);
string s = "";
foreach (var it in (System.Collections.IEnumerable)root.GetType().GetProperty("children", BF).GetValue(root)) {
  var dn = (string)it.GetType().GetProperty("displayName", BF).GetValue(it); s += dn + ";";
  var se = data.GetType().GetMethods(BF).FirstOrDefault(m => m.Name == "SetExpanded" && m.GetParameters().Length == 2 && m.GetParameters()[0].ParameterType.IsInstanceOfType(it));
  if (dn == "Packages" || dn == "Favorites") se?.Invoke(data, new object[]{ it, dn == "Favorites" }); }
state.GetType().GetField("scrollPos", BF).SetValue(state, Vector2.zero);
w.Focus();
var sf = tree.GetType().GetMethod("GrabKeyboardFocus", BF); sf?.Invoke(tree, null);   // applied on the tree's next OnGUI (focused selection colour)
w.Repaint(); return s + " focus=" + (sf != null);
