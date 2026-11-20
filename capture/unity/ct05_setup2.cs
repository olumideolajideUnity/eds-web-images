using UnityEditor; using UnityEditor.SceneManagement; using UnityEngine; using UnityEngine.ProBuilder; using UnityEngine.ProBuilder.Shapes; using System.Linq; using System.Reflection;
float YR = 0, AR = 0;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
var light = new GameObject("Directional Light").AddComponent<Light>(); light.type = LightType.Directional; light.transform.rotation = Quaternion.Euler(50, -30, 0); light.transform.position = new Vector3(0, 40, -60);
var scType = typeof(ProBuilderMesh).Assembly.GetType("UnityEngine.ProBuilder.Shapes.ProBuilderShape");
GameObject Make(System.Type shape, string name, Vector3 pos, Vector3 size, float yRot) {
  var m = ShapeFactory.Instantiate(shape); m.gameObject.name = name;
  var sc = m.GetComponent(scType);
  scType.GetProperty("size", BF).SetValue(sc, size);
  scType.GetField("m_ShapeRotation", BF).SetValue(sc, Quaternion.Euler(0, yRot, 0));
  scType.GetMethod("Rebuild", BF, null, System.Type.EmptyTypes, null)?.Invoke(sc, null);
  var ver = typeof(ProBuilderMesh).GetProperty("versionIndex", BF)?.GetValue(m);
  if (ver != null) scType.GetField("m_UnmodifiedMeshVersion", BF).SetValue(sc, System.Convert.ToUInt16(ver));
  m.transform.position = pos; return m.gameObject;
}
Make(typeof(Cube), "Base", new Vector3(0, 1.25f, 0), new Vector3(8, 2.5f, 8), 0);
Make(typeof(Stairs), "Stairs Left", new Vector3(-6, 1.25f, 0), new Vector3(4, 2.5f, 3), 90);
Make(typeof(Stairs), "Stairs Right", new Vector3(0, 1.25f, -6), new Vector3(4, 2.5f, 3), YR);
var arch = Make(typeof(Arch), "Arch", new Vector3(0, 4.3f, 0), new Vector3(5, 3.6f, 4), AR);
{ var sc = arch.GetComponent(scType); var a = scType.GetProperty("shape", BF).GetValue(sc);
  a.GetType().GetField("m_Thickness", BF)?.SetValue(a, 1.1f); a.GetType().GetField("m_NumberOfSides", BF)?.SetValue(a, 12);
  scType.GetMethod("Rebuild", BF, null, System.Type.EmptyTypes, null)?.Invoke(sc, null);
  var ver = typeof(ProBuilderMesh).GetProperty("versionIndex", BF)?.GetValue(arch.GetComponent<ProBuilderMesh>());
  if (ver != null) scType.GetField("m_UnmodifiedMeshVersion", BF).SetValue(sc, System.Convert.ToUInt16(ver)); }
EditorSceneManager.SaveScene(scene, "Assets/EFDS_Screenshots/ProBuilderArch.unity");
Selection.activeGameObject = arch;
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
sv.sceneViewState.showSkybox = false; sv.drawGizmos = true; sv.showGrid = true;
sv.LookAtDirect(new Vector3(0.4f, 2.2f, 0), Quaternion.Euler(30f, -38f, 0f), 9.5f);
sv.Repaint();
return "built";
