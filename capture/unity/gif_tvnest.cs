// tv-nest.gif setup (user screen recording): a temporary Lit material Assets/NewMaterial.mat in the Assets root (the asset to drag),
// then the floating two-column Project browser (run unity/gif_proj.cs first: W x H, tree collapsed, right pane Assets) with Assets and
// ExampleAssets expanded (via a ping of ExampleAssets/Materials), nothing selected. Drop target: ExampleAssets > Materials.
// Clean up afterwards: delete NewMaterial.mat wherever it ended up (MODE CLEAN).
using UnityEditor; using UnityEngine; using System.Linq;
string MODE = "SETUP";
if (MODE == "CLEAN") { var g = AssetDatabase.FindAssets("NewMaterial t:Material").Select(AssetDatabase.GUIDToAssetPath).Where(p => p.EndsWith("/NewMaterial.mat")).ToList(); foreach (var p in g) AssetDatabase.DeleteAsset(p); return "deleted " + string.Join(", ", g); }
if (!System.IO.File.Exists("Assets/NewMaterial.mat")) {
  var sh = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
  AssetDatabase.CreateAsset(new Material(sh), "Assets/NewMaterial.mat"); AssetDatabase.SaveAssets(); }
EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<Object>("Assets/ExampleAssets/Materials"));
Selection.activeObject = null;
return "ok";
