using UnityEditor; using UnityEditor.SceneManagement; using UnityEngine; using System.Linq;
EditorSceneManager.OpenScene("Assets/EFDS_Screenshots/GOAuthoring.unity");
var asset = AssetDatabase.LoadMainAssetAtPath("Assets/ExampleAssets/Shaders/Paint1G_WAnim_Shader.shadergraph");
Selection.activeObject = asset;
// Focused Inspector (Properties window) for the same asset
foreach (var old in Resources.FindObjectsOfTypeAll<EditorWindow>().Where(w => w.GetType().Name == "PropertyEditor").ToList()) old.Close();
EditorUtility.OpenPropertyEditor(asset);
var pe = Resources.FindObjectsOfTypeAll<EditorWindow>().First(w => w.GetType().Name == "PropertyEditor");
pe.position = new Rect(420, 160, 403, 460);
// Scene view back to its own tab with the cube in view
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
sv.Focus();
return "pe='" + pe.titleContent.text + "' " + pe.position + " sv=" + sv.position;
