using UnityEditor; using UnityEditor.SceneManagement; using UnityEngine; using System.Linq;
var path = "Assets/ExampleAssets/Prefabs/PaintSupplies.prefab";
if (AssetDatabase.LoadAssetAtPath<GameObject>(path) == null)
    AssetDatabase.RenameAsset("Assets/ExampleAssets/Prefabs/Paint Supplies.prefab", "PaintSupplies");
var p = AssetDatabase.LoadAssetAtPath<GameObject>(path);
return p.name + " children: " + string.Join(", ", p.GetComponentsInChildren<Transform>(true).Skip(1).Select(t => t.name));
