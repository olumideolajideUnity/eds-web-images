// Temp unsaved Main Camera in the Windowing scene; floating Inspector W x H; report EditorElement rects to OUT.
using UnityEditor; using UnityEditor.SceneManagement; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection;
float W = 454, H = 382;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
if (EditorSceneManager.GetActiveScene().path != "Assets/EFDS_Screenshots/Windowing.unity") EditorSceneManager.OpenScene("Assets/EFDS_Screenshots/Windowing.unity");
foreach (var g in Resources.FindObjectsOfTypeAll<GameObject>().Where(x => x.name == "Main Camera" && x.hideFlags == HideFlags.DontSave).ToList()) Object.DestroyImmediate(g);
var go = new GameObject("Main Camera") { hideFlags = HideFlags.DontSave }; go.tag = "MainCamera"; go.transform.position = new Vector3(0, 1, -10); go.AddComponent<Camera>();
foreach (var c in go.GetComponents<Component>()) UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(c, true);
Selection.activeGameObject = go;
var it = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
var cwT = typeof(Editor).Assembly.GetType("UnityEditor.ContainerWindow");
bool NotMain(EditorWindow x) { var p = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(x); var c = p?.GetType().GetProperty("window", BF)?.GetValue(p); return c != null && (int)cwT.GetProperty("showMode", BF).GetValue(c) != 4; }
var w = Resources.FindObjectsOfTypeAll(it).Cast<EditorWindow>().FirstOrDefault(NotMain);
if (w == null) { w = (EditorWindow)ScriptableObject.CreateInstance(it); w.Show(); }
var d = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(w); var cw = d.GetType().GetProperty("window", BF).GetValue(d);
cw.GetType().GetProperty("position", BF).SetValue(cw, new Rect(300, 100, W, H));
(it.GetProperty("tracker", BF)?.GetValue(w) as ActiveEditorTracker)?.ForceRebuild(); w.Repaint();
var els = w.rootVisualElement.Query<VisualElement>().ToList().Where(e => e.GetType().Name == "EditorElement").Select(e => "\"" + e.name.Split('_')[0] + "\":[" + e.worldBound.x + "," + e.worldBound.y + "," + e.worldBound.width + "," + e.worldBound.height + "]");
System.IO.File.WriteAllText("OUT", "{" + string.Join(",", els) + "}"); return string.Join(",", els);
