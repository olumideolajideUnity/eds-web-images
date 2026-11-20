using UnityEditor; using UnityEngine; using System.Linq;
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
var inspType = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
var insp = Resources.FindObjectsOfTypeAll(inspType).Cast<EditorWindow>().First(w => !w.docked);
// 1.2x smaller than the composed size: 838x525 -> 698x437, 500 -> 416
sv.position = new Rect(60, 120, 698, 437);
insp.position = new Rect(800, 120, 416, 640);
sv.LookAtDirect(new Vector3(8.789518f, 0.829852f, -4.06888f) + new Vector3(0.12f, 0.05f, 0f), Quaternion.Euler(17f, 22f, 0f), 1.05f);
sv.Repaint(); insp.Repaint();
return sv.position + " " + insp.position;
