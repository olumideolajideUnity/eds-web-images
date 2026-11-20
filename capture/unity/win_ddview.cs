// Windowing dd-800 (light pass): match the dark capture's floating Scene view, where only Tools (left), Draw Modes and View Options
// show: hide Grid and Snap, Tool Settings, Orientation and the Overlay Menu bar in this view.
using UnityEditor; using UnityEngine; using System.Linq;
string[] HIDE = { "Grid and Snap", "Tool Settings", "Orientation", "Overlay Menu" };
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
foreach (var o in sv.overlayCanvas.overlays) if (HIDE.Contains(o.displayName)) o.displayed = false;
sv.Repaint(); return string.Join(",", sv.overlayCanvas.overlays.Where(o => o.displayed).Select(o => o.displayName));
