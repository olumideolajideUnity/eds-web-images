// Overlay layout modes: Tools collapsed (1), Particles panel (2), Draw Modes vertical (3), Grid and Snap horizontal floating (4); the
// top toolbar keeps View Options docked (5). Reports rects (pt, window-content coords) to OUT.
using UnityEditor; using UnityEditor.Overlays; using UnityEditor.EditorTools; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var cwT = typeof(Editor).Assembly.GetType("UnityEditor.ContainerWindow");
bool NotMain(EditorWindow x) { var p = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(x); var c = p?.GetType().GetProperty("window", BF)?.GetValue(p); return c != null && (int)cwT.GetProperty("showMode", BF).GetValue(c) != 4; }
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(NotMain); sv.Focus();
Selection.activeGameObject = Object.FindObjectsByType<ParticleSystem>(FindObjectsSortMode.None).OrderBy(p => p.name).First().gameObject;
foreach (var o in sv.overlayCanvas.overlays.Where(x => x.displayName == "Overlay Menu" || x.displayName == "Shape Settings" || x.displayName == "Tool Settings")) o.displayed = false;
Overlay F(string name, Layout l, Vector2 p, bool col = false) { var o = sv.overlayCanvas.overlays.First(x => x.displayName == name); o.displayed = true; o.Undock(); typeof(Overlay).GetProperty("layout", BF).GetSetMethod(true).Invoke(o, new object[]{ l }); o.floatingPosition = p; o.collapsed = col; return o; }
var a = F("Tools", Layout.HorizontalToolbar, new Vector2(14, 34), true);
var b = F("Particles", Layout.Panel, new Vector2(84, 34));
var c = F("Draw Modes", Layout.VerticalToolbar, new Vector2(340, 70));
var d = F("Grid and Snap", Layout.HorizontalToolbar, new Vector2(392, 34));
sv.Repaint();
string R(Overlay o) { var r = o.rootVisualElement.worldBound; return "[" + r.x + "," + r.y + "," + r.width + "," + r.height + "]"; }
var vo = sv.overlayCanvas.overlays.First(x => x.displayName == "View Options");
System.IO.File.WriteAllText("OUT", "{\"collapsed\":" + R(a) + ",\"panel\":" + R(b) + ",\"vertical\":" + R(c) + ",\"horizontal\":" + R(d) + ",\"toolbar\":" + R(vo) + "}");
return R(a) + R(b) + R(c) + R(d) + R(vo);
