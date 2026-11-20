// Inheritance overview: Cube prefab + instance (position 50, Scale overridden to 2) in Assets/EFDS_Screenshots/Inheritance.unity.
using UnityEditor; using UnityEditor.SceneManagement; using UnityEngine; using System.Linq;
string PF = "Assets/EFDS_Screenshots/Cube.prefab", SC = "Assets/EFDS_Screenshots/Inheritance.unity";
var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
if (AssetDatabase.LoadAssetAtPath<GameObject>(PF) == null) {
  var tmp = GameObject.CreatePrimitive(PrimitiveType.Cube); tmp.name = "Cube";
  PrefabUtility.SaveAsPrefabAsset(tmp, PF); Object.DestroyImmediate(tmp);
}
var inst = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(PF));
inst.transform.position = new Vector3(50, 0, 0);
inst.transform.localScale = new Vector3(2, 2, 2);
EditorSceneManager.SaveScene(scene, SC);
Selection.activeGameObject = inst;
var mods = PrefabUtility.GetPropertyModifications(inst).Select(m => m.propertyPath + "=" + m.value);
return "mods=" + string.Join(",", mods);
