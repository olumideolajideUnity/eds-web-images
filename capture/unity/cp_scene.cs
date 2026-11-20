// Color palette > Text Colors Base 3: clean default floating Scene window (W x H pt incl. tab) on a new default scene (camera + sun, both behind the view, like the original).
// Scene view, apply the overlay canvas's "Default" preset, size W x H (incl. tab strip), frame the light.
using UnityEditor; using UnityEditor.Overlays; using UnityEditor.SceneManagement; using UnityEngine; using System.Linq; using System.Reflection;
float W = 456, H = 243;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var cwT = typeof(Editor).Assembly.GetType("UnityEditor.ContainerWindow");
bool NotMain(EditorWindow x) { var p = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(x); var c = p?.GetType().GetProperty("window", BF)?.GetValue(p); return c != null && (int)cwT.GetProperty("showMode", BF).GetValue(c) != 4; }
if (GameObject.Find("Directional Light") == null || EditorSceneManager.GetActiveScene().rootCount != 2) EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
Selection.activeObject = null;
foreach (var w in Resources.FindObjectsOfTypeAll<EditorWindow>().Where(x => (x is SceneView || x.GetType().Name == "GameView") && NotMain(x)).ToList()) w.Close();
var sv = ScriptableObject.CreateInstance<SceneView>(); sv.Show();
var dock = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(sv);
var cw = dock.GetType().GetProperty("window", BF).GetValue(dock);
cw.GetType().GetProperty("position", BF).SetValue(cw, new Rect(100, 100, W, H));
var mgr = typeof(Editor).Assembly.GetType("UnityEditor.Overlays.OverlayPresetManager");
var get = mgr.GetMethods(BF).FirstOrDefault(m => m.Name == "GetDefaultPreset" || (m.Name == "GetPreset" && m.GetParameters().Length == 2));
object preset = get.GetParameters().Length == 1 ? get.Invoke(null, new object[]{ typeof(SceneView) }) : get.Invoke(null, new object[]{ typeof(SceneView), "Default" });
if (preset != null) typeof(OverlayCanvas).GetMethods(BF).First(m => m.Name == "ApplyPreset").Invoke(sv.overlayCanvas, new[]{ preset });
typeof(SceneView).GetField("s_LastActiveSceneView", BF).SetValue(null, sv);
sv.sceneViewState.showSkybox = true; sv.showGrid = true; sv.drawGizmos = true; sv.in2DMode = false;
var cs = sv.cameraSettings; cs.fieldOfView = 60; sv.cameraSettings = cs;
sv.LookAtDirect(new Vector3(-30, 0, -30), Quaternion.Euler(12, 225, 0), 10f); sv.Repaint();
return "preset=" + (preset != null) + " via " + get.Name + " shown=" + string.Join(",", sv.overlayCanvas.overlays.Where(o => o.displayed).Select(o => o.displayName + (o.floating ? "*" : "")));
