using UnityEditor; using UnityEditor.SceneManagement; using UnityEngine;
if (EditorSceneManager.GetActiveScene().path != "Assets/EFDS_Screenshots/ProBuilderArch.unity") EditorSceneManager.OpenScene("Assets/EFDS_Screenshots/ProBuilderArch.unity");
Selection.activeGameObject = null;
return "scene=" + EditorSceneManager.GetActiveScene().name;
