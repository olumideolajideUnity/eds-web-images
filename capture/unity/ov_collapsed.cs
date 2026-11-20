// Collapsed overlays: select one of Atrio's particle systems (its Particles overlay appears), float Tools and Particles collapsed side by side.
using UnityEditor; using UnityEditor.Overlays; using UnityEditor.EditorTools; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var cwT = typeof(Editor).Assembly.GetType("UnityEditor.ContainerWindow");
bool NotMain(EditorWindow x) { var p = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(x); var c = p?.GetType().GetProperty("window", BF)?.GetValue(p); return c != null && (int)cwT.GetProperty("showMode", BF).GetValue(c) != 4; }
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(NotMain); sv.Focus();
ToolManager.SetActiveContext<GameObjectToolContext>(); Tools.current = Tool.Move;
var ps = Object.FindObjectsByType<ParticleSystem>(FindObjectsSortMode.None).OrderBy(p => p.name).First();
Selection.activeGameObject = ps.gameObject;
string r = "ps=" + ps.name;
foreach (var name in new[]{ "Shape Settings", "Tool Settings" }) foreach (var o in sv.overlayCanvas.overlays.Where(x => x.displayName == name)) o.displayed = false;
float x0 = 12;
foreach (var name in new[]{ "Tools", "Particles" }) {
  var o = sv.overlayCanvas.overlays.FirstOrDefault(x => x.displayName == name); if (o == null) { r += " no " + name; continue; }
  o.displayed = true; o.Undock(); o.floatingPosition = new Vector2(x0, 56); o.collapsed = true; x0 += 50;
}
sv.Repaint();
return r + " shown=" + string.Join(",", sv.overlayCanvas.overlays.Where(o => o.displayed).Select(o => o.displayName + (o.collapsed ? "(c)" : "")));
