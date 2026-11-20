using UnityEditor; using UnityEngine; using UnityEngine.ProBuilder; using UnityEditor.ProBuilder; using UnityEditor.EditorTools; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var a = System.AppDomain.CurrentDomain.GetAssemblies().First(x => x.GetName().Name == "Unity.ProBuilder.Editor");
Selection.activeGameObject = GameObject.Find("Box");
ToolManager.SetActiveContext(a.GetType("UnityEditor.ProBuilder.PositionToolContext"));
ProBuilderEditor.selectMode = SelectMode.Face;
var pb = GameObject.Find("Box").GetComponent<ProBuilderMesh>();
// the bevel face is the one whose normal points up-and-out (not axis aligned)
var bevel = pb.faces.First(f => { var n = UnityEngine.ProBuilder.Math.Normal(pb, f); return n.y > 0.2f && n.y < 0.95f; });
pb.SetSelectedFaces(new[]{ bevel });
ProBuilderEditor.Refresh();
Tools.current = Tool.Move;
var act = System.Activator.CreateInstance(a.GetType("UnityEditor.ProBuilder.Actions.BevelEdges"));
act.GetType().GetMethod("PerformAltAction", BF).Invoke(act, null);
foreach (var w in Resources.FindObjectsOfTypeAll<EditorWindow>().Where(w => w.GetType().Name == "MenuOption").ToList()) w.Close();
SceneView.RepaintAll();
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
return "ctx=" + ToolManager.activeContextType?.Name + " tool=" + ToolManager.activeToolType?.Name + " faces=" + pb.selectedFaceCount + " overlays=" + string.Join(",", sv.overlayCanvas.overlays.Where(o => o.displayed).Select(o => o.displayName));
