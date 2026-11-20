// Accessibility a11y-3: a small realtime-GI scene (Enlighten) so the Profiler's Realtime GI module has data.
// Saved as Assets/EFDS_Screenshots/RealtimeGI.unity, then the GI precompute is started (Lightmapping.BakeAsync).
using UnityEditor; using UnityEditor.SceneManagement; using UnityEngine; using System.Linq;
const string PATH = "Assets/EFDS_Screenshots/RealtimeGI.unity", LS = "Assets/EFDS_Screenshots/RealtimeGI.lighting";
var sc = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
var flags = StaticEditorFlags.ContributeGI | StaticEditorFlags.ReflectionProbeStatic | StaticEditorFlags.BatchingStatic;
GameObject Prim(PrimitiveType t, string n, Vector3 p, Vector3 s, Color c) {
  var g = GameObject.CreatePrimitive(t); g.name = n; g.transform.position = p; g.transform.localScale = s;
  var m = new Material(Shader.Find("Universal Render Pipeline/Lit")); m.color = c; g.GetComponent<Renderer>().sharedMaterial = m;
  GameObjectUtility.SetStaticEditorFlags(g, flags); return g; }
Prim(PrimitiveType.Plane, "Floor", Vector3.zero, Vector3.one * 2, new Color(0.8f, 0.8f, 0.8f));
Prim(PrimitiveType.Cube, "Wall Red", new Vector3(-4, 2, 0), new Vector3(0.5f, 4, 8), new Color(0.9f, 0.2f, 0.2f));
Prim(PrimitiveType.Cube, "Wall Green", new Vector3(4, 2, 0), new Vector3(0.5f, 4, 8), new Color(0.2f, 0.8f, 0.3f));
Prim(PrimitiveType.Cube, "Back Wall", new Vector3(0, 2, 4), new Vector3(8, 4, 0.5f), new Color(0.9f, 0.9f, 0.9f));
Prim(PrimitiveType.Cube, "Box", new Vector3(1, 0.75f, -1), new Vector3(1.5f, 1.5f, 1.5f), new Color(0.3f, 0.5f, 0.9f));
var lg = new GameObject("Directional Light"); var l = lg.AddComponent<Light>(); l.type = LightType.Directional; l.lightmapBakeType = LightmapBakeType.Realtime;
l.intensity = 1.5f; lg.transform.rotation = Quaternion.Euler(40, 30, 0); RenderSettings.sun = l;
var pl = new GameObject("Point Light"); var p2 = pl.AddComponent<Light>(); p2.type = LightType.Point; p2.lightmapBakeType = LightmapBakeType.Realtime; p2.range = 8; p2.intensity = 3; pl.transform.position = new Vector3(0, 2.5f, -1);
var cam = new GameObject("Main Camera"); cam.tag = "MainCamera"; cam.AddComponent<Camera>(); cam.transform.position = new Vector3(0, 3, -9); cam.transform.LookAt(new Vector3(0, 1.5f, 0));
var ls = new LightingSettings { name = "RealtimeGI", realtimeGI = true, bakedGI = false, autoGenerate = false, indirectResolution = 2 };
AssetDatabase.DeleteAsset(LS); AssetDatabase.CreateAsset(ls, LS);
Lightmapping.lightingSettings = ls;
EditorSceneManager.SaveScene(sc, PATH);
var ok = Lightmapping.BakeAsync();
return "saved " + PATH + " bake=" + ok;
