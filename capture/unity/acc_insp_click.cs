// Click (X, Y) (pt, window coords incl. tab) in the floating Inspector through its own event path.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
float X = 188, Y = 238;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var it = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
var cwT = typeof(Editor).Assembly.GetType("UnityEditor.ContainerWindow");
bool NotMain(EditorWindow x) { var p = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(x); var c = p?.GetType().GetProperty("window", BF)?.GetValue(p); return c != null && (int)cwT.GetProperty("showMode", BF).GetValue(c) != 4; }
var w = Resources.FindObjectsOfTypeAll(it).Cast<EditorWindow>().First(NotMain);
w.Focus();
var pos = new Vector2(X, Y);   // event coordinates include the tab strip
w.SendEvent(new Event { type = EventType.MouseDown, mousePosition = pos, button = 0, clickCount = 1 });
w.SendEvent(new Event { type = EventType.MouseUp, mousePosition = pos, button = 0, clickCount = 1 });
w.SendEvent(new Event { type = EventType.MouseMove, mousePosition = new Vector2(5, 400) });
w.Repaint();
var cam = Selection.activeGameObject.GetComponent<Camera>();
return "fov=" + cam.fieldOfView + " kc=" + GUIUtility.keyboardControl;
