using UnityEditor; using UnityEngine; using System.Linq;
var t = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("UnityEditor.ProBuilder.EditShapeTool")).First(x => x != null);
UnityEditor.EditorTools.ToolManager.SetActiveTool(t); return "tool=" + UnityEditor.EditorTools.ToolManager.activeToolType?.Name;
