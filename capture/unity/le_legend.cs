using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq;
var w = Resources.FindObjectsOfTypeAll<ProfilerWindow>().First(x => !x.docked);
var labs = w.rootVisualElement.Query<Label>().Where(l => l.text == "Others").ToList();
var s = "n=" + labs.Count;
foreach (var l in labs) { var p = (VisualElement)l; var chain = ""; for (int i = 0; i < 5 && p != null; i++, p = p.parent) chain += p.GetType().Name + "#" + p.name + "." + string.Join(".", p.GetClasses()) + " > "; s += "\n" + chain; }
System.IO.File.WriteAllText("C:/tmp/legend.txt", s); return "ok";
