// Accessibility a11y-5: fresh floating Scene view (W x H pt incl. tab) on Atrio, default preset, every overlay hidden except
// NAME, floated at POS (pt, viewport coords) in LAYOUT. SHOW=false hides it too (for the see-through 'don't' version).
using UnityEditor; using UnityEditor.Overlays; using UnityEditor.SceneManagement; using UnityEngine; using System.Linq; using System.Reflection;
float W = 420, H = 300; string NAME = "Tool Settings"; string LAYOUT = "Panel"; Vector2 POS = new Vector2(150, 110); bool SHOW = true; bool FRESH = true;
Vector3 PIV = new Vector3(-2.8f, 4.0f, -0.8f); float PITCH = 18, YAW = 35, SIZE = 6f;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
if (EditorSceneManager.GetActiveScene().path != "Assets/Atrio/AtrioScenes/Atrio.unity") EditorSceneManager.OpenScene("Assets/Atrio/AtrioScenes/Atrio.unity");
Selection.activeObject = null;
var cwT = typeof(Editor).Assembly.GetType("UnityEditor.ContainerWindow");
bool NotMain(EditorWindow x) { var p = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(x); var c = p?.GetType().GetProperty("window", BF)?.GetValue(p); return c != null && (int)cwT.GetProperty("showMode", BF).GetValue(c) != 4; }
SceneView sv;
if (FRESH) {
  foreach (var w in Resources.FindObjectsOfTypeAll<EditorWindow>().Where(x => (x is SceneView || x.GetType().Name == "GameView") && NotMain(x)).ToList()) w.Close();
  sv = ScriptableObject.CreateInstance<SceneView>(); sv.Show();
  var dock = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(sv); var cw = dock.GetType().GetProperty("window", BF).GetValue(dock);
  cw.GetType().GetProperty("position", BF).SetValue(cw, new Rect(100, 100, W, H));
  var mgr = typeof(Editor).Assembly.GetType("UnityEditor.Overlays.OverlayPresetManager");
  typeof(OverlayCanvas).GetMethods(BF).First(m => m.Name == "ApplyPreset").Invoke(sv.overlayCanvas, new[]{ mgr.GetMethod("GetDefaultPreset", BF).Invoke(null, new object[]{ typeof(SceneView) }) });
  sv.sceneViewState.showSkybox = true; sv.showGrid = false; sv.drawGizmos = false;
  var cs = sv.cameraSettings; cs.fieldOfView = 60; sv.cameraSettings = cs;
  sv.LookAtDirect(PIV, Quaternion.Euler(PITCH, YAW, 0), SIZE);
} else sv = Resources.FindObjectsOfTypeAll<SceneView>().First(NotMain);
foreach (var o in sv.overlayCanvas.overlays) if (o.displayName != NAME) o.displayed = false;
var ov = sv.overlayCanvas.overlays.First(x => x.displayName == NAME);
ov.displayed = SHOW;
if (SHOW) { ov.Undock(); typeof(Overlay).GetProperty("layout", BF).GetSetMethod(true).Invoke(ov, new object[]{ System.Enum.Parse(typeof(Layout), LAYOUT) }); ov.collapsed = false; ov.floatingPosition = POS; }
typeof(SceneView).GetField("s_LastActiveSceneView", BF).SetValue(null, sv);
sv.Repaint();
var r = ov.rootVisualElement.worldBound;
return "all=" + string.Join(",", sv.overlayCanvas.overlays.Select(o => o.displayName)) + " | rect=" + r;
