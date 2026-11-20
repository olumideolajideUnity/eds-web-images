// Render Atrio's Cat prefab (Cat_Black / Cat_Tiger materials) to 128px transparent PNG sprites in a preview scene, six turns each,
// into Assets/EFDS_Screenshots/Sprites, imported as Sprites. Nothing in the open scene or Atrio is modified.
using UnityEditor; using UnityEditor.SceneManagement; using UnityEngine; using System.Linq;
string DIR = "Assets/EFDS_Screenshots/Sprites"; int S = 128;
if (!AssetDatabase.IsValidFolder(DIR)) AssetDatabase.CreateFolder("Assets/EFDS_Screenshots", "Sprites");
var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Atrio/Character/Cat/Prefabs/Cat.prefab");
var scene = EditorSceneManager.NewPreviewScene();
var cat = (GameObject)PrefabUtility.InstantiatePrefab(prefab, scene);
var lgo = new GameObject("L"); var l = lgo.AddComponent<Light>(); l.type = LightType.Directional; l.intensity = 1.6f; lgo.transform.rotation = Quaternion.Euler(35, 160, 0); RenderSettings.ambientLight = new Color(0.5f, 0.5f, 0.55f);
UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(lgo, scene);
var cgo = new GameObject("C"); var cam = cgo.AddComponent<Camera>(); UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(cgo, scene);
cam.scene = scene; cam.clearFlags = CameraClearFlags.SolidColor; cam.backgroundColor = new Color(0, 0, 0, 0); cam.orthographic = true; cam.enabled = false;
var rends = cat.GetComponentsInChildren<Renderer>();
var b = rends.Select(r => r.bounds).Aggregate((a, c) => { a.Encapsulate(c); return a; });
cam.orthographicSize = Mathf.Max(b.extents.y, b.extents.x) * 1.12f;
cam.transform.position = b.center + new Vector3(0, b.extents.y * 0.25f, -10); cam.transform.LookAt(b.center); cam.nearClipPlane = 0.1f; cam.farClipPlane = 50;
var rt = new RenderTexture(S, S, 24, RenderTextureFormat.ARGB32) { antiAliasing = 4 };
cam.targetTexture = rt;
string Shot(string mat, float yaw, string name) {
  var m = AssetDatabase.LoadAssetAtPath<Material>("Assets/Atrio/Character/Cat/Materials/" + mat + ".mat");
  foreach (var r in rends) r.sharedMaterials = r.sharedMaterials.Select(_ => m).ToArray();
  cat.transform.rotation = Quaternion.Euler(0, yaw, 0);
  cam.Render();
  var prev = RenderTexture.active; RenderTexture.active = rt;
  var t = new Texture2D(S, S, TextureFormat.RGBA32, false); t.ReadPixels(new Rect(0, 0, S, S), 0, 0); t.Apply(); RenderTexture.active = prev;
  var p = DIR + "/" + name + ".png"; System.IO.File.WriteAllBytes(p, t.EncodeToPNG()); return p;
}
var paths = new System.Collections.Generic.List<string>();
float[] yaws = { -35, -20, -5, 10, 25, 40 };
for (int i = 0; i < 6; i++) paths.Add(Shot("Cat_Black", yaws[i], "CatBlack_idle0" + (i + 1)));
paths.Add(Shot("Cat_Tiger", -35, "CatTiger_idle01"));
rt.Release(); EditorSceneManager.ClosePreviewScene(scene);
AssetDatabase.Refresh();
foreach (var p in paths) { var ti = (TextureImporter)AssetImporter.GetAtPath(p); ti.textureType = TextureImporterType.Sprite; ti.spriteImportMode = SpriteImportMode.Single; ti.alphaIsTransparency = true; ti.SaveAndReimport(); }
return "renderers=" + rends.Length + " " + string.Join(",", paths.Select(System.IO.Path.GetFileName));
