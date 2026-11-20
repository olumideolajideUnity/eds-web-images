using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var w = Resources.FindObjectsOfTypeAll<ProfilerWindow>().First(x => !x.docked);
var cpu = ((System.Collections.IEnumerable)typeof(ProfilerWindow).GetField("m_AllModules", BF).GetValue(w)).Cast<object>().First(m => m.GetType().Name == "CPUProfilerModule");
var s = "";
for (var t = cpu.GetType(); t != null; t = t.BaseType) s += t.Name + ": " + string.Join(", ", t.GetFields(BF | BindingFlags.DeclaredOnly).Select(f => f.Name + ":" + f.FieldType.Name)) + "\n";
System.IO.File.WriteAllText("C:/tmp/cpu.txt", s); return "ok";
