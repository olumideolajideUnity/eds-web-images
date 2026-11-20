using UnityEditor; using UnityEditor.SceneManagement; using UnityEngine; using System.Linq;
if (EditorSceneManager.GetActiveScene().path != "Assets/Atrio/AtrioScenes/Atrio.unity") EditorSceneManager.OpenScene("Assets/Atrio/AtrioScenes/Atrio.unity");
var cams = Object.FindObjectsByType<Camera>(FindObjectsSortMode.None);
var cam = cams.FirstOrDefault(c => c.CompareTag("MainCamera")) ?? cams.FirstOrDefault();
Selection.activeGameObject = cam ? cam.gameObject : null;
var roots = EditorSceneManager.GetActiveScene().GetRootGameObjects().Select(g => g.name);
return "sel=" + Selection.activeGameObject + " roots=" + string.Join(",", roots.Take(20));
