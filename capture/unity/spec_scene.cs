using UnityEditor; using UnityEditor.SceneManagement; using UnityEngine; using System.Linq;
var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
string[] types = { "ComponentHeader", "HeightNoiseLayer", "MeshRenderer", "RectTransform", "MaterialLayer" };
var mat = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Material.mat");
foreach (var t in types) {
  var go = new GameObject(t);
  var type = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS." + t)).First(x => x != null);
  var c = go.AddComponent(type);
  if (c is EFDS.MeshRenderer m) { m.elements.Add(mat); m.elements.Add(mat); }
  UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(go.transform, false);
}
EditorSceneManager.SaveScene(scene, "Assets/EFDS_Screenshots/Specimens.unity");
return "ok " + string.Join(",", scene.GetRootGameObjects().Select(g => g.name));
