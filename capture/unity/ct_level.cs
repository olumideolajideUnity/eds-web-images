// Contextual tooling #8/#10 model: blocky ProBuilder level piece in the original's colours (ProBuilder grid material + vertex colours).
using UnityEditor; using UnityEditor.SceneManagement; using UnityEngine; using UnityEngine.ProBuilder; using UnityEngine.ProBuilder.Shapes; using UnityEngine.ProBuilder.MeshOperations; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
var light = new GameObject("Directional Light").AddComponent<Light>(); light.type = LightType.Directional;
light.transform.rotation = Quaternion.Euler(48, -35, 0); light.transform.position = new Vector3(0, 40, -60); light.intensity = 0.75f; RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat; RenderSettings.ambientLight = new Color(0.32f, 0.32f, 0.36f);
var scType = typeof(ProBuilderMesh).Assembly.GetType("UnityEngine.ProBuilder.Shapes.ProBuilderShape");
Color blue = new Color(0.0f, 0.17f, 0.72f), red = new Color(0.65f, 0.06f, 0.04f), green = new Color(0.04f, 0.6f, 0.06f), teal = new Color(0.12f, 0.45f, 0.22f), orange = new Color(0.95f, 0.38f, 0.04f);
ProBuilderMesh Make(System.Type shape, string name, Vector3 pos, Vector3 size, Vector3 rot, Color col) {
  var m = ShapeFactory.Instantiate(shape); m.gameObject.name = name;
  var sc = m.GetComponent(scType);
  scType.GetProperty("size", BF).SetValue(sc, size);
  scType.GetMethod("Rebuild", BF, null, System.Type.EmptyTypes, null)?.Invoke(sc, null);
  Object.DestroyImmediate(sc);                      // plain mesh from here on (keeps edits)
  m.transform.position = pos; m.transform.rotation = Quaternion.Euler(rot);
  // subdivide big faces so the grid reads like the original's tiled blocks
  m.GetComponent<MeshRenderer>().sharedMaterial = BuiltinMaterials.defaultMaterial;
  foreach (var f in m.faces) m.SetFaceColor(f, col);
  m.ToMesh(); m.Refresh();
  return m;
}
var main  = Make(typeof(Cube),  "Main Block",  new Vector3(-1.2f, 1.6f, 0f),   new Vector3(4.2f, 3.2f, 4.2f), Vector3.zero, blue);
var slab  = Make(typeof(Cube),  "Low Block",   new Vector3(3.0f, 1.0f, -0.6f), new Vector3(4.8f, 2.0f, 3.0f), Vector3.zero, blue);
// bevel the slab's top-front edge like the original
{ var top = slab.faces.OrderByDescending(f => f.indexes.Average(i => slab.positions[i].y)).First();
  var e = top.edges.OrderByDescending(x => slab.positions[x.a].x + slab.positions[x.b].x).First();
  var nf = Bevel.BevelEdges(slab, new[]{ e }, 0.45f); if (nf != null) foreach (var f in nf) slab.SetFaceColor(f, blue);
  slab.ToMesh(); slab.Refresh(); }
var wedge = Make(typeof(Cube),  "Red Panel",   new Vector3(-3.36f, 1.75f, -0.4f), new Vector3(0.14f, 1.9f, 2.0f), new Vector3(30, 0, 0), red);
var door  = Make(typeof(Cube),  "Green Pillar A", new Vector3(1.75f, 1.05f, -2.4f), new Vector3(0.6f, 2.1f, 0.8f), new Vector3(0, 0, 0), green);
var door2 = Make(typeof(Cube),  "Green Pillar B", new Vector3(2.45f, 0.95f, -2.4f), new Vector3(0.6f, 1.9f, 0.8f), new Vector3(0, 0, 0), green);
var bent  = Make(typeof(Arch),  "Teal Bend",   new Vector3(-2.6f, 0.45f, -2.6f), new Vector3(1.8f, 0.9f, 1.0f), new Vector3(-90, 0, -30), teal);
var box   = Make(typeof(Cube),  "Orange Block",new Vector3(0.1f, 3.5f, 1.2f),   new Vector3(1.2f, 0.6f, 0.8f), new Vector3(0, 15, 0), orange);
// denser grid tiling (UV scale) for the original's tiled-block look
foreach (var m in new[]{ main, slab, wedge, door, door2, bent, box }) {
  foreach (var f in m.faces) { var u = f.uv; u.scale = new Vector2(3.2f, 3.2f); f.uv = u; }
  m.ToMesh(); m.Refresh(); }
var cam = new GameObject("Camera").AddComponent<Camera>();
cam.transform.position = new Vector3(-1.0f, 3.55f, 0.2f); cam.transform.LookAt(new Vector3(0.1f, 3.4f, 1.2f)); cam.transform.Rotate(4, 0, 0);
cam.clearFlags = CameraClearFlags.SolidColor; cam.backgroundColor = new Color(0.27f, 0.22f, 0.32f); cam.fieldOfView = 50;
EditorSceneManager.SaveScene(scene, "Assets/EFDS_Screenshots/ProBuilderLevel.unity");
return "built";
