// Temporary (DontSave) cube; Inspector shows only its Mesh Renderer expanded (Lighting/Probes collapsed, Additional Settings open).
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
float W = 420, H = 600;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
foreach (var g in Resources.FindObjectsOfTypeAll<GameObject>().Where(x => x.name == "EFDS Renderer").ToList()) Object.DestroyImmediate(g);
var go = GameObject.CreatePrimitive(PrimitiveType.Cube); go.name = "EFDS Renderer"; go.hideFlags = HideFlags.DontSave;
foreach (var c in go.GetComponents<Component>()) UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(c, c is MeshRenderer);
Selection.activeGameObject = go;
var it = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
var w = Resources.FindObjectsOfTypeAll(it).Cast<EditorWindow>().FirstOrDefault(x => !x.docked);
if (w == null) { w = (EditorWindow)ScriptableObject.CreateInstance(it); w.Show(); }
w.position = new Rect(300, 100, W, H);
(it.GetProperty("tracker", BF)?.GetValue(w) as ActiveEditorTracker)?.ForceRebuild(); w.Repaint();
return "ok";
