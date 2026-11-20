// Overlay menus: Tools floating as a panel at the top-left; the Overlay Menu overlay floated as a panel beside it.
using UnityEditor; using UnityEditor.Overlays; using UnityEditor.EditorTools; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var cwT = typeof(Editor).Assembly.GetType("UnityEditor.ContainerWindow");
bool NotMain(EditorWindow x) { var p = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(x); var c = p?.GetType().GetProperty("window", BF)?.GetValue(p); return c != null && (int)cwT.GetProperty("showMode", BF).GetValue(c) != 4; }
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(NotMain); sv.Focus();
Selection.activeObject = null;
void Float(string name, Layout l, Vector2 p) { var o = sv.overlayCanvas.overlays.First(x => x.displayName == name); o.displayed = true; o.Undock(); o.collapsed = false; typeof(Overlay).GetProperty("layout", BF).GetSetMethod(true).Invoke(o, new object[]{ l }); o.floatingPosition = p; }
Float("Tools", Layout.Panel, new Vector2(14, 60));
Float("Overlay Menu", Layout.Panel, new Vector2(612, 112));
sv.Repaint();
var t = sv.overlayCanvas.overlays.First(x => x.displayName == "Tools");
var ou = typeof(Overlay).Assembly.GetType("UnityEditor.Overlays.OverlayUtilities");
bool rs = (bool)ou.GetMethod("IsResizable", BF).Invoke(null, new object[]{ t });
bool inTb = (bool)typeof(Overlay).GetProperty("isInToolbar", BF).GetValue(t);
var sl = (Layout)typeof(Overlay).GetProperty("supportedLayouts", BF).GetValue(t);
var items = "Hide|" + ((bool)typeof(Overlay).GetProperty("userControlledVisibility", BF).GetValue(t) ? "Normal" : "Disabled") + (char)10 + "Collapse|Normal" + (char)10;
if (!inTb) { if (rs) items += "Reset Size|Normal" + (char)10; items += "---|" + (char)10;
  if ((sl & (Layout.HorizontalToolbar | Layout.VerticalToolbar)) != 0) items += "Panel|" + (t.layout == Layout.Panel ? "Checked" : "Normal") + (char)10;
  if ((sl & Layout.HorizontalToolbar) != 0) items += "Horizontal|" + (t.layout == Layout.HorizontalToolbar ? "Checked" : "Normal") + (char)10;
  if ((sl & Layout.VerticalToolbar) != 0) items += "Vertical|" + (t.layout == Layout.VerticalToolbar ? "Checked" : "Normal") + (char)10; }
System.IO.File.WriteAllText("OUT", items);
return "resizable=" + rs + " tools=" + t.rootVisualElement.worldBound + " layout=" + t.layout + " menu=" + sv.overlayCanvas.overlays.First(x => x.displayName == "Overlay Menu").rootVisualElement.worldBound;
