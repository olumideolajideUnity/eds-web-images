using UnityEditor; using UnityEditor.Overlays; using UnityEngine; using UnityEngine.Splines; using UnityEditor.Splines; using UnityEditor.EditorTools; using Unity.Mathematics; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var sc = GameObject.Find("Spline").GetComponent<SplineContainer>();
var s = sc.Spline; s.Clear();
float3[] pts = { new float3(-7,0,-1.2f), new float3(-5.2f,0,2.4f), new float3(-2.8f,0,-0.6f), new float3(-1.2f,0,2.9f), new float3(1.4f,0,0.2f), new float3(3.0f,0,2.6f), new float3(5.2f,0,1.2f) };
foreach (var p in pts) s.Add(new BezierKnot(p), TangentMode.AutoSmooth);
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
var parent = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(sv);
var container = parent.GetType().GetProperty("window", BF).GetValue(parent);
container.GetType().GetProperty("position", BF).SetValue(container, new Rect(10, 60, 1500, 711));
sv.sceneViewState.showSkybox = false; sv.drawGizmos = true; sv.showGrid = true;
sv.LookAtDirect(new Vector3(-1.2f, 0, 0.9f), Quaternion.Euler(58f, 0f, 0f), 4.6f);
foreach (var o in sv.overlayCanvas.overlays) {
  if (o.displayName == "Orientation" || o.displayName == "Search" || o.displayName == "View Options" || o.displayName == "Grid and Snap") o.displayed = false;
}
Overlay tools = sv.overlayCanvas.overlays.First(o => o.displayName == "Tools");
Overlay insp = sv.overlayCanvas.overlays.First(o => o.id == "unity-spline-inspector");
tools.Undock(); var lp = typeof(Overlay).GetProperty("layout", BF); var ls = lp.GetSetMethod(true); if (ls != null) ls.Invoke(tools, new object[]{ Layout.HorizontalToolbar }); else typeof(Overlay).GetMethod("SetLayout", BF)?.Invoke(tools, new object[]{ Layout.HorizontalToolbar }); tools.floatingPosition = new Vector2(1500 - 12 - tools.rootVisualElement.layout.width, 34);
insp.displayed = true; insp.Undock(); insp.floatingPosition = new Vector2(1500 - 12 - Mathf.Max(insp.rootVisualElement.layout.width, 300), 34 + 40);
Selection.activeGameObject = sc.gameObject; sv.Focus(); sv.Repaint();
return "toolsW=" + tools.rootVisualElement.layout.width + " inspW=" + insp.rootVisualElement.layout.width + " layout=" + tools.layout + " sv=" + sv.position + " tools=" + tools.floatingPosition + " insp=" + insp.floatingPosition;
