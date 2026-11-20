using UnityEditor; using UnityEngine; using System.Linq;
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
foreach (var o in sv.overlayCanvas.overlays) if (o.displayName == "Cameras") o.displayed = false;
Selection.activeGameObject = GameObject.Find("Low Block");
Tools.current = Tool.View; sv.drawGizmos = false;
var rot = Quaternion.Euler(24f, 62f, 0f);
sv.LookAtDirect(new Vector3(-0.2f, 1.4f, -0.8f) + rot * Vector3.right * 1.6f, rot, 4.4f);
sv.Repaint();
return "ok";
