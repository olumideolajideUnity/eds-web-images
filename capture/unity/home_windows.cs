// Home hero pieces on Atrio: floating Scene (aerial, Default preset), Inspector on the Camera, Hierarchy, one-column Project.
using UnityEditor; using UnityEditor.Overlays; using UnityEditor.EditorTools; using UnityEditor.SceneManagement; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
if (EditorSceneManager.GetActiveScene().path != "Assets/Atrio/AtrioScenes/Atrio.unity") EditorSceneManager.OpenScene("Assets/Atrio/AtrioScenes/Atrio.unity");
var cwT = typeof(Editor).Assembly.GetType("UnityEditor.ContainerWindow");
bool NotMain(EditorWindow x) { var p = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(x); var c = p?.GetType().GetProperty("window", BF)?.GetValue(p); return c != null && (int)cwT.GetProperty("showMode", BF).GetValue(c) != 4; }
void Place(EditorWindow w, Rect r) { var d = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(w); var c = d.GetType().GetProperty("window", BF).GetValue(d); c.GetType().GetProperty("position", BF).SetValue(c, r); }
foreach (var w in Resources.FindObjectsOfTypeAll<EditorWindow>().Where(x => NotMain(x) && (x is SceneView || new[]{ "GameView", "InspectorWindow", "HierarchyWindow", "ProjectBrowser" }.Contains(x.GetType().Name))).ToList()) w.Close();
ToolManager.SetActiveContext<GameObjectToolContext>(); Tools.current = Tool.Move;
var cam = Object.FindObjectsByType<Camera>().First().gameObject; Selection.activeGameObject = cam;
var sv = ScriptableObject.CreateInstance<SceneView>(); sv.Show(); Place(sv, new Rect(60, 60, 610, 470));
var preset = typeof(Editor).Assembly.GetType("UnityEditor.Overlays.OverlayPresetManager").GetMethod("GetDefaultPreset", BF).Invoke(null, new object[]{ typeof(SceneView) });
typeof(OverlayCanvas).GetMethods(BF).First(m => m.Name == "ApplyPreset").Invoke(sv.overlayCanvas, new[]{ preset });
foreach (var o in sv.overlayCanvas.overlays.Where(o => o.displayName == "Overlay Menu" || o.displayName == "Camera" || o.displayName == "Cameras")) o.displayed = false;
sv.drawGizmos = false; sv.sceneViewState.showSkybox = true;
sv.LookAtDirect(new Vector3(-2.5f, 3.5f, -0.5f), Quaternion.Euler(58, 30, 0), 11f);
var it = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
var iw = (EditorWindow)ScriptableObject.CreateInstance(it); iw.Show(); Place(iw, new Rect(700, 60, 300, 470));
foreach (var c in cam.GetComponents<Component>()) UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(c, c is Camera);
var ht = Resources.FindObjectsOfTypeAll<EditorWindow>().First(x => x.GetType().Name == "HierarchyWindow").GetType();
var hw = (EditorWindow)ScriptableObject.CreateInstance(ht); hw.Show(); Place(hw, new Rect(1020, 60, 210, 250));
var pt = typeof(Editor).Assembly.GetType("UnityEditor.ProjectBrowser");
var pw = (EditorWindow)ScriptableObject.CreateInstance(pt); pw.Show(); Place(pw, new Rect(1020, 330, 260, 220));
pt.GetMethod("SetOneColumn", BF)?.Invoke(pw, null);
EditorGUIUtility.PingObject(AssetDatabase.LoadMainAssetAtPath("Assets/Atrio/Environment/Pond"));
typeof(SceneView).GetField("s_LastActiveSceneView", BF).SetValue(null, sv); sv.Repaint();
return "ok cam=" + cam.name;
