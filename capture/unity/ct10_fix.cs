using UnityEditor; using UnityEditor.SceneManagement; using UnityEngine; using System.Linq;
string path = "Assets/EFDS_Screenshots/EFDS_Blue.mat";
var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
if (mat == null) {
  var sh = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
  mat = new Material(sh); var c = new Color(0.22f, 0.5f, 0.95f);
  if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", c); if (mat.HasProperty("_Color")) mat.SetColor("_Color", c);
  AssetDatabase.CreateAsset(mat, path);
}
var box = GameObject.Find("Box"); box.GetComponent<MeshRenderer>().sharedMaterial = mat;
EditorSceneManager.MarkSceneDirty(box.scene); EditorSceneManager.SaveScene(box.scene);
Selection.activeGameObject = box;
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
sv.LookAtDirect(new Vector3(-2.6f, 1.0f, -0.6f), Quaternion.Euler(26f, -48f, 0f), 5.2f);
var co = sv.overlayCanvas.overlays.First(o => o.displayName == "Cameras"); co.floatingPosition = new Vector2(1090, 300);
sv.Repaint();
return "ok";
