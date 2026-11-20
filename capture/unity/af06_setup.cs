using UnityEditor; using UnityEditor.SceneManagement; using UnityEngine; using System.Linq;
UnityEditor.SceneManagement.StageUtility.GoToMainStage();
var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
var cube = GameObject.CreatePrimitive(PrimitiveType.Cube); cube.name = "Cube";
var col = cube.GetComponent<BoxCollider>();
col.center = new Vector3(-0.6f, 0f, -0.45f); col.size = new Vector3(2.2f, 1f, 1.9f);
// keep the directional light out of frame
var light = GameObject.Find("Directional Light"); if (light) light.transform.position = new Vector3(0, 30, 30);
EditorSceneManager.SaveScene(scene, "Assets/EFDS_Screenshots/Toggling.unity");
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
sv.position = new Rect(300, 120, 726, 407);
sv.drawGizmos = true;
sv.Focus();
Selection.activeGameObject = cube;
var toolType = typeof(Collider).Assembly.GetType("UnityEditor.BoxPrimitiveColliderTool")
            ?? System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("UnityEditor.BoxPrimitiveColliderTool")).First(t => t != null);
UnityEditor.EditorTools.ToolManager.SetActiveTool(toolType);
sv.LookAtDirect(new Vector3(-0.3f, 0.1f, -0.25f), Quaternion.Euler(22f, -32f, 0f), 1.55f);
sv.Repaint();
return "tool=" + UnityEditor.EditorTools.ToolManager.activeToolType?.Name;
