using UnityEditor; using UnityEngine; using System.Linq;
var path = "Assets/ExampleAssets/Shaders/Paint1G_WAnim_Shader.shadergraph";
var asset = AssetDatabase.LoadMainAssetAtPath(path);
AssetDatabase.OpenAsset(asset);
var wins = Resources.FindObjectsOfTypeAll<EditorWindow>().Where(w => w.GetType().Name.Contains("Graph")).ToList();
return string.Join("; ", wins.Select(w => w.GetType().FullName + " '" + w.titleContent.text + "' docked=" + w.docked + " " + w.position));
