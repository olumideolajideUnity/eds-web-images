// Rename the instance and report worldBounds of the Inspector's "Scale" label and its row (pt, window content coords).
using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq;
var cube = GameObject.Find("Cube") ?? GameObject.Find("Cube instance"); cube.name = "Cube instance";
var it = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
var w = Resources.FindObjectsOfTypeAll(it).Cast<EditorWindow>().First(x => !x.docked);
var lbls = w.rootVisualElement.Query<Label>().ToList().Where(l => l.text == "Scale").ToList();
string r = string.Join(" | ", lbls.Select(l => "label " + l.worldBound + " parent " + l.parent?.GetType().Name + " " + l.parent?.worldBound + " gp " + l.parent?.parent?.worldBound));
var x = w.rootVisualElement.Query<FloatField>().ToList().Select(f => f.worldBound.x).DefaultIfEmpty(-1).Min();
return r + " | firstField x=" + x;
