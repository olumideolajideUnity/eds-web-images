using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var w = Resources.FindObjectsOfTypeAll<ProfilerWindow>().First(x => !x.docked);
var cpu = ((System.Collections.IEnumerable)typeof(ProfilerWindow).GetField("m_AllModules", BF).GetValue(w)).Cast<object>().First(m => m.GetType().Name == "CPUProfilerModule");
object cvc = null; for (var t = cpu.GetType(); t != null && cvc == null; t = t.BaseType) cvc = t.GetField("m_ChartViewController", BF | BindingFlags.DeclaredOnly)?.GetValue(cpu);
var s = "";
for (var t = cvc.GetType(); t != null && t != typeof(object); t = t.BaseType) s += t.Name + " M: " + string.Join(", ", t.GetMethods(BF | BindingFlags.DeclaredOnly).Select(m => m.Name + "/" + m.GetParameters().Length)) + "\n F: " + string.Join(", ", t.GetFields(BF | BindingFlags.DeclaredOnly).Select(f => f.Name + ":" + f.FieldType.Name)) + "\n";
System.IO.File.WriteAllText("C:/tmp/cvc.txt", s); return "ok";
