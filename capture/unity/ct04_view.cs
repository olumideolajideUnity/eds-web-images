// Shared CT retake framing: floating Scene view content W x H pt (plus the 24pt Scene toolbar strip that scene_crop removes),
// Tools overlay + optional SECOND overlay floating at the original's positions (pt, measured below the toolbar), camera on TARGET.
using UnityEditor; using UnityEditor.Overlays; using UnityEngine; using System.Linq; using System.Reflection;
float W = 914, H = 434; float TX = 35, TY = 53, OX = 35, OY = 96; string SECOND = "Shape Settings";
string TARGET = "Stairs"; float PITCH = 33, YAW = 30, SIZE = 2.8f, RIGHT = 2.9f, UP = -0.55f;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var sv = Resources.FindObjectsOfTypeAll<SceneView>().FirstOrDefault(w => !w.docked);
if (sv == null) { sv = ScriptableObject.CreateInstance<SceneView>(); sv.Show(); }
var parent = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(sv);
var container = parent.GetType().GetProperty("window", BF).GetValue(parent);
container.GetType().GetProperty("position", BF).SetValue(container, new Rect(150, 100, W, H + 24 + 26));
typeof(SceneView).GetField("s_LastActiveSceneView", BF).SetValue(null, sv);
sv.sceneViewState.showSkybox = false; sv.drawGizmos = true; sv.showGrid = true;
foreach (var o in sv.overlayCanvas.overlays)
  if (new[]{ "Orientation", "Search", "View Options", "Grid and Snap", "Cameras", "Tool Settings" }.Contains(o.displayName) && !SECOND.Contains(o.displayName)) o.displayed = false;
var tools = sv.overlayCanvas.overlays.First(o => o.displayName == "Tools");
tools.displayed = true; tools.Undock();
typeof(Overlay).GetProperty("layout", BF).GetSetMethod(true)?.Invoke(tools, new object[]{ Layout.HorizontalToolbar });
tools.floatingPosition = new Vector2(TX, TY + 24);
string res = "tools=" + tools.floatingPosition;
if (SECOND.Length > 0) {
  var o2 = sv.overlayCanvas.overlays.First(o => o.displayName.Contains(SECOND));
  o2.displayed = true; o2.Undock(); o2.collapsed = false; o2.floatingPosition = new Vector2(OX, OY + 24);
  res += " second=" + o2.displayName;
}
if (TARGET.Length > 0) {
  var b = GameObject.Find(TARGET).GetComponent<Renderer>().bounds; var rot = Quaternion.Euler(PITCH, YAW, 0);
  sv.LookAtDirect(b.center - rot * Vector3.right * RIGHT + new Vector3(0, UP, 0), rot, SIZE);
}
sv.Focus(); sv.Repaint();
return res + " sv=" + sv.position;
