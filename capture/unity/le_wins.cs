using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.NonPublic|BindingFlags.Public;
var ost = typeof(Editor).Assembly.GetType("UnityEditor.ObjectSelector");
var os = (EditorWindow)ost.GetProperty("get", BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic).GetValue(null);
var m = ost.GetMethods(BF).First(x => x.Name == "Show" && x.GetParameters().Length == 10 && x.GetParameters()[1].ParameterType == typeof(System.Type));
m.Invoke(os, new object[]{ null, typeof(Mesh), null, false, null, null, null, true, true, false });
var s = string.Join("; ", Resources.FindObjectsOfTypeAll<EditorWindow>().Where(w => w.GetType().FullName.Contains("Search") || w.GetType().Name.Contains("Picker") || w.GetType().Name.Contains("Selector")).Select(w => w.GetType().FullName + " '" + w.titleContent.text + "' " + w.position + " parent=" + (typeof(EditorWindow).GetField("m_Parent", BF).GetValue(w)?.GetType().Name ?? "null")));
return s;
