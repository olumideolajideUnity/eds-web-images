using UnityEditor; using UnityEditor.SceneManagement; using UnityEngine; using System.Linq;
var scene = EditorSceneManager.OpenScene("Assets/Atrio/AtrioScenes/Atrio.unity", OpenSceneMode.Single);
var all = scene.GetRootGameObjects().SelectMany(g => g.GetComponentsInChildren<Transform>(true)).Select(t => t.gameObject).ToList();
var plants = all.Where(g => g.name.Contains("PottedPlant") || g.name.Contains("Plant")).Select(g => g.name + "@" + g.transform.position.ToString("F1")).Distinct().Take(15);
var tables = all.Where(g => g.name.Contains("Table") || g.name.Contains("Chair") || g.name.Contains("Stool") || g.name.Contains("Sofa") || g.name.Contains("Armchair")).Select(g => g.name + "@" + g.transform.position.ToString("F1")).Distinct().Take(20);
return "objects=" + all.Count + "\nPLANTS: " + string.Join(" | ", plants) + "\nFURNITURE: " + string.Join(" | ", tables);
