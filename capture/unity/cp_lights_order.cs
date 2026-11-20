// Light Explorer lists lights in an engine-defined order; recreate the four lights in each creation order until the
// rows read Spot, Area, Point, Directional (the original's order).
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection; using System.Collections.Generic;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var t = typeof(Editor).Assembly.GetType("UnityEditor.LightingExplorerWindow");
var w = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().First();
var tab = ((System.Array)t.GetField("m_TableTabs", BF).GetValue(w)).GetValue(0);
var tbl = tab.GetType().GetField("m_LightTable", BF).GetValue(tab);
var tv = tbl.GetType().GetField("m_TreeView", BF).GetValue(tbl);
var target = new[]{ "Spot Light", "Area Light", "Point Light", "Directional Light" };
void Mk(string n) {
  var g = new GameObject(n); var l = g.AddComponent<Light>(); l.intensity = 1;
  if (n == "Spot Light") { l.type = LightType.Spot; l.lightmapBakeType = LightmapBakeType.Realtime; }
  if (n == "Area Light") { l.type = LightType.Rectangle; l.lightmapBakeType = LightmapBakeType.Baked; }
  if (n == "Point Light") { l.type = LightType.Point; l.lightmapBakeType = LightmapBakeType.Realtime; }
  if (n == "Directional Light") { l.type = LightType.Directional; l.lightmapBakeType = LightmapBakeType.Mixed; l.color = new Color32(255, 244, 214, 255); }
}
IEnumerable<string[]> Perms(string[] a) { if (a.Length <= 1) { yield return a; yield break; } for (int i = 0; i < a.Length; i++) foreach (var p in Perms(a.Where((_, j) => j != i).ToArray())) yield return new[]{ a[i] }.Concat(p).ToArray(); }
string got = "";
foreach (var order in Perms(target).Concat(Perms(target))) {
  foreach (var l in Object.FindObjectsByType<Light>(FindObjectsInactive.Include)) Object.DestroyImmediate(l.gameObject);
  foreach (var n in order) Mk(n);
  tv.GetType().GetMethod("Update", BF, null, System.Type.EmptyTypes, null).Invoke(tv, null);
  tv.GetType().GetMethod("Reload", BF, null, System.Type.EmptyTypes, null).Invoke(tv, null);
  var rows = ((System.Collections.IEnumerable)tv.GetType().GetMethod("GetRows", BF).Invoke(tv, null)).Cast<object>().Select(r => (string)r.GetType().GetProperty("displayName").GetValue(r)).ToArray();
  got = string.Join(",", rows) + " <- " + string.Join(",", order);
  if (rows.SequenceEqual(target)) return "MATCH " + got;
}
return "nomatch " + got;
