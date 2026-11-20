// Components > Help box positioning: temp unsaved Canvas (DontSave, Screen Space - Overlay) with additional shader channels CH,
// floating Inspector (W x H pt) with only the Canvas component expanded.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
float W = 320, H = 260; int CH = 0; bool VCG = false;   // CH: 0 = Nothing; 24 = Normal | Tangent. VCG: Vertex Color Always In Gamma Color Space
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
foreach (var g in Resources.FindObjectsOfTypeAll<GameObject>().Where(x => x.name == "Canvas" && x.hideFlags == HideFlags.DontSave).ToList()) Object.DestroyImmediate(g);
var go = new GameObject("Canvas") { hideFlags = HideFlags.DontSave }; go.layer = 5;
var c = go.AddComponent<Canvas>(); c.renderMode = RenderMode.ScreenSpaceOverlay; c.additionalShaderChannels = (AdditionalCanvasShaderChannels)CH; c.vertexColorAlwaysGammaSpace = VCG;
foreach (var comp in go.GetComponents<Component>()) UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(comp, comp is Canvas);
Selection.activeGameObject = go;
var it = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
var cwT = typeof(Editor).Assembly.GetType("UnityEditor.ContainerWindow");
bool NotMain(EditorWindow x) { var p = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(x); var cc = p?.GetType().GetProperty("window", BF)?.GetValue(p); return cc != null && (int)cwT.GetProperty("showMode", BF).GetValue(cc) != 4; }
var w = Resources.FindObjectsOfTypeAll(it).Cast<EditorWindow>().FirstOrDefault(NotMain);
if (w == null) { w = (EditorWindow)ScriptableObject.CreateInstance(it); w.Show(); }
var d = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(w); var cw = d.GetType().GetProperty("window", BF).GetValue(d);
cw.GetType().GetProperty("position", BF).SetValue(cw, new Rect(300, 100, W, H));
(it.GetProperty("tracker", BF)?.GetValue(w) as ActiveEditorTracker)?.ForceRebuild(); w.Repaint();
return "channels=" + c.additionalShaderChannels;
