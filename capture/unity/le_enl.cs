using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.DeclaredOnly;
var asms = new[]{ typeof(Editor).Assembly, typeof(GameObject).Assembly }.Concat(System.AppDomain.CurrentDomain.GetAssemblies().Where(a => a.GetName().Name.Contains("Universal") || a.GetName().Name.Contains("Rendering")));
var s = string.Join("\n", asms.SelectMany(a => { try { return a.GetTypes(); } catch { return new System.Type[0]; } })
  .SelectMany(t => t.GetMembers(BF).Where(m => (m.MemberType == MemberTypes.Property || m.MemberType == MemberTypes.Field || m.MemberType == MemberTypes.Method) && (m.Name.Contains("Enlighten") || m.Name.Contains("RealtimeGI") || m.Name.Contains("realtimeGI"))).Select(m => t.FullName + "." + m.Name)).Distinct().Take(60));
System.IO.File.WriteAllText("C:/tmp/enl.txt", s); return "ok";
