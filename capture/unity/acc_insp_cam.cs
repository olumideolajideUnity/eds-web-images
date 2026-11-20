// Accessibility a11y-6/8: temp unsaved "Main Camera" (DontSave) with Camera + Audio Listener in the open scene, floating
// Inspector W x H pt. EXPAND = comma-separated component names to expand ("" = all collapsed).
using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection;
float W = 346, H = 230; string EXPAND = "";
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
foreach (var g in Resources.FindObjectsOfTypeAll<GameObject>().Where(x => x.name == "Main Camera" && x.hideFlags == HideFlags.DontSave).ToList()) Object.DestroyImmediate(g);
var go = new GameObject("Main Camera") { hideFlags = HideFlags.DontSave }; go.tag = "MainCamera"; go.transform.position = new Vector3(0, 1, -10);
go.AddComponent<Camera>(); go.AddComponent<AudioListener>();
foreach (var c in go.GetComponents<Component>()) UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(c, EXPAND.Split(',').Contains(c.GetType().Name));
Selection.activeGameObject = go;
var it = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
var cwT = typeof(Editor).Assembly.GetType("UnityEditor.ContainerWindow");
bool NotMain(EditorWindow x) { var p = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(x); var c = p?.GetType().GetProperty("window", BF)?.GetValue(p); return c != null && (int)cwT.GetProperty("showMode", BF).GetValue(c) != 4; }
var w = Resources.FindObjectsOfTypeAll(it).Cast<EditorWindow>().FirstOrDefault(NotMain);
if (w == null) { w = (EditorWindow)ScriptableObject.CreateInstance(it); w.Show(); }
var d = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(w); var cw = d.GetType().GetProperty("window", BF).GetValue(d);
cw.GetType().GetProperty("position", BF).SetValue(cw, new Rect(300, 100, W, H));
(it.GetProperty("tracker", BF)?.GetValue(w) as ActiveEditorTracker)?.ForceRebuild(); w.Repaint();
return "comps=" + string.Join(",", go.GetComponents<Component>().Select(c => c.GetType().Name));
