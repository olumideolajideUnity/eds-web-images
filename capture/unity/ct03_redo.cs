// Contextual tooling #3 at the original's scale: 1390x611pt Scene view (overlays ~1.15x), an 8-knot spline laid out from the
// original's knot positions (screen px mapped onto the XZ plane under a steep, narrow-FOV camera), Knot 5 Bezier Mirrored and selected.
using UnityEditor; using UnityEditor.Overlays; using UnityEditor.SceneManagement; using UnityEngine; using UnityEngine.Splines; using UnityEditor.Splines; using UnityEditor.EditorTools; using Unity.Mathematics; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
float W = 1390, H = 611; float PITCH = 58, SIZE = 4.2f, FOV = 30, S = 150f; float3 PIV = new float3(0, 0, 0.2f);
float2[] scr = { new float2(197,176), new float2(163,446), new float2(472,242), new float2(725,467), new float2(1212,297), new float2(1013,298), new float2(695,213), new float2(370,580) };
if (EditorSceneManager.GetActiveScene().path != "Assets/EFDS_Screenshots/Splines.unity") EditorSceneManager.OpenScene("Assets/EFDS_Screenshots/Splines.unity");
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
var parent = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(sv);
var container = parent.GetType().GetProperty("window", BF).GetValue(parent);
container.GetType().GetProperty("position", BF).SetValue(container, new Rect(40, 60, W, H + 24 + 26));
typeof(SceneView).GetField("s_LastActiveSceneView", BF).SetValue(null, sv);
var sc = GameObject.Find("Spline").GetComponent<SplineContainer>();
var s = sc.Spline; s.Clear();
float zk = 1f / Mathf.Sin(PITCH * Mathf.Deg2Rad);
float3 P(float2 p) => new float3((p.x - 800) / S, 0, (351 - p.y) / S * zk);
for (int i = 0; i < scr.Length; i++) s.Add(new BezierKnot(P(scr[i])), TangentMode.AutoSmooth);
// Knot 5: Bezier Mirrored, tangents running up/down the screen like the original's
s.SetTangentMode(5, TangentMode.Mirrored);
var k5 = s[5]; k5.TangentIn = new float3(0, 0, 1.1f * zk); k5.TangentOut = new float3(0, 0, -1.1f * zk); k5.Rotation = quaternion.identity; s[5] = k5;
sv.sceneViewState.showSkybox = false; sv.drawGizmos = true; sv.showGrid = true;
var cs = sv.cameraSettings; cs.fieldOfView = FOV; sv.cameraSettings = cs;
sv.LookAtDirect((Vector3)PIV, Quaternion.Euler(PITCH, 0, 0), SIZE);
foreach (var o in sv.overlayCanvas.overlays) if (new[]{ "Orientation", "Search", "View Options", "Grid and Snap", "Cameras", "Tool Settings", "Bevel Edges" }.Contains(o.displayName)) o.displayed = false;
var tools = sv.overlayCanvas.overlays.First(o => o.displayName == "Tools"); tools.displayed = true; tools.Undock();
typeof(Overlay).GetProperty("layout", BF).GetSetMethod(true)?.Invoke(tools, new object[]{ Layout.HorizontalToolbar });
var insp = sv.overlayCanvas.overlays.First(o => o.id == "unity-spline-inspector"); insp.displayed = true; insp.Undock(); insp.collapsed = false;
insp.rootVisualElement.style.display = UnityEngine.UIElements.DisplayStyle.Flex;
tools.floatingPosition = new Vector2(W - 51 - Mathf.Max(tools.rootVisualElement.layout.width, 380), 89 + 24);
insp.floatingPosition = new Vector2(1340 - Mathf.Max(insp.rootVisualElement.layout.width, 260), 148 + 24);
Selection.activeGameObject = sc.gameObject;
if (ToolManager.activeContextType != typeof(SplineToolContext)) ToolManager.SetActiveContext<SplineToolContext>();
Tools.current = Tool.Move;
SplineSelection.Clear(); SplineSelection.Add(new SelectableKnot(new SplineInfo(sc, 0), 5));
sv.Focus(); sv.Repaint();
return "toolsW=" + tools.rootVisualElement.layout.width + " inspW=" + insp.rootVisualElement.layout.width + " sel=" + SplineSelection.Count;
