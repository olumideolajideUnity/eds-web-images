// Windowing backdrop scene: empty scene + Directional Light (sun gizmo), saved as Assets/EFDS_Screenshots/Windowing.unity.
using UnityEditor; using UnityEditor.SceneManagement; using UnityEngine;
string SC = "Assets/EFDS_Screenshots/Windowing.unity";
if (AssetDatabase.LoadAssetAtPath<SceneAsset>(SC) == null) {
  var s = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
  var l = new GameObject("Directional Light").AddComponent<Light>(); l.type = LightType.Directional; l.transform.rotation = Quaternion.Euler(50, -30, 0); l.transform.position = new Vector3(2.2f, 1.2f, 0);
  EditorSceneManager.SaveScene(s, SC);
} else if (EditorSceneManager.GetActiveScene().path != SC) EditorSceneManager.OpenScene(SC);
Selection.activeObject = null;
return "ok";
