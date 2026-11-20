// Overlay anatomy: Tools floating as a panel, Overlay Menu floating as a panel; report rects (window pt) of the parts to OUT.
using UnityEditor; using UnityEditor.Overlays; using UnityEditor.EditorTools; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var cwT = typeof(Editor).Assembly.GetType("UnityEditor.ContainerWindow");
bool NotMain(EditorWindow x) { var p = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(x); var c = p?.GetType().GetProperty("window", BF)?.GetValue(p); return c != null && (int)cwT.GetProperty("showMode", BF).GetValue(c) != 4; }
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(NotMain); sv.Focus(); Selection.activeObject = null;
void Float(string name, Layout l, Vector2 p) { var o = sv.overlayCanvas.overlays.First(x => x.displayName == name); o.displayed = true; o.Undock(); o.collapsed = false; typeof(Overlay).GetProperty("layout", BF).GetSetMethod(true).Invoke(o, new object[]{ l }); o.floatingPosition = p; }
Float("Tools", Layout.Panel, new Vector2(40, 74));
Float("Overlay Menu", Layout.Panel, new Vector2(520, 50));
sv.Repaint();
var t = sv.overlayCanvas.overlays.First(x => x.displayName == "Tools").rootVisualElement;
string R(VisualElement e) { if (e == null) return "null"; var r = e.worldBound; return "[" + r.x + "," + r.y + "," + r.width + "," + r.height + "]"; }
var drag = t.Q(name: (string)typeof(Overlay).GetField("k_DraggerName", BF).GetValue(null));
var label = t.Query<Label>().ToList().FirstOrDefault(l => l.text == "Tools");
var icon = label?.parent?.Children().ElementAtOrDefault(2);
var content = t.Query<VisualElement>().ToList().FirstOrDefault(e => e.GetClasses().Any(c => c.Contains("content")) && e.worldBound.height > 10);
var tb = sv.overlayCanvas.overlays.First(x => x.displayName == "Grid and Snap").rootVisualElement.parent;
var menu = sv.overlayCanvas.overlays.First(x => x.displayName == "Overlay Menu").rootVisualElement;
var hdr = label?.parent;
string cls = hdr == null ? "" : string.Join(";", hdr.Children().Select(c => c.GetType().Name + "." + string.Join(".", c.GetClasses())));
System.IO.File.WriteAllText("OUT", "{\"container\":" + R(t) + ",\"toolbar\":" + R(tb) + ",\"dragger\":" + R(drag) + ",\"label\":" + R(label) + ",\"icon\":" + R(icon) + ",\"content\":" + R(content) + ",\"menu\":" + R(menu) + "}");
return "hdr=" + cls;
