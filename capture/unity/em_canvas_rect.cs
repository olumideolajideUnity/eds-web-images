using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq;
var it = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
var w = Resources.FindObjectsOfTypeAll(it).Cast<EditorWindow>().First(x => !x.docked);
var els = w.rootVisualElement.Query<VisualElement>().ToList().Where(e => e.GetType().Name == "EditorElement").Select(e => e.name + ":" + e.worldBound);
return string.Join(" | ", els);
