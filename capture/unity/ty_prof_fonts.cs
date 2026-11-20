// Typography: resolved font sizes of the Profiler's UI Toolkit text (module titles, legend, chart axis and value labels).
using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq;
var w = Resources.FindObjectsOfTypeAll<ProfilerWindow>().First(x => !x.docked);
var s = string.Join("\n", w.rootVisualElement.Query<TextElement>().ToList().Where(t => !string.IsNullOrEmpty(t.text) && t.resolvedStyle.display != DisplayStyle.None && t.worldBound.y < 300)
  .Select(t => t.resolvedStyle.fontSize + "px " + t.GetType().Name + " '" + t.text + "' #" + t.name + " @" + t.worldBound.position).Distinct().Take(60));
System.IO.File.WriteAllText("C:/tmp/pfonts.txt", s); return "ok";
