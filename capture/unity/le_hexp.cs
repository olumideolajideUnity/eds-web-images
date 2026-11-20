using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var t = typeof(Editor).Assembly.GetType("UnityEditor.SceneHierarchyWindow");
var w = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().First(x => !x.docked);
var sh = t.GetField("m_SceneHierarchy", BF)?.GetValue(w);
var ms = sh == null ? "" : string.Join(",", sh.GetType().GetMethods(BF).Where(m => m.Name.Contains("Expand")).Select(m => m.Name + "(" + string.Join(" ", m.GetParameters().Select(p => p.ParameterType.Name)) + ")"));
System.IO.File.WriteAllText("C:/tmp/hexp.txt", "sh=" + (sh != null) + " " + ms); return "ok";
