using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
int MODE = 0;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var cwT = typeof(Editor).Assembly.GetType("UnityEditor.ContainerWindow");
bool NotMain(EditorWindow x) { var p = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(x); var c = p?.GetType().GetProperty("window", BF)?.GetValue(p); return c != null && (int)cwT.GetProperty("showMode", BF).GetValue(c) != 4; }
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(NotMain);
var cam = Object.FindObjectsByType<Camera>().First().gameObject;
if (MODE == 0) { Selection.activeObject = null; sv.LookAtDirect(new Vector3(-2.0f, 3.0f, -1.0f), Quaternion.Euler(52, 30, 0), 15f); sv.Repaint(); return "scene"; }
Selection.activeGameObject = cam; EditorGUIUtility.PingObject(cam);
EditorGUIUtility.PingObject(AssetDatabase.LoadMainAssetAtPath("Assets/Atrio/Environment/FountainStatue"));
return "selected " + cam.name;
