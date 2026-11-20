using UnityEditor; using UnityEngine; using UnityEngine.ProBuilder; using UnityEditor.ProBuilder; using UnityEditor.EditorTools; using System.Linq; using System.Reflection;
var a = System.AppDomain.CurrentDomain.GetAssemblies().First(x => x.GetName().Name == "Unity.ProBuilder.Editor");
Selection.activeGameObject = GameObject.Find("Box");
ToolManager.SetActiveContext(a.GetType("UnityEditor.ProBuilder.PositionToolContext"));
ProBuilderEditor.selectMode = SelectMode.Edge;
var pb = GameObject.Find("Box").GetComponent<ProBuilderMesh>();
var bevel = pb.faces.First(f => { var n = UnityEngine.ProBuilder.Math.Normal(pb, f); return n.y > 0.2f && n.y < 0.95f; });
// the bevel's upper long edge
var e = bevel.edges.OrderByDescending(x => pb.positions[x.a].y + pb.positions[x.b].y).ThenByDescending(x => Vector3.Distance(pb.positions[x.a], pb.positions[x.b])).First();
pb.SetSelectedEdges(new[]{ e });
ProBuilderEditor.Refresh(); Tools.current = Tool.Move; SceneView.RepaintAll();
return "edges=" + pb.selectedEdgeCount;
