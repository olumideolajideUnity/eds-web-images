// Select a specimen object and size the floating Inspector. Edit NAME, W, H.
using UnityEditor; using UnityEngine; using System.Linq;
string NAME = "HeightNoiseLayer"; float W = 360, H = 420;
var go = GameObject.Find(NAME); Selection.activeGameObject = go;
UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(go.transform, false);
var it = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
var insp = Resources.FindObjectsOfTypeAll(it).Cast<EditorWindow>().First(w => !w.docked);
insp.position = new Rect(800, 120, W, H);
EFDS.Rects.All.Clear(); insp.Repaint();
return "ok";
