// Float the Tools overlay horizontally at TX,TY and a second overlay (by display-name match) at OX,OY. Hide orientation/search/view options.
using UnityEditor; using UnityEditor.Overlays; using UnityEngine; using System.Linq; using System.Reflection;
float TX = 12, TY = 34, OX = 12, OY = 78; string SECOND = "Shape Settings"; float WIN_H = 762;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
var parent = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(sv);
var container = parent.GetType().GetProperty("window", BF).GetValue(parent);
container.GetType().GetProperty("position", BF).SetValue(container, new Rect(10, 60, 1500, WIN_H));
foreach (var o in sv.overlayCanvas.overlays)
  if (new[]{ "Orientation", "Search", "View Options", "Grid and Snap", "Cameras" }.Contains(o.displayName)) o.displayed = false;
var tools = sv.overlayCanvas.overlays.First(o => o.displayName == "Tools");
tools.displayed = true; tools.Undock();
typeof(Overlay).GetProperty("layout", BF).GetSetMethod(true)?.Invoke(tools, new object[]{ Layout.HorizontalToolbar });
tools.floatingPosition = new Vector2(TX, TY);
string res = "tools=" + tools.floatingPosition;
if (SECOND.Length > 0) {
  var o2 = sv.overlayCanvas.overlays.First(o => o.displayName.Contains(SECOND));
  o2.displayed = true; o2.Undock(); o2.collapsed = false; o2.floatingPosition = new Vector2(OX, OY);
  res += " second=" + o2.displayName + "@" + o2.floatingPosition;
}
sv.Focus(); sv.Repaint();
return res + " sv=" + sv.position;
