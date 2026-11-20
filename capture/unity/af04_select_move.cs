using UnityEditor; using UnityEngine; using UnityEngine.Splines; using UnityEditor.Splines; using UnityEditor.EditorTools; using System.Linq;
var sc = GameObject.Find("Spline").GetComponent<SplineContainer>();
Selection.activeGameObject = sc.gameObject;
if (ToolManager.activeContextType != typeof(SplineToolContext)) ToolManager.SetActiveContext<SplineToolContext>();
Tools.current = Tool.Move;
SplineSelection.Clear();
SplineSelection.Add(new SelectableKnot(new SplineInfo(sc, 0), 4));
SceneView.RepaintAll();
return "tool=" + ToolManager.activeToolType?.Name + " knots=" + SplineSelection.Count;
