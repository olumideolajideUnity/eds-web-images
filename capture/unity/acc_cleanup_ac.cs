using UnityEditor; using UnityEngine; using System.Linq;
int n = 0; foreach (var g in Resources.FindObjectsOfTypeAll<GameObject>().Where(x => x.name == "EFDS_AddComponent").ToList()) { Object.DestroyImmediate(g); n++; }
var hostT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSPopupHost")).First(x => x != null);
foreach (var o in Resources.FindObjectsOfTypeAll(hostT).Cast<EditorWindow>().ToList()) o.Close();
return "removed " + n;
