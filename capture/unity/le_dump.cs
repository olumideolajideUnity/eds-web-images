using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection; using System.Text;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var t = typeof(Editor).Assembly.GetType("UnityEditor.LightingExplorerWindow");
var w = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().First();
var tabs = (System.Array)t.GetField("m_TableTabs", BF).GetValue(w);
var tab = tabs.GetValue(0);
var tbl = tab.GetType().GetField("m_LightTable", BF).GetValue(tab);
var sb = new StringBuilder();
foreach (var f in tbl.GetType().GetFields(BF)) sb.Append(f.Name + ":" + f.FieldType.Name + "; ");
var st = tbl.GetType().GetField("m_MultiColumnHeaderState", BF)?.GetValue(tbl) as UnityEditor.IMGUI.Controls.MultiColumnHeaderState;
if (st != null) { sb.Append("\nCOLS: "); foreach (var c in st.columns) sb.Append(System.Text.RegularExpressions.Regex.Replace(c.headerContent.text ?? "", "[^A-Za-z ]", "") + "=" + Mathf.Round(c.width) + "|" + c.headerContent.text.Length + ", "); sb.Append(" sorted=" + st.sortedColumnIndex); }
return sb.ToString().Replace("\"", "'");
