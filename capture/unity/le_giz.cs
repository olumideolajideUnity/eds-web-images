using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var cwT = typeof(Editor).Assembly.GetType("UnityEditor.ContainerWindow");
bool NotMain(EditorWindow x) { var p = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(x); var c = p?.GetType().GetProperty("window", BF)?.GetValue(p); return c != null && (int)cwT.GetProperty("showMode", BF).GetValue(c) != 4; }
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(NotMain);
bool G = false; sv.drawGizmos = G; sv.Repaint();
var all = string.Join(",", Resources.FindObjectsOfTypeAll<Renderer>().Where(r => !EditorUtility.IsPersistent(r) && r.gameObject.scene.IsValid()).Select(r => r.name + ":" + r.gameObject.hideFlags));
return "gizmos=" + G + " renderers: " + all;
