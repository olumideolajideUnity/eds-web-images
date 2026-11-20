using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var s = string.Join("\n", typeof(Editor).Assembly.GetTypes().SelectMany(t => t.GetMembers(BF).Where(m => m.Name.ToLower().Contains("colorblind")).Select(m => t.FullName + "." + m.Name + " " + m.MemberType)).Distinct().Take(30));
System.IO.File.WriteAllText("C:/tmp/cb.txt", s); return "ok";
