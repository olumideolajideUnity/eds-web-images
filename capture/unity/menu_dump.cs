// Dump a main-menu submenu (MENU) as "label hotkey|Normal/Disabled" lines with "---|" separators (native_menu.py format),
// plus the menu bar's top-level names (in Unity's order) to OUT.bar.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection; using System.Text;
string MENU = "Edit"; string OUT = "C:/tmp/menu.txt";
var BF = BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance;
var gm = typeof(Menu).GetMethod("GetMenuItems", BF);
var smi = typeof(Editor).Assembly.GetType("UnityEditor.ScriptingMenuItem");
string P(object o, string n) => smi.GetProperty(n, BF).GetValue(o)?.ToString();
var items = ((System.Array)gm.Invoke(null, new object[]{ MENU, true, true })).Cast<object>().ToList();
var sb = new StringBuilder();
int lastPri = int.MinValue; string lastParent = null;
foreach (var it in items) {
  var path = P(it, "path"); var sep = P(it, "isSeparator") == "True";
  var rel = path.Substring(MENU.Length + 1);
  if (sep) { var sub = rel.Contains("/") ? rel.Substring(0, rel.LastIndexOf('/') + 1) : ""; sb.Append("---|" + sub + "\n"); continue; }
  var pri = int.Parse(P(it, "priority")); var parent = rel.Contains("/") ? rel.Substring(0, rel.IndexOf('/')) : "";
  if (parent == "" && lastParent == "" && lastPri != int.MinValue && pri - lastPri > 10) sb.Append("---|" + (char)10);   // Unity's priority-gap separator rule
  if (parent == "" || parent != lastParent) { lastPri = pri; } lastParent = parent;
  var hk = (string)typeof(Menu).GetMethod("GetHotkey", BF).Invoke(null, new object[]{ path }); var en = Menu.GetEnabled(path);
  sb.Append(rel + (string.IsNullOrEmpty(hk) ? "" : " " + hk) + "|" + (en ? "Normal" : "Disabled") + "\n");
}
System.IO.File.WriteAllText(OUT, sb.ToString());
var cands = new[]{ "File", "Edit", "Assets", "GameObject", "Component", "Services", "Tools", "Jobs", "ProBuilder", "Tutorials", "Window", "Help" };
var all = cands.Where(c => ((System.Array)gm.Invoke(null, new object[]{ c, false, true })).Length > 0);
System.IO.File.WriteAllText(OUT + ".bar", string.Join("\n", all));
return items.Count + " items";
