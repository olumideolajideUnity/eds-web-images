using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.DeclaredOnly;
var s = string.Join("\n", typeof(Editor).Assembly.GetTypes().Where(t => (t.Namespace ?? "").Contains("Overlay") || t.Name.Contains("Overlay") || t.Name.Contains("SceneView"))
  .SelectMany(t => t.GetMembers(BF).Where(m => m.Name.ToLower().Contains("opacity") || m.Name.ToLower().Contains("transparen") || m.Name.ToLower().Contains("alpha")).Select(m => t.FullName + "." + m.Name + " " + m.MemberType)).Distinct().Take(40));
System.IO.File.WriteAllText("C:/tmp/opac.txt", s); return "ok";
