// Contextual tooling #6 retake at the original's scale: Scene view content 800x360pt (-> 1600x720 at 2x), Tools + Tool Settings
// floating top-left, three-knot Bezier spline with broken tangents laid out like the original, knot [1] selected (Move).
using UnityEditor; using UnityEditor.Overlays; using UnityEditor.SceneManagement; using UnityEngine; using UnityEngine.Splines; using UnityEditor.Splines; using UnityEditor.EditorTools; using Unity.Mathematics; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
float W = 800, H = 360 + 30;                       // + Scene toolbar strip (cropped later)
float PITCH = 35, YAW = -30, SIZE = 2.3f; float3 PIV = new float3(-1.2f, 0, 0.45f);
if (EditorSceneManager.GetActiveScene().path != "Assets/EFDS_Screenshots/Splines.unity") EditorSceneManager.OpenScene("Assets/EFDS_Screenshots/Splines.unity");
var sv = Resources.FindObjectsOfTypeAll<SceneView>().FirstOrDefault(w => !w.docked);
if (sv == null) { sv = ScriptableObject.CreateInstance<SceneView>(); sv.Show(); sv.position = new Rect(150, 120, W, H); }
var host = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(sv);
var cw = host.GetType().GetProperty("window", BF).GetValue(host);
var r = (Rect)cw.GetType().GetProperty("position", BF).GetValue(cw);
cw.GetType().GetProperty("position", BF).SetValue(cw, new Rect(r.x, r.y, W, H + 26));
var sc = GameObject.Find("Spline").GetComponent<SplineContainer>();
var s = sc.Spline; s.Clear();
var Ry = quaternion.RotateY(math.radians(YAW));   // spline turns with the camera so its on-screen shape stays put
BezierKnot K(float3 p, float3 tin, float3 tout) => new BezierKnot(math.mul(Ry, p), math.mul(Ry, tin), math.mul(Ry, tout), quaternion.identity);
s.Add(K(new float3(-2.8f, 0, -1.2f), new float3(-1.0f, 0, 0.4f), new float3(1.8f, 0, -0.5f)), TangentMode.Broken);
s.Add(K(new float3(0.4f, 0, 2.0f), new float3(-1.3f, 0, -0.4f), new float3(1.1f, 0, 0.4f)), TangentMode.Broken);
s.Add(K(new float3(2.7f, 0, -0.9f), new float3(-2.2f, 0, -0.4f), new float3(1.0f, 0, 0.3f)), TangentMode.Broken);
var tools = sv.overlayCanvas.overlays.First(o => o.displayName == "Tools");
var ts = sv.overlayCanvas.overlays.First(o => o.displayName == "Tool Settings");
foreach (var o in new[]{ tools, ts }) { o.displayed = true; o.collapsed = false; o.Undock(); typeof(Overlay).GetProperty("layout", BF).GetSetMethod(true)?.Invoke(o, new object[]{ Layout.HorizontalToolbar }); }
tools.floatingPosition = new Vector2(35, 61); ts.floatingPosition = new Vector2(35, 110);
foreach (var o in sv.overlayCanvas.overlays) if (o.id == "unity-spline-inspector" || o.displayName == "Element Inspector") { o.displayed = false; o.rootVisualElement.style.display = UnityEngine.UIElements.DisplayStyle.None; }
Selection.activeGameObject = sc.gameObject;
if (ToolManager.activeContextType != typeof(SplineToolContext)) ToolManager.SetActiveContext<SplineToolContext>();
Tools.current = Tool.Move;
SplineSelection.Clear(); SplineSelection.Add(new SelectableKnot(new SplineInfo(sc, 0), 1));
sv.LookAtDirect((Vector3)math.mul(Ry, PIV), Quaternion.Euler(PITCH, YAW, 0), SIZE);
sv.Repaint();
return "ok " + sv.position + " tool=" + ToolManager.activeToolType?.Name;
