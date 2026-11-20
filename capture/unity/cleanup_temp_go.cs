// Remove temporary DontSave GameObjects created for captures (Canvas, Main Camera, EFDS_*).
using UnityEditor; using UnityEngine; using System.Linq;
int n = 0;
foreach (var g in Resources.FindObjectsOfTypeAll<GameObject>().Where(x => !EditorUtility.IsPersistent(x) && x.hideFlags == HideFlags.DontSave && x.transform.parent == null && (x.name == "Canvas" || x.name == "Main Camera" || x.name == "Point Light" || x.name.StartsWith("EFDS_"))).ToList()) { Object.DestroyImmediate(g); n++; }
return "removed " + n;
