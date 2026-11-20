// Light Explorer: the original's column widths, no sort column, Point Light selected.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var t = typeof(Editor).Assembly.GetType("UnityEditor.LightingExplorerWindow");
var w = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().First();
var tab = ((System.Array)t.GetField("m_TableTabs", BF).GetValue(w)).GetValue(0);
var tbl = tab.GetType().GetField("m_LightTable", BF).GetValue(tab);
var st = (UnityEditor.IMGUI.Controls.MultiColumnHeaderState)tbl.GetType().GetField("m_MultiColumnHeaderState", BF).GetValue(tbl);
float[] wd = { 50, 200, 120, 120, 70, 70, 60, 60, 110, 100 };
for (int i = 0; i < st.columns.Length && i < wd.Length; i++) st.columns[i].width = wd[i];
st.sortedColumnIndex = -1;
var tv = tbl.GetType().GetField("m_TreeView", BF).GetValue(tbl);
var hdr = tv.GetType().GetProperty("multiColumnHeader", BF)?.GetValue(tv) as UnityEditor.IMGUI.Controls.MultiColumnHeader;
if (hdr != null) { hdr.sortedColumnIndex = -1; foreach (var i in Enumerable.Range(0, wd.Length)) if (i < hdr.state.columns.Length) hdr.state.columns[i].width = wd[i]; }
var p = GameObject.Find("Point Light");
Selection.activeGameObject = p;
var rl = tv.GetType().GetMethod("Reload", BF, null, System.Type.EmptyTypes, null);
rl?.Invoke(tv, null);
string r = "rl=" + rl?.Name;
foreach (var m in tv.GetType().GetMethods(BF).Where(m => m.Name == "SetSelection" && m.GetParameters().Length == 1)) {
  var pt = m.GetParameters()[0].ParameterType; var et = pt.IsGenericType ? pt.GetGenericArguments()[0] : null;
  if (et == null) continue;
  var lst = (System.Collections.IList)System.Activator.CreateInstance(typeof(System.Collections.Generic.List<>).MakeGenericType(et));
  lst.Add(et == typeof(int) ? (object)(int)typeof(Object).GetMethod("GetInstanceID").Invoke(p, null) : (object)p.GetEntityId());
  try { m.Invoke(tv, new object[]{ lst }); r += " ss=" + et.Name; } catch (System.Exception e) { r += " sserr=" + e.GetType().Name; }
  break;
}
w.Repaint();
return r + " hdr=" + (hdr != null);
