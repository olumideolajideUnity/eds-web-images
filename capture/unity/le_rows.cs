using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var t = typeof(Editor).Assembly.GetType("UnityEditor.LightingExplorerWindow");
var w = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().First();
var tab = ((System.Array)t.GetField("m_TableTabs", BF).GetValue(w)).GetValue(0);
var tbl = tab.GetType().GetField("m_LightTable", BF).GetValue(tab);
var tv = tbl.GetType().GetField("m_TreeView", BF).GetValue(tbl);
tv.GetType().GetMethod("Update", BF, null, System.Type.EmptyTypes, null).Invoke(tv, null); tv.GetType().GetMethod("Reload", BF, null, System.Type.EmptyTypes, null).Invoke(tv, null);
var rows = (System.Collections.IEnumerable)tv.GetType().GetMethod("GetRows", BF).Invoke(tv, null);
var s = "";
foreach (var r in rows) { s += r.GetType().GetProperty("id").GetValue(r) + ":" + r.GetType().GetProperty("displayName").GetValue(r) + "; "; }
var sel = (System.Collections.IEnumerable)tv.GetType().GetMethod("GetSelection", BF).Invoke(tv, null);
s += " sel="; foreach (var x in sel) s += x + ",";
foreach (var n in new[]{"Spot Light","Area Light","Point Light","Directional Light"}) { var g = GameObject.Find(n); s += " " + n + "=" + g.GetEntityId() + "/" + g.GetComponent<Light>().GetEntityId(); }
return s;
