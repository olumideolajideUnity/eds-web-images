// Drop keyboard focus (blue focused label) by focusing the floating Scene view; optionally hide the tool-context toolbar.
using UnityEditor; using UnityEditor.Overlays; using UnityEngine; using System.Linq;
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First();
if (sv.TryGetOverlay("unity-tool-contexts-toolbar", out Overlay o)) o.displayed = false;
GUIUtility.keyboardControl = 0; sv.Focus();
foreach (var w in Resources.FindObjectsOfTypeAll<EditorWindow>()) w.Repaint();
return "ok";
