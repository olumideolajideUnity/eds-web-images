using UnityEditor; using UnityEditor.Overlays; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
// layout: Tools horizontal top-left, Tool Settings floating beneath it, both like the original
var tools = sv.overlayCanvas.overlays.First(o => o.displayName == "Tools");
var ts = sv.overlayCanvas.overlays.First(o => o.displayName == "Tool Settings");
foreach (var o in new[]{ tools, ts }) { o.displayed = true; o.Undock(); typeof(Overlay).GetProperty("layout", BF).GetSetMethod(true)?.Invoke(o, new object[]{ Layout.HorizontalToolbar }); }
tools.floatingPosition = new Vector2(30, 34); ts.floatingPosition = new Vector2(30, 34 + 46);
var sc = GameObject.Find("Spline").GetComponent<UnityEngine.Splines.SplineContainer>();
var s = sc.Spline; s.Clear();
foreach (var p in new[]{ new Unity.Mathematics.float3(-3.2f,0,-1.6f), new Unity.Mathematics.float3(0.6f,0,1.4f), new Unity.Mathematics.float3(4.2f,0,-1.0f) }) s.Add(new UnityEngine.Splines.BezierKnot(p), UnityEngine.Splines.TangentMode.AutoSmooth);
sv.LookAtDirect(new Vector3(1.4f, 0, 0.2f), Quaternion.Euler(40f, 0f, 0f), 3.4f);
sv.Repaint();
return "ok";
