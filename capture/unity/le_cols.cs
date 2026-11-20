using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var t = typeof(Editor).Assembly.GetType("UnityEditor.LightingExplorerWindow");
var w = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().First();
var tab = ((System.Array)t.GetField("m_TableTabs", BF).GetValue(w)).GetValue(0);
var tbl = tab.GetType().GetField("m_LightTable", BF).GetValue(tab);
var st = (UnityEditor.IMGUI.Controls.MultiColumnHeaderState)tbl.GetType().GetField("m_MultiColumnHeaderState", BF).GetValue(tbl);
return string.Join(";", st.columns.Select((c, i) => i + "=" + Mathf.Round(c.width))) + " sorted=" + st.sortedColumnIndex + " vis=" + string.Join(",", st.visibleColumns);
