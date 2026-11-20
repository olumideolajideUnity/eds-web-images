using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var w = Resources.FindObjectsOfTypeAll<ProfilerWindow>().First(x => !x.docked);
var s = "min=" + w.minSize + "\nFIELDS: " + string.Join(", ", typeof(ProfilerWindow).GetFields(BF).Select(f => f.Name + ":" + f.FieldType.Name));
var mf = typeof(ProfilerWindow).GetField("m_AllModules", BF) ?? typeof(ProfilerWindow).GetField("m_ProfilerModules", BF);
if (mf != null) { var l = (System.Collections.IEnumerable)mf.GetValue(w); s += "\nMODULES: "; foreach (var m in l) { var t = m.GetType(); var ap = t.GetProperty("active", BF); s += t.Name + "/" + t.GetProperty("DisplayName", BF)?.GetValue(m) + "=" + ap?.GetValue(m) + "; "; } }
System.IO.File.WriteAllText("C:/tmp/prof.txt", s); return "ok";
