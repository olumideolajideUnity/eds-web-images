using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var t = typeof(Editor).Assembly.GetType("UnityEditor.ProjectBrowser");
return string.Join(", ", t.GetFields(BF).Where(f => f.IsLiteral || f.IsStatic).Where(f => f.Name.Contains("Width") || f.Name.Contains("Min")).Select(f => f.Name + "=" + (f.IsLiteral ? f.GetRawConstantValue() : f.GetValue(null))));
