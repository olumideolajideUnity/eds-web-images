// Click the Point Light row's Name cell through the window's own GUI event path (gives the tree focus + selection).
using UnityEditor; using UnityEngine; using System.Linq;
var t = typeof(Editor).Assembly.GetType("UnityEditor.LightingExplorerWindow");
var w = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().First();
w.Focus();
var pos = new Vector2(110, 159);
w.SendEvent(new Event { type = EventType.MouseDown, mousePosition = pos, button = 0, clickCount = 1 });
w.SendEvent(new Event { type = EventType.MouseUp, mousePosition = pos, button = 0, clickCount = 1 });
w.Repaint();
return "sel=" + (Selection.activeGameObject ? Selection.activeGameObject.name : "null");
