// Floating Scene view content W x H pt; MODE 0 = backdrop (all overlays hidden, no toolbar needed), 1 = default window look.
using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection;
int MODE = 0; float W = 800, H = 300; float PITCH = 22, YAW = -20, SIZE = 6f;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var sv = Resources.FindObjectsOfTypeAll<SceneView>().FirstOrDefault(w => !w.docked);
if (sv == null) { sv = ScriptableObject.CreateInstance<SceneView>(); sv.Show(); }
var parent = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(sv);
var container = parent.GetType().GetProperty("window", BF).GetValue(parent);
container.GetType().GetProperty("position", BF).SetValue(container, new Rect(100, 100, W, H + 24 + 26));
typeof(SceneView).GetField("s_LastActiveSceneView", BF).SetValue(null, sv);
var cs = sv.cameraSettings; cs.fieldOfView = 60; sv.cameraSettings = cs;
sv.sceneViewState.showSkybox = false; sv.showGrid = true; sv.drawGizmos = true; sv.in2DMode = false; sv.orthographic = false;
foreach (var o in sv.overlayCanvas.overlays) {
  o.rootVisualElement.style.display = StyleKeyword.Null;
  bool want = MODE == 1 && (new[]{ "Orientation", "Tools", "View Options", "Grid and Snap", "Draw Modes", "Search" }.Contains(o.displayName));
  o.displayed = want; o.rootVisualElement.style.display = want ? StyleKeyword.Null : DisplayStyle.None;
}
sv.LookAtDirect(new Vector3(0, 0.5f, 0), Quaternion.Euler(PITCH, YAW, 0), SIZE);
sv.Repaint();
return "sv=" + sv.position + " shown=" + string.Join(",", sv.overlayCanvas.overlays.Where(o => o.displayed).Select(o => o.displayName));
