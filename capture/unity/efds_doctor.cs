// EFDS doctor: report what the open Editor and project provide, as JSON (read-only).
using UnityEditor; using UnityEngine; using System.Linq;
var pk = UnityEditor.PackageManager.PackageInfo.GetAllRegisteredPackages().Select(p => "\"" + p.name + "\":\"" + p.version + "\"");
string[] dirs = { "Assets/EFDS_Screenshots", "Assets/Atrio", "Assets/ExampleAssets", "Assets/Scenes/SampleScene.unity" };
var found = dirs.Where(d => AssetDatabase.IsValidFolder(d) || System.IO.File.Exists(d)).Select(d => "\"" + d + "\"");
bool host = System.AppDomain.CurrentDomain.GetAssemblies().Any(a => a.GetType("EFDS.EFDSPopupHost") != null);
bool timelineDemo = AssetDatabase.FindAssets("GameplaySequence t:Scene").Length > 0;
return "{\"unity\":\"" + Application.unityVersion + "\",\"skin\":\"" + (EditorGUIUtility.isProSkin ? "dark" : "light") +
  "\",\"pixelsPerPoint\":" + EditorGUIUtility.pixelsPerPoint.ToString(System.Globalization.CultureInfo.InvariantCulture) +
  ",\"fixtures_compiled\":" + (host ? "true" : "false") + ",\"timeline_demo\":" + (timelineDemo ? "true" : "false") +
  ",\"content\":[" + string.Join(",", found) + "],\"packages\":{" + string.Join(",", pk) + "}}";
