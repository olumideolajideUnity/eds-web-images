using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
float PITCH = 22, YAW = -20, SIZE = 6f; Vector3 PIV = new Vector3(0, 0.5f, 0);
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
typeof(SceneView).GetField("s_LastActiveSceneView", BindingFlags.Static|BindingFlags.NonPublic).SetValue(null, sv);
sv.sceneViewState.showSkybox = false; sv.showGrid = true; sv.drawGizmos = true;
sv.LookAtDirect(PIV, Quaternion.Euler(PITCH, YAW, 0), SIZE); sv.Repaint(); return "ok";
