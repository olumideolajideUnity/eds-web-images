using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq;
Vector3 PIV = new Vector3(1.46f, 0.3f, 0.46f); float PITCH = 30, YAW = -62, SIZE = 2.2f, FOV = 60; Vector2 OV = new Vector2(483, 260 + 24);
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
var cs = sv.cameraSettings; cs.fieldOfView = FOV; sv.cameraSettings = cs;
sv.LookAtDirect(PIV, Quaternion.Euler(PITCH, YAW, 0), SIZE);
var tools = sv.overlayCanvas.overlays.First(o => o.displayName == "Tools"); tools.rootVisualElement.style.display = DisplayStyle.None;
foreach (var o in sv.overlayCanvas.overlays) if (o.displayName == "Tool Settings") o.displayed = false;
var ov = sv.overlayCanvas.overlays.FirstOrDefault(o => o.displayName == "Bevel Edges"); if (ov != null) { ov.Undock(); ov.floatingPosition = OV; }
sv.Repaint(); return "ov=" + (ov != null);
