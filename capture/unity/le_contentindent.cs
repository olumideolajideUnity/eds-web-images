using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection; using System.Collections;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var t = typeof(Editor).Assembly.GetType("UnityEditor.ProjectBrowser");
var cwT = typeof(Editor).Assembly.GetType("UnityEditor.ContainerWindow");
bool NotMain(EditorWindow x) { var p = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(x); var c = p?.GetType().GetProperty("window", BF)?.GetValue(p); return c != null && (int)cwT.GetProperty("showMode", BF).GetValue(c) != 4; }
var w = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().First(NotMain);
var tree = t.GetField("m_FolderTree", BF).GetValue(w);
var gui = tree.GetType().GetProperty("gui", BF).GetValue(tree); var data = tree.GetType().GetProperty("data", BF).GetValue(tree);
var rows = ((IEnumerable)data.GetType().GetMethods(BF).Concat(data.GetType().GetInterfaces().SelectMany(i => i.GetMethods())).First(m => m.Name == "GetRows" && m.GetParameters().Length == 0).Invoke(data, null)).Cast<object>().ToList();
MethodInfo G(string n) { for (var tt = gui.GetType(); tt != null; tt = tt.BaseType) { var m = tt.GetMethods(BF | BindingFlags.DeclaredOnly).FirstOrDefault(x => x.Name == n && x.GetParameters().Length == 1); if (m != null) return m; } return null; }
var ci = G("GetContentIndent"); var fi = G("GetFoldoutIndent");
string F(string n) { for (var tt = gui.GetType(); tt != null; tt = tt.BaseType) { var f = tt.GetField(n, BF | BindingFlags.DeclaredOnly); if (f != null) return n + "=" + f.GetValue(f.IsStatic ? null : gui); var p = tt.GetProperty(n, BF | BindingFlags.DeclaredOnly); if (p != null) return n + "=" + p.GetValue(gui); } return n + "=?"; }
var s = string.Join(" ", new[]{ "k_BaseIndent", "k_IndentWidth", "indentWidth", "k_FoldoutWidth", "foldoutStyleWidth", "k_IconWidth", "k_SpaceBetweenIconAndText", "iconLeftPadding", "k_LineHeight" }.Select(F));
foreach (var r in rows.Take(13)) s += " | " + r.GetType().GetProperty("displayName").GetValue(r) + ": fold=" + fi?.Invoke(gui, new[]{ r }) + " content=" + ci?.Invoke(gui, new[]{ r });
return s;
