using UnityEditor; using UnityEditor.SceneManagement; using UnityEngine; using System.Linq;
var path = "Assets/ExampleAssets/Prefabs/WorkshopSet.prefab";
if (AssetDatabase.LoadAssetAtPath<GameObject>(path) == null)
    AssetDatabase.RenameAsset("Assets/ExampleAssets/Prefabs/Workshop Set.prefab", "WorkshopSet");
var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
var go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, scene);
go.transform.position = Vector3.zero; go.transform.rotation = Quaternion.identity;
EditorSceneManager.SaveScene(scene, "Assets/EFDS_Screenshots/PrefabInspector.unity");
Selection.activeGameObject = go;
return go.name + " comps=" + string.Join(",", go.GetComponents<Component>().Select(c => c.GetType().Name)) + " children=" + go.transform.childCount;
