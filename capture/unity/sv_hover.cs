// Hover (X, Y) (pt, window coords incl. tab) in the floating one-column Project browser by sending it a MouseMove event (no OS cursor).
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
float X = 295, Y = 246;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var pt = typeof(Editor).Assembly.GetType("UnityEditor.ProjectBrowser");
var w = Resources.FindObjectsOfTypeAll(pt).Cast<EditorWindow>().First(x => pt.GetField("m_AssetTree", BF).GetValue(x) != null);
w.SendEvent(new Event { type = EventType.MouseMove, mousePosition = new Vector2(X, Y) });
w.Repaint(); return "hover " + X + "," + Y;
