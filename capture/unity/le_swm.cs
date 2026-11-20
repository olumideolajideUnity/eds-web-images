using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.DeclaredOnly;
var t = typeof(Editor).Assembly.GetType("UnityEditor.ProjectSettingsWindow").BaseType;
System.IO.File.WriteAllText("C:/tmp/swm.txt", t.Name + ": " + string.Join(", ", t.GetMethods(BF).Select(m => m.Name + "(" + string.Join(" ", m.GetParameters().Select(p => p.ParameterType.Name)) + ")")));
return "ok";
