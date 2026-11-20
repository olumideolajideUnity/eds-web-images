// List Unity Registry packages that are deprecated or have deprecated versions (they show a help box in Package Manager details).
using UnityEditor; using UnityEditor.PackageManager; using UnityEngine; using System.Linq;
var req = Client.SearchAll(true);
EditorApplication.CallbackFunction cb = null;
cb = () => { if (!req.IsCompleted) return; EditorApplication.update -= cb;
  var s = req.Status == StatusCode.Success ? string.Join("\n", req.Result.Where(p => p.isDeprecated || (p.versions.deprecated != null && p.versions.deprecated.Length > 0)).Select(p => p.name + " dep=" + p.isDeprecated + " msg=" + p.deprecationMessage + " depVers=" + string.Join(",", p.versions.deprecated ?? new string[0]))) : "error " + req.Error?.message;
  System.IO.File.WriteAllText("C:/tmp/pm_dep.txt", s); };
EditorApplication.update += cb; return "searching";
