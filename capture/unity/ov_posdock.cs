// Placement: Default preset view; Tool Settings floating mid-view (1); report rects of Tool Settings, top-toolbar Grid and Snap (2, edge),
// Orientation (3, corner) to OUT.
using UnityEditor; using UnityEditor.Overlays; using UnityEditor.EditorTools; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var cwT = typeof(Editor).Assembly.GetType("UnityEditor.ContainerWindow");
bool NotMain(EditorWindow x) { var p = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(x); var c = p?.GetType().GetProperty("window", BF)?.GetValue(p); return c != null && (int)cwT.GetProperty("showMode", BF).GetValue(c) != 4; }
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(NotMain); sv.Focus();
Selection.activeObject = null; ToolManager.SetActiveContext<GameObjectToolContext>(); Tools.current = Tool.Move;
var ts = sv.overlayCanvas.overlays.First(x => x.displayName == "Tool Settings"); ts.displayed = true; ts.Undock(); ts.collapsed = false;
typeof(Overlay).GetProperty("layout", BF).GetSetMethod(true).Invoke(ts, new object[]{ Layout.HorizontalToolbar }); ts.floatingPosition = new Vector2(330, 96);
sv.Repaint();
string R(string n) { var r = sv.overlayCanvas.overlays.First(x => x.displayName == n).rootVisualElement.worldBound; return "[" + r.x + "," + r.y + "," + r.width + "," + r.height + "]"; }
System.IO.File.WriteAllText("OUT", "{\"floating\":" + R("Tool Settings") + ",\"edge\":" + R("Grid and Snap") + ",\"corner\":" + R("Orientation") + "}");
return R("Tool Settings") + R("Grid and Snap") + R("Orientation");
