// Components > Help box overview: a never-saved Canvas (Screen Space - Overlay) with Additional Shader Channels Normal + Tangent,
// so the real Canvas inspector shows Unity's own warning help box. The Rect Transform is hidden from the Inspector; floating
// Inspector W x H pt (capdock InspectorWindow, then crop to the Canvas component).
using UnityEditor; using UnityEngine; using System.Linq;
float W = 320, H = 380;
foreach (var old in Resources.FindObjectsOfTypeAll<GameObject>().Where(g => g.name == "Canvas").ToList()) Object.DestroyImmediate(old);
var go = new GameObject("Canvas", typeof(Canvas)); go.hideFlags = HideFlags.DontSave;
var c = go.GetComponent<Canvas>(); c.renderMode = RenderMode.ScreenSpaceOverlay;
c.additionalShaderChannels = AdditionalCanvasShaderChannels.Normal | AdditionalCanvasShaderChannels.Tangent;
go.transform.hideFlags = HideFlags.HideInInspector;
UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(c, true);
Selection.activeGameObject = go;
var it = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
var insp = Resources.FindObjectsOfTypeAll(it).Cast<EditorWindow>().FirstOrDefault(w => !w.docked);
if (insp == null) { insp = (EditorWindow)ScriptableObject.CreateInstance(it); insp.Show(); }
insp.position = new Rect(800, 120, W, H); ActiveEditorTracker.sharedTracker.ForceRebuild(); insp.Repaint();
return "canvas " + c.additionalShaderChannels;
