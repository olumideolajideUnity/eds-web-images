// Ensures exactly one floating Inspector titled "EFDSCapture" at the given size (points), for `capture editor_window --window EFDSCapture`.
// Edit W/H below per shot.
using UnityEditor; using UnityEngine; using System.Linq;
float W = 416, H = 640;
var inspType = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
var insp = Resources.FindObjectsOfTypeAll(inspType).Cast<EditorWindow>().FirstOrDefault(w => !w.docked);
if (insp == null) { insp = (EditorWindow)ScriptableObject.CreateInstance(inspType); insp.Show(); }
insp.position = new Rect(800, 120, W, H);
insp.Repaint();
return "insp=" + insp.position;
