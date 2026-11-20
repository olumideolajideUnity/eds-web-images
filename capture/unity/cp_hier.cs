// Color palette > Base Layers: SampleScene (unsaved, so it reads "SampleScene*") with the original's Hierarchy contents,
// shown in a floating Hierarchy window (W x H pt incl. tab).
using UnityEditor; using UnityEngine; using UnityEditor.SceneManagement; using System.Linq; using System.Reflection;
float W = 420, H = 300; string PREFAB = "";
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var sc = EditorSceneManager.GetActiveScene();
if (sc.path != "Assets/Scenes/SampleScene.unity") sc = EditorSceneManager.OpenScene("Assets/Scenes/SampleScene.unity");
var keep = new[]{ "Main Camera", "Directional Light" };
foreach (var g in sc.GetRootGameObjects()) if (!keep.Contains(g.name)) g.SetActive(true);
string roots0 = string.Join(",", sc.GetRootGameObjects().Select(g => g.name));
if (GameObject.Find("Reflection Probe") == null) {
  var pf = AssetDatabase.LoadAssetAtPath<GameObject>(PREFAB);
  var pi = (GameObject)PrefabUtility.InstantiatePrefab(pf, sc); pi.name = "New Prefab";
  GameObject L(string n, LightType t) { var g = new GameObject(n); g.AddComponent<Light>().type = t; return g; }
  L("Point Light", LightType.Point); L("Area Light", LightType.Rectangle); L("Spot Light", LightType.Spot);
  new GameObject("Light Probe Group").AddComponent<LightProbeGroup>();
  new GameObject("Reflection Probe").AddComponent<ReflectionProbe>();
}
Selection.activeObject = null;
var t = typeof(Editor).Assembly.GetType("UnityEditor.SceneHierarchyWindow");
var w = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().FirstOrDefault(x => !x.docked);
if (w == null) { w = (EditorWindow)ScriptableObject.CreateInstance(t); w.Show(); }
var host = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(w);
var cw = host.GetType().GetProperty("window", BF).GetValue(host);
cw.GetType().GetProperty("position", BF).SetValue(cw, new Rect(300, 150, W, H));
EditorSceneManager.MarkSceneDirty(sc);
var sh = t.GetField("m_SceneHierarchy", BF).GetValue(w);
sh.GetType().GetMethod("SetScenesExpanded", BF).Invoke(sh, new object[]{ new System.Collections.Generic.List<string>{ sc.name } });
w.Repaint();
return "roots before: " + roots0 + " | now: " + string.Join(",", sc.GetRootGameObjects().Select(g => g.name));
