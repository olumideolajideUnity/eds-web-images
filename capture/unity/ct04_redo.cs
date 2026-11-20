using UnityEditor; using UnityEditor.SceneManagement; using UnityEngine; using UnityEditor.EditorTools; using System.Linq;
if (EditorSceneManager.GetActiveScene().path != "Assets/EFDS_Screenshots/ProBuilderStairs.unity") EditorSceneManager.OpenScene("Assets/EFDS_Screenshots/ProBuilderStairs.unity");
Selection.activeGameObject = GameObject.Find("Stairs");
return "sel=" + Selection.activeGameObject;
