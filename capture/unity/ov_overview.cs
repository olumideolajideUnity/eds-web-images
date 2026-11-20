// Overlays overview: ProBuilder's Create Cube tool active (its Shape Settings overlay floating as a panel at top-left), Tool Settings
// floating beside it, Tools floating vertically at the right, as in the original.
using UnityEditor; using UnityEditor.Overlays; using UnityEditor.EditorTools; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var cwT = typeof(Editor).Assembly.GetType("UnityEditor.ContainerWindow");
bool NotMain(EditorWindow x) { var p = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(x); var c = p?.GetType().GetProperty("window", BF)?.GetValue(p); return c != null && (int)cwT.GetProperty("showMode", BF).GetValue(c) != 4; }
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(NotMain);
typeof(SceneView).GetField("s_LastActiveSceneView", BF).SetValue(null, sv); sv.Focus();
Selection.activeObject = null;
var cube = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("UnityEditor.ProBuilder.CreateCubeTool")).First(x => x != null);
ToolManager.SetActiveTool(cube);
void Float(string name, Layout l, Vector2 p) { var o = sv.overlayCanvas.overlays.FirstOrDefault(x => x.displayName == name); if (o == null) return; o.displayed = true; o.collapsed = false; o.Undock(); typeof(Overlay).GetProperty("layout", BF).GetSetMethod(true).Invoke(o, new object[]{ l }); o.floatingPosition = p; }
foreach (var t in sv.overlayCanvas.overlays.Where(x => x.displayName == "Terrain Tools")) t.displayed = false;
Float("Shape Settings", Layout.Panel, new Vector2(12, 60));
Float("Tool Settings", Layout.HorizontalToolbar, new Vector2(330, 84));
Float("Tools", Layout.VerticalToolbar, new Vector2(612, 62));
sv.Repaint();
return "tool=" + ToolManager.activeToolType?.Name + " shown=" + string.Join(",", sv.overlayCanvas.overlays.Where(o => o.displayed).Select(o => o.displayName + (o.floating ? "*" : "")));
