// MODE 0: hide trigger-volume objects in the Scene view (SceneVisibilityManager, view-only); MODE 1: show them again.
using UnityEditor; using UnityEngine; using System.Linq;
int MODE = 0;
var gos = Object.FindObjectsByType<Transform>(FindObjectsSortMode.None).Where(t => t.name.Contains("TriggerVolume") || t.name.StartsWith("Reflies") || t.name.Contains("FloorEast") || t.name.Contains("FloorWest")
  || t.GetComponents<MonoBehaviour>().Any(m => m != null && (m.GetType().Name.Contains("Trigger") || m.GetType().Name.Contains("Volume")))).Select(t => t.gameObject).ToArray();
if (MODE == 0) SceneVisibilityManager.instance.Hide(gos, false); else SceneVisibilityManager.instance.Show(gos, false);
SceneView.RepaintAll(); return (MODE == 0 ? "hidden " : "shown ") + gos.Length;
