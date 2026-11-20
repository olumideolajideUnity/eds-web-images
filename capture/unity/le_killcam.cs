// Cleanup: destroy the temporary DontSave "Main Camera" left by cp_insp.cs.
using UnityEditor; using UnityEngine; using System.Linq;
var n = 0;
foreach (var g in Resources.FindObjectsOfTypeAll<GameObject>().Where(g => !EditorUtility.IsPersistent(g) && g.name == "Main Camera" && g.hideFlags == HideFlags.DontSave).ToList()) { Object.DestroyImmediate(g); n++; }
SceneView.RepaintAll(); return "removed " + n;
