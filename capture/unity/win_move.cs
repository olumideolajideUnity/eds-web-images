using UnityEditor; using UnityEditor.EditorTools; using UnityEngine;
ToolManager.SetActiveContext<GameObjectToolContext>(); Tools.current = Tool.Move; SceneView.RepaintAll(); return "tool=" + ToolManager.activeToolType?.Name;
