using UnityEditor; using UnityEngine; using System.Linq;
int n = 0; foreach (var w in Resources.FindObjectsOfTypeAll<EditorWindow>().Where(x => x.GetType().Name == "PackageManagerWindow").ToList()) { w.Close(); n++; } return "closed " + n;
