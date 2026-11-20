using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var t = typeof(Editor).Assembly.GetType("UnityEditor.ProjectSettingsWindow");
var w = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().First(x => !x.docked);
var s = "";
for (var tt = t; tt != typeof(EditorWindow); tt = tt.BaseType) foreach (var f in tt.GetFields(BF | BindingFlags.DeclaredOnly)) s += f.Name + ":" + f.FieldType.Name + "; ";
s += " | VE types: " + string.Join(",", w.rootVisualElement.Query<VisualElement>().ToList().Select(v => v.GetType().Name).Distinct());
System.IO.File.WriteAllText("C:/tmp/sw.txt", s); return "ok";
