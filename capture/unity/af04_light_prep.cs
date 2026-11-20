// af04 light: on the floating Scene view show the spline Element Inspector + tool-context toolbar, and make it the last active
// Scene view so the CLI scene_view capture targets it (not the main window's view).
using UnityEditor; using UnityEditor.Overlays; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Static|BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
foreach (var id in new[]{ "unity-spline-inspector", "unity-tool-contexts-toolbar" }) if (sv.TryGetOverlay(id, out Overlay o)) o.displayed = true;
var f = typeof(SceneView).GetField("s_LastActiveSceneView", BF); f?.SetValue(null, sv);
sv.Focus(); sv.Repaint();
return "last=" + SceneView.lastActiveSceneView.position + " field=" + (f != null);
