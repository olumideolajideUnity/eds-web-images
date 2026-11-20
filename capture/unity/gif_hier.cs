// Search filters GIF: unsaved demo objects in the open scene (Canvas with two Text children, EventSystem, Sphere, Capsule, Point Light),
// a floating Unity 7 Hierarchy (W x H pt, search cleared, everything expanded) beside a floating Scene view (SW x H pt).
using UnityEditor; using UnityEngine; using System.Linq;
float W = 300, H = 300, SW = 500;
foreach (var n in new[]{ "Canvas", "EventSystem", "Sphere", "Capsule", "Point Light" }) { var o = GameObject.Find(n); if (o) Object.DestroyImmediate(o); }
var canvas = new GameObject("Canvas", typeof(Canvas)); new GameObject("Text (TMP)").transform.SetParent(canvas.transform); new GameObject("Text (TMP)").transform.SetParent(canvas.transform);
new GameObject("EventSystem"); GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = new Vector3(2, 0.5f, 0); GameObject.CreatePrimitive(PrimitiveType.Capsule).transform.position = new Vector3(-2, 1, 0);
var pl = new GameObject("Point Light"); pl.AddComponent<Light>().type = LightType.Point; pl.transform.position = new Vector3(0, 2, -1);
Selection.activeObject = null;
var t = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("Unity.Hierarchy.Editor.HierarchyWindow")).First(x => x != null);
var w = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().FirstOrDefault(x => !x.docked);
if (w == null) { w = (EditorWindow)ScriptableObject.CreateInstance(t); w.Show(); }
var BF2 = System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic;
foreach (var g in Resources.FindObjectsOfTypeAll<GameObject>().Where(x => x.name.StartsWith("EFDS GIF")).ToList()) Object.DestroyImmediate(g);
void Size(EditorWindow ew, Rect r) { var d = typeof(EditorWindow).GetField("m_Parent", BF2).GetValue(ew); var cw = d.GetType().GetProperty("window", BF2).GetValue(d); cw.GetType().GetProperty("position", BF2).SetValue(cw, r); }
Size(w, new Rect(100, 100, W, H + 26));
var sst = t.GetMethods(System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic).First(m => m.Name == "SetSearchText" && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(string)); sst.Invoke(w, new object[]{ "" });
var sv = Resources.FindObjectsOfTypeAll<SceneView>().FirstOrDefault(x => !x.docked);
if (sv == null) { sv = ScriptableObject.CreateInstance<SceneView>(); sv.Show(); }
Size(sv, new Rect(100 + W, 100, SW, H + 26)); sv.drawGizmos = true; sv.sceneViewState.showSkybox = false;
foreach (var o in sv.overlayCanvas.overlays) if (o.displayName == "Tools" || o.displayName == "Tool Settings") o.displayed = false;
sv.LookAtDirect(new Vector3(0, 0.6f, 0), Quaternion.Euler(22, 25, 0), 3.2f); sv.Repaint(); w.Repaint();
return "hier " + w.position + " sv " + sv.position;
