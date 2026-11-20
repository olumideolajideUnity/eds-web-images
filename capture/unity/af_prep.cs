// Authoring flows: open SCENE, select SEL (GameObject path name, "" = none), and make sure exactly one floating Scene view and one
// floating Inspector exist (created if missing), so the per-image af*.cs scripts find them.
using UnityEditor; using UnityEditor.SceneManagement; using UnityEngine; using System.Linq; using System.Reflection;
string SCENE = "Assets/EFDS_Screenshots/GOAuthoring.unity"; string SEL = "Cube";
var BF = BindingFlags.Instance|BindingFlags.NonPublic|BindingFlags.Public;
UnityEditor.SceneManagement.StageUtility.GoToMainStage();
if (EditorSceneManager.GetActiveScene().path != SCENE) EditorSceneManager.OpenScene(SCENE);
var cwT = typeof(Editor).Assembly.GetType("UnityEditor.ContainerWindow");
bool NotMain(EditorWindow x) { var p = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(x); var c = p?.GetType().GetProperty("window", BF)?.GetValue(p); return c != null && (int)cwT.GetProperty("showMode", BF).GetValue(c) != 4; }
var svs = Resources.FindObjectsOfTypeAll<SceneView>().Where(NotMain).ToList();
foreach (var x in svs.Skip(1)) x.Close();
if (!svs.Any()) { var sv = ScriptableObject.CreateInstance<SceneView>(); sv.Show(); }
var it = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
var ins = Resources.FindObjectsOfTypeAll(it).Cast<EditorWindow>().Where(NotMain).ToList();
foreach (var x in ins.Skip(1)) x.Close();
if (!ins.Any()) { var iw = (EditorWindow)ScriptableObject.CreateInstance(it); iw.Show(); }
Selection.activeGameObject = SEL == "" ? null : GameObject.Find(SEL);
Tools.current = Tool.Move; Tools.pivotMode = PivotMode.Pivot; Tools.pivotRotation = PivotRotation.Global;
return "scene=" + EditorSceneManager.GetActiveScene().name + " sel=" + Selection.activeGameObject;
