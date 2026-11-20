using UnityEditor; using UnityEditor.SceneManagement; using UnityEngine;
if (EditorSceneManager.GetActiveScene().path != "Assets/EFDS_Screenshots/ProBuilderLevel.unity") EditorSceneManager.OpenScene("Assets/EFDS_Screenshots/ProBuilderLevel.unity");
return "scene=" + EditorSceneManager.GetActiveScene().name + " cam=" + GameObject.Find("Camera");
