// Profiler CPU Usage legend: click the NAME series colour toggle through the window's own event path (toggles that series' visibility).
using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq;
string NAME = "Others";
var w = Resources.FindObjectsOfTypeAll<ProfilerWindow>().First(x => !x.docked);
var lab = w.rootVisualElement.Query<Label>().Where(l => l.text == NAME).First();
var item = lab.parent; var c = item.Q("profiler-chart-view__legend__series-item__toggle").worldBound.center;
w.Focus();
w.SendEvent(new Event { type = EventType.MouseDown, mousePosition = c, button = 0, clickCount = 1 });
w.SendEvent(new Event { type = EventType.MouseUp, mousePosition = c, button = 0, clickCount = 1 });
w.Repaint(); return "clicked " + NAME + " at " + c + " classes=" + string.Join(".", item.GetClasses());
