// Move the (virtual) mouse off the Profiler's charts so no hover-only icons (pin) show; sends a MouseMove event only.
using UnityEditor; using UnityEngine; using System.Linq;
var w = Resources.FindObjectsOfTypeAll<ProfilerWindow>().First(x => !x.docked);
w.SendEvent(new Event { type = EventType.MouseMove, mousePosition = new Vector2(400, 440) }); w.Repaint(); return "ok";
