// Drive an overlay's own OverlayDragger with UI Toolkit mouse events (no OS input): MODE 0 = press on its dragger and move to TO
// (window-content pt, i.e. panel coords) and stay mid-drag; MODE 1 = cancel the drag (back where it was).
using UnityEditor; using UnityEditor.Overlays; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection;
int MODE = 0; string OVERLAY = "Tools"; Vector2 TO = new Vector2(300, 150);
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var cwT = typeof(Editor).Assembly.GetType("UnityEditor.ContainerWindow");
bool NotMain(EditorWindow x) { var p = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(x); var c = p?.GetType().GetProperty("window", BF)?.GetValue(p); return c != null && (int)cwT.GetProperty("showMode", BF).GetValue(c) != 4; }
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(NotMain);
var o = sv.overlayCanvas.overlays.First(x => x.displayName == OVERLAY);
var dT = typeof(Overlay).Assembly.GetType("UnityEditor.Overlays.OverlayDragger");
var df = typeof(Overlay).GetFields(BF).FirstOrDefault(f => f.FieldType == dT);
object drag = df?.GetValue(o);
var dEl = o.rootVisualElement.Q(name: (string)typeof(Overlay).GetField("k_DraggerName", BF).GetValue(null));
if (drag == null) { var mf = typeof(VisualElement).GetProperties(BF); }
if (MODE == 1) { dT.GetMethod("CancelDrag", BF).Invoke(drag, null); sv.Repaint(); return "cancelled"; }
var from = dEl.worldBound.center;
var down = new Event { type = EventType.MouseDown, mousePosition = from, button = 0, clickCount = 1 };
using (var e = MouseDownEvent.GetPooled(down)) { typeof(EventBase).GetProperty("target", BF).SetValue(e, dEl); dT.GetMethod("OnMouseDown", BF).Invoke(drag, new object[]{ e }); }
var mv = new Event { type = EventType.MouseDrag, mousePosition = TO, button = 0 };
using (var e = MouseMoveEvent.GetPooled(mv)) { typeof(EventBase).GetProperty("target", BF).SetValue(e, dEl); dT.GetMethod("OnMouseMove", BF).Invoke(drag, new object[]{ e }); }
sv.Repaint();
return "dragger=" + (drag != null) + " from=" + from + " to=" + TO + " active=" + dT.GetField("m_Active", BF).GetValue(drag);
