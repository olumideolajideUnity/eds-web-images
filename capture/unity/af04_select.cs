using UnityEditor; using UnityEngine; using UnityEngine.Splines; using UnityEditor.Splines; using UnityEditor.EditorTools; using System.Linq;
var sc = GameObject.Find("Spline").GetComponent<SplineContainer>();
Selection.activeGameObject = sc.gameObject;
ToolManager.SetActiveContext<SplineToolContext>();
SplineSelection.Clear();
SplineSelection.Add(new SelectableKnot(new SplineInfo(sc, 0), 4));
foreach (var v in Resources.FindObjectsOfTypeAll<SceneView>()) v.Repaint();
return "context=" + ToolManager.activeContextType?.Name + " selectedKnots=" + SplineSelection.Count;
