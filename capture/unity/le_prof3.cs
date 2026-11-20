using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var t = typeof(ProfilerWindow);
var w = Resources.FindObjectsOfTypeAll<ProfilerWindow>().First(x => !x.docked);
var s = string.Join(", ", t.GetMethods(BF).Where(m => m.DeclaringType == t && (m.Name.Contains("Chart") || m.Name.Contains("Stale") || m.Name.Contains("Refresh") || m.Name.Contains("Update"))).Select(m => m.Name + "/" + m.GetParameters().Length));
System.IO.File.WriteAllText("C:/tmp/prof3.txt", s); return "ok";
