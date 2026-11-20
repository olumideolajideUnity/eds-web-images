// Close every Scene and Game view that isn't in the main window (floating, tabbed or not).
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var cwT = typeof(Editor).Assembly.GetType("UnityEditor.ContainerWindow");
bool NotMain(EditorWindow x) { var p = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(x); var c = p?.GetType().GetProperty("window", BF)?.GetValue(p); return c != null && (int)cwT.GetProperty("showMode", BF).GetValue(c) != 4; }
var l = Resources.FindObjectsOfTypeAll<EditorWindow>().Where(x => (x is SceneView || x.GetType().Name == "GameView") && NotMain(x)).ToList();
foreach (var w in l) w.Close();
return "closed " + l.Count;
