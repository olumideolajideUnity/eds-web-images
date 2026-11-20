// Home hero: floating two-column Project browser with the folder pane widened to the whole window (folders only, Favorites root) (W x H pt incl. tab) with everything collapsed except
// Assets > EFDS_Screenshots > GameplaySequenceDemo > Info, Favorites collapsed, scrolled to the top, nothing selected.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection; using System.Collections;
float W = 260, H = 276; string[] PATH = { "Assets", "EFDS_Screenshots", "GameplaySequenceDemo", "Info" };
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var cwT = typeof(Editor).Assembly.GetType("UnityEditor.ContainerWindow");
bool NotMain(EditorWindow x) { var p = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(x); var c = p?.GetType().GetProperty("window", BF)?.GetValue(p); return c != null && (int)cwT.GetProperty("showMode", BF).GetValue(c) != 4; }
var pt = typeof(Editor).Assembly.GetType("UnityEditor.ProjectBrowser");
var pw = Resources.FindObjectsOfTypeAll(pt).Cast<EditorWindow>().FirstOrDefault(NotMain);
if (pw == null) { pw = (EditorWindow)ScriptableObject.CreateInstance(pt); pw.Show(); }
if (pt.GetField("m_FolderTree", BF).GetValue(pw) == null) { var vm = pt.GetNestedType("ViewMode", BF); pt.GetMethod("SetViewMode", BF).Invoke(pw, new object[]{ System.Enum.Parse(vm, "TwoColumns") }); pw.Repaint(); return "set two columns; run again"; }
pt.GetField("m_DirectoriesAreaWidth", BF).SetValue(pw, W + 40f);
var d = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(pw); var cw = d.GetType().GetProperty("window", BF).GetValue(d);
cw.GetType().GetProperty("position", BF).SetValue(cw, new Rect(1020, 330, W, H));
Selection.activeObject = null;
var tree = pt.GetField("m_FolderTree", BF).GetValue(pw);
var data = tree.GetType().GetProperty("data", BF).GetValue(tree);
var dt = data.GetType();
MethodInfo M(string n, int np) { for (var t = dt; t != null; t = t.BaseType) { var m = t.GetMethods(BF).FirstOrDefault(x => x.Name == n && x.GetParameters().Length == np); if (m != null) return m; } foreach (var i in dt.GetInterfaces()) { var m = i.GetMethods().FirstOrDefault(x => x.Name == n && x.GetParameters().Length == np); if (m != null) return m; } return null; }
var setIds = M("SetExpandedIDs", 1); var ids = setIds.GetParameters()[0].ParameterType;
setIds.Invoke(data, new object[]{ System.Array.CreateInstance(ids.GetElementType(), 0) });
var reload = M("ReloadData", 0); reload?.Invoke(data, null);
var getRows = M("GetRows", 0);
var setExp = dt.GetMethods(BF).Concat(dt.GetInterfaces().SelectMany(i => i.GetMethods())).First(m => m.Name == "SetExpanded" && m.GetParameters().Length == 2 && m.GetParameters()[0].ParameterType.Name.Contains("TreeViewItem"));
string log = "";
foreach (var name in PATH) {
  var rows = ((IEnumerable)getRows.Invoke(data, null)).Cast<object>().ToList();
  var it = rows.FirstOrDefault(r => (string)r.GetType().GetProperty("displayName").GetValue(r) == name);
  if (it == null) { log += " missing:" + name; break; }
  setExp.Invoke(data, new object[]{ it, true });
  M("InitIfNeeded", 0)?.Invoke(data, null);
}
var st = tree.GetType().GetProperty("state", BF).GetValue(tree);
st.GetType().GetProperty("scrollPos", BF)?.SetValue(st, Vector2.zero);
tree.GetType().GetMethod("ReloadData", BF, null, System.Type.EmptyTypes, null)?.Invoke(tree, null);
pw.Repaint();
var fin = ((IEnumerable)getRows.Invoke(data, null)).Cast<object>().Select(r => new string(' ', (int)r.GetType().GetProperty("depth").GetValue(r)) + r.GetType().GetProperty("displayName").GetValue(r)).Take(16);
return log + " ROWS:" + string.Join("|", fin);
