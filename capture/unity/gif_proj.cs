// GIF frames: floating two-column Project browser W x H pt, folder tree with every root (Favorites, Assets, Packages) collapsed,
// the right pane showing SHOW (an asset folder path), search cleared, nothing selected. Real clicks then drive it (unity/gif_mouse.cs).
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
float W = 800, H = 300; string SHOW = "Assets";
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var t = typeof(Editor).Assembly.GetType("UnityEditor.ProjectBrowser");
var w = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().FirstOrDefault(x => !x.docked);
if (w == null) { w = (EditorWindow)ScriptableObject.CreateInstance(t); w.Show(); }
var vm = t.GetNestedType("ViewMode", BF); t.GetMethod("SetViewMode", BF).Invoke(w, new object[]{ System.Enum.Parse(vm, "TwoColumns") });
w.position = new Rect(300, 150, W, H);
t.GetMethod("ClearSearch", BF)?.Invoke(w, null);
var folder = AssetDatabase.LoadAssetAtPath<Object>(SHOW);
t.GetMethod("ShowFolderContents", BF, null, new[]{ typeof(EntityId), typeof(bool) }, null)?.Invoke(w, new object[]{ folder.GetEntityId(), false });
Selection.activeObject = null;
var tree = t.GetField("m_FolderTree", BF).GetValue(w);
var data = tree.GetType().GetProperty("data", BF).GetValue(tree);
var root = data.GetType().GetProperty("root", BF).GetValue(data);
string s = "";
foreach (var it in (System.Collections.IEnumerable)root.GetType().GetProperty("children", BF).GetValue(root)) {
  var dn = (string)it.GetType().GetProperty("displayName", BF).GetValue(it); s += dn + ";";
  var se = data.GetType().GetMethods(BF).FirstOrDefault(m => m.Name == "SetExpanded" && m.GetParameters().Length == 2 && m.GetParameters()[0].ParameterType.IsInstanceOfType(it));
  se?.Invoke(data, new object[]{ it, false }); }
MethodInfo DM(string n, int np) { for (var ty = data.GetType(); ty != null; ty = ty.BaseType) { var m = ty.GetMethods(BF).FirstOrDefault(x => x.Name == n && x.GetParameters().Length == np); if (m != null) return m; } return null; }
var setIds = DM("SetExpandedIDs", 1); if (setIds != null) setIds.Invoke(data, new object[]{ System.Array.CreateInstance(setIds.GetParameters()[0].ParameterType.GetElementType(), 0) });
DM("ReloadData", 0)?.Invoke(data, null);
var state = tree.GetType().GetProperty("state", BF).GetValue(tree); state.GetType().GetField("scrollPos", BF).SetValue(state, Vector2.zero);
var selP = state.GetType().GetProperty("selectedIDs", BF); if (selP != null) selP.SetValue(state, System.Activator.CreateInstance(selP.PropertyType));
w.wantsMouseMove = true; w.Repaint(); return s + " pos=" + w.position;
