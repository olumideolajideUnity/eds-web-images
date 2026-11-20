// Accessibility a11y-4: floating Inspector (W x H pt) on a temp unsaved GameObject with only a Transform (collapsed).
// NE=true marks it NotEditable so the Inspector draws its real disabled state (including Add Component).
// Reports the Add Component button's rect (window pt) in the result.
using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection;
float W = 300, H = 160; bool NE = false;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
foreach (var g in Resources.FindObjectsOfTypeAll<GameObject>().Where(x => x.name == "EFDS_AddComponent").ToList()) Object.DestroyImmediate(g);
var go = new GameObject("EFDS_AddComponent") { hideFlags = HideFlags.DontSave | (NE ? HideFlags.NotEditable : HideFlags.None) };
UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(go.transform, false);
Selection.activeGameObject = go;
var it = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
var cwT = typeof(Editor).Assembly.GetType("UnityEditor.ContainerWindow");
bool NotMain(EditorWindow x) { var p = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(x); var c = p?.GetType().GetProperty("window", BF)?.GetValue(p); return c != null && (int)cwT.GetProperty("showMode", BF).GetValue(c) != 4; }
var w = Resources.FindObjectsOfTypeAll(it).Cast<EditorWindow>().FirstOrDefault(NotMain);
if (w == null) { w = (EditorWindow)ScriptableObject.CreateInstance(it); w.Show(); }
var d = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(w); var cw = d.GetType().GetProperty("window", BF).GetValue(d);
cw.GetType().GetProperty("position", BF).SetValue(cw, new Rect(300, 100, W, H));
(it.GetProperty("tracker", BF)?.GetValue(w) as ActiveEditorTracker)?.ForceRebuild(); w.Repaint();
var b = w.rootVisualElement.Query<Button>().ToList().FirstOrDefault(x => x.text == "Add Component");
return b == null ? "no button yet" : "btn=" + b.worldBound + " enabled=" + b.enabledInHierarchy;
