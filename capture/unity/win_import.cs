// Real "Import Unity Package" window in its nothing-to-import state: export a package of an asset already in the project,
// then import it interactively (Unity sees every item unchanged).
using UnityEditor; using UnityEngine; using System.Linq;
string PKG = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "EFDS Sample.unitypackage");
AssetDatabase.ExportPackage("Assets/EFDS_Screenshots/Cube.prefab", PKG, ExportPackageOptions.Default);
AssetDatabase.ImportPackage(PKG, true);
return "importing " + PKG;
