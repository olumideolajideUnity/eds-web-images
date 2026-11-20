// Components > Scroll view: floating one-column Project browser (W x H pt incl. tab), Favorites collapsed, everything collapsed except
// the folder chains in EXPAND (each "A/B/C" expands A, then B, then C), scrolled to SCROLL, nothing selected.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection; using System.Collections;
float W = 300, H = 264; string[] EXPAND = { "Assets/Atrio/Character/Bird" }; Vector2 SCROLL = Vector2.zero;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var cwT = typeof(Editor).Assembly.GetType("UnityEditor.ContainerWindow");
bool NotMain(EditorWindow x) { var p = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(x); var c = p?.GetType().GetProperty("window", BF)?.GetValue(p); return c != null && (int)cwT.GetProperty("showMode", BF).GetValue(c) != 4; }
var pt = typeof(Editor).Assembly.GetType("UnityEditor.ProjectBrowser");
var pw = Resources.FindObjectsOfTypeAll(pt).Cast<EditorWindow>().FirstOrDefault(NotMain);
if (pw == null) { pw = (EditorWindow)ScriptableObject.CreateInstance(pt); pw.Show(); }
if (pt.GetField("m_AssetTree", BF).GetValue(pw) == null) { var vm = pt.GetNestedType("ViewMode", BF); pt.GetMethod("SetViewMode", BF).Invoke(pw, new object[]{ System.Enum.Parse(vm, "OneColumn") }); pw.Repaint(); return "set one column; run again"; }
var d = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(pw); var cw = d.GetType().GetProperty("window", BF).GetValue(d);
cw.GetType().GetProperty("position", BF).SetValue(cw, new Rect(1020, 300, W, H));
Selection.activeObject = null;
var tree = pt.GetField("m_AssetTree", BF).GetValue(pw);
var data = tree.GetType().GetProperty("data", BF).GetValue(tree);
var dt = data.GetType();
MethodInfo M(string n, int np) { for (var t = dt; t != null; t = t.BaseType) { var m = t.GetMethods(BF).FirstOrDefault(x => x.Name == n && x.GetParameters().Length == np); if (m != null) return m; } foreach (var i in dt.GetInterfaces()) { var m = i.GetMethods().FirstOrDefault(x => x.Name == n && x.GetParameters().Length == np); if (m != null) return m; } return null; }
var setIds = M("SetExpandedIDs", 1); var ids = setIds.GetParameters()[0].ParameterType;
setIds.Invoke(data, new object[]{ System.Array.CreateInstance(ids.GetElementType(), 0) });
M("ReloadData", 0)?.Invoke(data, null);
var getRows = M("GetRows", 0);
var setExp = dt.GetMethods(BF).Concat(dt.GetInterfaces().SelectMany(i => i.GetMethods())).First(m => m.Name == "SetExpanded" && m.GetParameters().Length == 2 && m.GetParameters()[0].ParameterType.Name.Contains("TreeViewItem"));
string log = "";
foreach (var chain in EXPAND) {
  int depth = -1;
  foreach (var name in chain.Split('/')) {
    var rows = ((IEnumerable)getRows.Invoke(data, null)).Cast<object>().ToList();
    var it = rows.FirstOrDefault(r => (string)r.GetType().GetProperty("displayName").GetValue(r) == name && (int)r.GetType().GetProperty("depth").GetValue(r) > depth);
    if (it == null) { log += " missing:" + name; break; }
    depth = (int)it.GetType().GetProperty("depth").GetValue(it);
    setExp.Invoke(data, new object[]{ it, true }); M("InitIfNeeded", 0)?.Invoke(data, null); } }
var st = tree.GetType().GetProperty("state", BF).GetValue(tree);
var sp = st.GetType().GetField("scrollPos", BF); if (sp != null) sp.SetValue(st, SCROLL); else st.GetType().GetProperty("scrollPos", BF)?.SetValue(st, SCROLL);
pw.Repaint();
var tr = tree.GetType().GetMethods(BF).FirstOrDefault(m => m.Name == "GetTotalRect" && m.GetParameters().Length == 0)?.Invoke(tree, null);
var rowsAll = ((IEnumerable)getRows.Invoke(data, null)).Cast<object>().Select(r => "[\"" + r.GetType().GetProperty("displayName").GetValue(r) + "\"," + r.GetType().GetProperty("depth").GetValue(r) + "]");
System.IO.File.WriteAllText("C:/tmp/tv_rows.json", "{\"total\":\"" + tr + "\",\"rows\":[" + string.Join(",", rowsAll) + "]}");
var fin = ((IEnumerable)getRows.Invoke(data, null)).Cast<object>().Select(r => new string(' ', (int)r.GetType().GetProperty("depth").GetValue(r)) + r.GetType().GetProperty("displayName").GetValue(r)).Take(18);
return log + " ROWS:" + string.Join("|", fin);
