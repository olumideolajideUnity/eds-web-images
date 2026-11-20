using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var t = typeof(Editor).Assembly.GetType("UnityEditor.ProjectBrowser");
var w = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().First();
var s = "";
foreach (var fn in new[]{ "m_FolderTree", "m_AssetTree" }) {
  var tree = t.GetField(fn, BF)?.GetValue(w); if (tree == null) { s += fn + "=null; "; continue; }
  var gui = tree.GetType().GetProperty("gui", BF).GetValue(tree);
  float Get(string n) { for (var tt = gui.GetType(); tt != null; tt = tt.BaseType) { var f = tt.GetField(n, BF); if (f != null) return System.Convert.ToSingle(f.GetValue(f.IsStatic ? null : gui)); var pp = tt.GetProperty(n, BF); if (pp != null) return System.Convert.ToSingle(pp.GetValue(gui)); } return -1; }
  s += fn + ": " + gui.GetType().Name + " k_IndentWidth=" + Get("k_IndentWidth") + " indentWidth=" + Get("indentWidth") + " k_BaseIndent=" + Get("k_BaseIndent") + " k_IconWidth=" + Get("k_IconWidth") + " foldoutWidth=" + Get("foldoutWidth") + "; ";
}
return s;
