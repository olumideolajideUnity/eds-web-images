using UnityEditor; using UnityEngine; using System.Linq;
foreach (var sv in Resources.FindObjectsOfTypeAll<SceneView>()) { var cs = sv.cameraSettings; cs.fieldOfView = 60; sv.cameraSettings = cs; }
return "fov reset";
