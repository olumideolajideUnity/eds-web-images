// Overlays base: Atrio scene; a fresh floating Scene view (default overlay preset) with a Game tab docked beside it, the whole
// dock area W x H pt (incl. tab strip), Scene tab selected, camera framed on the courtyard.
using UnityEditor; using UnityEditor.SceneManagement; using UnityEngine; using System.Linq; using System.Reflection;
float W = 656, H = 230; Vector3 PIV = new Vector3(0, 1.0f, 0); float PITCH = 6, YAW = 0, SIZE = 8f;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
if (EditorSceneManager.GetActiveScene().path != "Assets/EFDS_Screenshots/Windowing.unity") EditorSceneManager.OpenScene("Assets/EFDS_Screenshots/Windowing.unity");
Selection.activeObject = null;
var cwT = typeof(Editor).Assembly.GetType("UnityEditor.ContainerWindow");
bool NotMain(EditorWindow x) { var p = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(x); var c = p?.GetType().GetProperty("window", BF)?.GetValue(p); return c != null && (int)cwT.GetProperty("showMode", BF).GetValue(c) != 4; }
foreach (var w in Resources.FindObjectsOfTypeAll<EditorWindow>().Where(x => (x is SceneView || x.GetType().Name == "GameView") && NotMain(x)).ToList()) w.Close();
var sv = ScriptableObject.CreateInstance<SceneView>(); sv.Show();
var dock = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(sv);
var gv = (EditorWindow)ScriptableObject.CreateInstance(typeof(Editor).Assembly.GetType("UnityEditor.GameView"));
dock.GetType().GetMethods(BF).First(m => m.Name == "AddTab" && m.GetParameters().Length == 2).Invoke(dock, new object[]{ gv, false });
dock.GetType().GetProperty("selected", BF)?.SetValue(dock, 1);
var cw = dock.GetType().GetProperty("window", BF).GetValue(dock);
cw.GetType().GetProperty("position", BF).SetValue(cw, new Rect(100, 100, W, H));
var mgr = typeof(Editor).Assembly.GetType("UnityEditor.Overlays.OverlayPresetManager");
var preset = mgr.GetMethod("GetDefaultPreset", BF).Invoke(null, new object[]{ typeof(SceneView) });
typeof(UnityEditor.Overlays.OverlayCanvas).GetMethods(BF).First(m => m.Name == "ApplyPreset").Invoke(sv.overlayCanvas, new[]{ preset });
typeof(SceneView).GetField("s_LastActiveSceneView", BF).SetValue(null, sv);
sv.sceneViewState.showSkybox = true; sv.showGrid = true; sv.drawGizmos = true;
var cs = sv.cameraSettings; cs.fieldOfView = 60; sv.cameraSettings = cs;
sv.LookAtDirect(PIV, Quaternion.Euler(PITCH, YAW, 0), SIZE);
sv.Focus(); sv.Repaint();
return "tabs ok, shown=" + string.Join(",", sv.overlayCanvas.overlays.Where(o => o.displayed).Select(o => o.displayName));
