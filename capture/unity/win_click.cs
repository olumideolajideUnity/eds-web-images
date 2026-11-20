// Click (X, Y) (pt, window coords incl. tab) in the first floating window whose type name is TYPE, through its own event path.
using UnityEditor; using UnityEngine; using System.Linq;
string TYPE = "PreferenceSettingsWindow"; float X = 43, Y = 61;
var w = Resources.FindObjectsOfTypeAll<EditorWindow>().First(x => x.GetType().Name == TYPE);
w.Focus();
w.SendEvent(new Event { type = EventType.MouseDown, mousePosition = new Vector2(X, Y), button = 0, clickCount = 1 });
w.SendEvent(new Event { type = EventType.MouseUp, mousePosition = new Vector2(X, Y), button = 0, clickCount = 1 });
w.Repaint(); return "clicked " + TYPE;
