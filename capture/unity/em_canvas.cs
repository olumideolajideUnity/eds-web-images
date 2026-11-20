// Errors & messaging: temporary (DontSave) Canvas in Overlay mode with Normal + Tangent shader channels, so the Canvas
// inspector shows Unity's own warning help box. Floating Inspector at W pt; reports the Canvas editor element's rect.
using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection;
float W = 480, H = 520;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
foreach (var g in Resources.FindObjectsOfTypeAll<GameObject>().Where(x => x.name == "EFDS Canvas").ToList()) Object.DestroyImmediate(g);
var go = new GameObject("EFDS Canvas") { hideFlags = HideFlags.DontSave };
var cv = go.AddComponent<Canvas>(); cv.renderMode = RenderMode.ScreenSpaceOverlay;
cv.additionalShaderChannels = AdditionalCanvasShaderChannels.Normal | AdditionalCanvasShaderChannels.Tangent;
UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(go.GetComponent<RectTransform>(), false);
UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(cv, true);
Selection.activeGameObject = go;
var it = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
var w = Resources.FindObjectsOfTypeAll(it).Cast<EditorWindow>().FirstOrDefault(x => !x.docked);
if (w == null) { w = (EditorWindow)ScriptableObject.CreateInstance(it); w.Show(); }
w.position = new Rect(300, 120, W, H);
var tr = it.GetProperty("tracker", BF)?.GetValue(w) as ActiveEditorTracker; tr?.ForceRebuild();
w.Repaint();
return "ok " + w.position;
