using UnityEditor; using UnityEngine; using System.Linq; using System.Collections.Generic;
var all = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(g => g.GetComponentsInChildren<Transform>(true)).Where(t => t.gameObject.activeInHierarchy).ToList();
var plants = all.Where(t => t.name.StartsWith("PottedPlant") && t.GetComponentInChildren<Renderer>() != null).ToList();
var furn = all.Where(t => t.name.Contains("Table") || t.name.Contains("Chair") || t.name.Contains("Stool") || t.name.Contains("Bench")).ToList();
var outp = new List<string>();
foreach (var p in plants) {
  var b = new Bounds(p.position, Vector3.zero); foreach (var r in p.GetComponentsInChildren<Renderer>()) b.Encapsulate(r.bounds);
  int nearF = furn.Count(f => Vector3.Distance(f.position, p.position) < 2.5f);
  var clear = new List<int>();
  for (int yaw = 0; yaw < 360; yaw += 45) {
    var rot = Quaternion.Euler(15f, yaw, 0f);
    var cam = b.center - rot * Vector3.forward * 3.0f;
    // clear line of sight from camera to plant (ignore the plant itself) and open sky above the plant
    var hits = Physics.RaycastAll(cam, (b.center - cam).normalized, Vector3.Distance(cam, b.center) - b.extents.magnitude);
    bool sky = !Physics.Raycast(b.center + Vector3.up * (b.extents.y + 0.1f), Vector3.up, 30f);
    if (hits.Length == 0 && sky) clear.Add(yaw);
  }
  if (clear.Count > 0) outp.Add(p.name + "@" + p.position.ToString("F1") + " furniture=" + nearF + " clearYaws=" + string.Join(",", clear));
}
return outp.Count + " candidates\n" + string.Join("\n", outp.OrderByDescending(s => s.Contains("furniture=0") ? 0 : 1).Take(15));
