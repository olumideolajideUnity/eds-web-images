using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var t = typeof(ProfilerWindow);
var s = string.Join("\n", t.GetMembers(BF).Where(m => m.Name.ToLower().Contains("bottleneck") || m.Name.ToLower().Contains("highlight") || m.Name.ToLower().Contains("pinned") || m.Name.ToLower().Contains("split")).Select(m => m.MemberType + " " + m.Name));
var asm = t.Assembly;
s += "\nTYPES: " + string.Join(",", asm.GetTypes().Where(x => x.Name.Contains("Bottleneck") || x.Name.Contains("Highlights")).Select(x => x.FullName));
System.IO.File.WriteAllText("C:/tmp/prof2.txt", s); return "ok";
