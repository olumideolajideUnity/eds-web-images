// Profiler CPU Usage chart: list series and set HIDE (comma list of series names) disabled, like clicking them in the legend.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
string HIDE = "";
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var w = Resources.FindObjectsOfTypeAll<ProfilerWindow>().First(x => !x.docked);
var cpu = ((System.Collections.IEnumerable)typeof(ProfilerWindow).GetField("m_AllModules", BF).GetValue(w)).Cast<object>().First(m => m.GetType().Name == "CPUProfilerModule");
object chart = null; for (var t = cpu.GetType(); t != null && chart == null; t = t.BaseType) chart = t.GetField("m_Chart", BF)?.GetValue(cpu);
var series = (System.Array)chart.GetType().GetField("m_Series", BF).GetValue(chart);
var s = "";
var hide = HIDE.Split(',').Where(x => x != "").ToList();
foreach (var sr in series) {
  var name = sr.GetType().GetField("name", BF)?.GetValue(sr) as string; var en = sr.GetType().GetField("enabled", BF);
  if (hide.Count > 0) en.SetValue(sr, !hide.Contains(name));
  s += name + "=" + en.GetValue(sr) + "; ";
}
var m1 = chart.GetType().GetMethods(BF).Where(m => m.Name.Contains("Enabled") || m.Name.Contains("Toggle")).Select(m => m.Name);
w.Repaint(); return s + " | " + string.Join(",", m1);
