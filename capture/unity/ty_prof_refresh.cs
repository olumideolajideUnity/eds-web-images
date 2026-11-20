// Profiler: force the module charts to rebuild from the recorded frames (they stay blank after recording stops otherwise).
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var t = typeof(ProfilerWindow);
var w = Resources.FindObjectsOfTypeAll<ProfilerWindow>().First(x => !x.docked);
t.GetMethod("UpdateModules", BF, null, System.Type.EmptyTypes, null).Invoke(w, null);
var stale = (System.Collections.IEnumerable)t.GetField("m_ModulesWithStaleCharts", BF).GetValue(w);
int n = 0; foreach (var _ in stale) n++;
foreach (var m in (System.Collections.IEnumerable)t.GetField("m_AllModules", BF).GetValue(w)) { var a = (bool)m.GetType().GetProperty("active", BF).GetValue(m); if (a) try { t.GetMethod("UpdateStaleChart", BF).Invoke(w, new[]{ m }); } catch {} }
t.GetMethod("UpdateStaleChartsOnScreen", BF).Invoke(w, null);
w.Repaint(); return "stale=" + n;
