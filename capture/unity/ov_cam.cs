// Frame the overlays Scene view (the Scene view that is not in the main window).
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
Vector3 PIV = new Vector3(-2.0f, 4.0f, -3.0f); float PITCH = 26, YAW = 60, SIZE = 11f;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var cwT = typeof(Editor).Assembly.GetType("UnityEditor.ContainerWindow");
bool NotMain(EditorWindow x) { var p = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(x); var c = p?.GetType().GetProperty("window", BF)?.GetValue(p); return c != null && (int)cwT.GetProperty("showMode", BF).GetValue(c) != 4; }
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(NotMain);
typeof(SceneView).GetField("s_LastActiveSceneView", BF).SetValue(null, sv);
sv.drawGizmos = false; sv.LookAtDirect(PIV, Quaternion.Euler(PITCH, YAW, 0), SIZE); sv.Repaint(); return "ok " + sv.position;
