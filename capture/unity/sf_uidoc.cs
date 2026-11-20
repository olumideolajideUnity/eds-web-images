// Search field / Toggle / Toolbar: a saved capture scene (Assets/EFDS_Screenshots/UIToolkitDoc.unity) with a UIDocument using the leftover
// Assets/UI Toolkit/PanelSettings.asset and Assets/New UXML.uxml (one Button), selected, so the UI Viewport and the UI element Inspector
// (with its condensed search) have something to show. Saved immediately so no scene/UI save prompt can block the Editor.
using UnityEditor; using UnityEditor.SceneManagement; using UnityEngine; using UnityEngine.UIElements; using System.Linq;
var path = "Assets/EFDS_Screenshots/UIToolkitDoc.unity";
var scene = System.IO.File.Exists(path) ? EditorSceneManager.OpenScene(path) : EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
var go = GameObject.Find("UIDocument");
if (go == null) { go = new GameObject("UIDocument"); var d = go.AddComponent<UIDocument>();
  d.panelSettings = AssetDatabase.LoadAssetAtPath<PanelSettings>("Assets/UI Toolkit/PanelSettings.asset");
  d.visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/New UXML.uxml"); }
EditorSceneManager.SaveScene(go.scene, path);
Selection.activeGameObject = go;
var kids = string.Join(",", go.GetComponentsInChildren<Transform>(true).Select(t => t.name));
return "scene=" + go.scene.path + " comps=" + string.Join(",", go.GetComponents<Component>().Select(c => c.GetType().Name)) + " children=" + kids;
