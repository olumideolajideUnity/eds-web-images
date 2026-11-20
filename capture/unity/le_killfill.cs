using UnityEditor; using UnityEngine; using System.Linq;
var n = 0;
foreach (var l in Resources.FindObjectsOfTypeAll<Light>().Where(l => l.name == "EFDS_TempFill" && !EditorUtility.IsPersistent(l))) { Object.DestroyImmediate(l.gameObject); n++; }
RenderSettings.sun = null; SceneView.RepaintAll();
return "removed " + n;
