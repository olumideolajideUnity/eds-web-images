using UnityEditor; using UnityEngine; using System.Linq;
var all = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(g => g.GetComponentsInChildren<Transform>(true)).Where(t => t.gameObject.activeInHierarchy).ToList();
var plants = all.Where(t => t.name.StartsWith("PottedPlant")).ToList();
var furn = all.Where(t => t.name.Contains("Table") || t.name.Contains("Chair") || t.name.Contains("Stool") || t.name.Contains("Sofa") || t.name.Contains("Bench") || t.name.Contains("Desk")).ToList();
var res = plants.Select(p => {
  var near = furn.Where(f => Vector3.Distance(f.position, p.position) < 1.8f).Select(f => f.name + "(" + Vector3.Distance(f.position, p.position).ToString("F1") + ")").Distinct().Take(4).ToList();
  return (p, near);
}).Where(x => x.near.Count > 0).Take(12).Select(x => x.p.name + "@" + x.p.position.ToString("F1") + " near " + string.Join(",", x.near));
return string.Join("\n", res) + "\nfurniture kinds: " + string.Join(",", furn.Select(f => f.name.Split(' ')[0]).Distinct().Take(25));
