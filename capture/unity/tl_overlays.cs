// Toolbar page: on the floating Scene view, show only the view-option toolbars like the original Scene toolbar
// (hide Tool Settings / Grid & Snap, show the search toolbar). SHOW = false restores them.
using UnityEditor; using UnityEditor.Overlays; using UnityEngine; using System.Linq;
bool SHOW = true;
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
foreach (var (id, on) in new[]{ ("Tool Settings", !SHOW), ("unity-grid-and-snap-toolbar", !SHOW), ("unity-search-toolbar", SHOW) })
  if (sv.TryGetOverlay(id, out Overlay o)) o.displayed = on;
sv.Repaint(); return "ok";
