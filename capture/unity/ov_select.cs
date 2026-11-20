// Select WHAT in the Atrio scene: "none", "camera" (the scene Camera), "particles" (first particle system).
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
string WHAT = "none";
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var cwT = typeof(Editor).Assembly.GetType("UnityEditor.ContainerWindow");
bool NotMain(EditorWindow x) { var p = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(x); var c = p?.GetType().GetProperty("window", BF)?.GetValue(p); return c != null && (int)cwT.GetProperty("showMode", BF).GetValue(c) != 4; }
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(NotMain); sv.Focus();
GameObject g = WHAT == "camera" ? Object.FindObjectsByType<Camera>(FindObjectsSortMode.None).First().gameObject : WHAT == "particles" ? Object.FindObjectsByType<ParticleSystem>(FindObjectsSortMode.None).OrderBy(p => p.name).First().gameObject : null;
Selection.activeGameObject = g; sv.Repaint();
return WHAT + " -> " + g + " shown=" + string.Join(",", sv.overlayCanvas.overlays.Where(o => o.displayed && o.rootVisualElement.worldBound.width > 0).Select(o => o.displayName));
