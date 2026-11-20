using UnityEditor; using UnityEngine; using System.Linq;
var toolType = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("UnityEditor.BoxPrimitiveColliderTool")).First(t => t != null);
UnityEditor.EditorTools.ToolManager.SetActiveTool(toolType);
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
sv.LookAtDirect(new Vector3(-0.3f, 0.1f, -0.25f), Quaternion.Euler(22f, -32f, 0f), 1.55f);
sv.Repaint();
return "tool=" + UnityEditor.EditorTools.ToolManager.activeToolType?.Name;
