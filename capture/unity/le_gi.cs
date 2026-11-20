using UnityEditor; using UnityEngine; using System.Linq;
var s = "";
foreach (var g in AssetDatabase.FindAssets("t:LightingSettings")) { var p = AssetDatabase.GUIDToAssetPath(g); var l = AssetDatabase.LoadAssetAtPath<LightingSettings>(p); s += p + " rt=" + l.realtimeGI + " baked=" + l.bakedGI + "\n"; }
foreach (var g in AssetDatabase.FindAssets("t:Scene", new[]{ "Assets" })) s += "scene " + AssetDatabase.GUIDToAssetPath(g) + "\n";
System.IO.File.WriteAllText("C:/tmp/gi.txt", s); return "ok";
