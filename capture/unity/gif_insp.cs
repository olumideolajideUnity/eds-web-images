// GIF frames: a never-saved object showing only the components in SHOW (others hidden from the Inspector), in a floating
// Inspector W x H pt. KIND: cube (MeshFilter + MeshRenderer, URP default material) | camera | light.
using UnityEditor; using UnityEngine; using UnityEngine.Rendering; using System.Linq;
string KIND = "cube"; string SHOW = "MeshRenderer"; float W = 400, H = 520;
foreach (var g in Resources.FindObjectsOfTypeAll<GameObject>().Where(x => x.name.StartsWith("EFDS GIF")).ToList()) Object.DestroyImmediate(g);
GameObject go;
if (KIND == "cube") { go = GameObject.CreatePrimitive(PrimitiveType.Cube); Object.DestroyImmediate(go.GetComponent<Collider>());
  var m = GraphicsSettings.currentRenderPipeline != null ? GraphicsSettings.currentRenderPipeline.defaultMaterial : AssetDatabase.GetBuiltinExtraResource<Material>("Default-Material.mat");
  go.GetComponent<MeshRenderer>().sharedMaterial = m; }
else if (KIND == "camera") { go = new GameObject(); go.AddComponent<Camera>(); }
else { go = new GameObject(); var l = go.AddComponent<Light>(); l.type = LightType.Directional; l.color = new Color(1f, 0.957f, 0.839f); }
go.name = "EFDS GIF " + KIND; go.hideFlags = HideFlags.DontSave;
var show = SHOW.Split(',');
foreach (var c in go.GetComponents<Component>()) { bool s = show.Contains(c.GetType().Name); if (!s) c.hideFlags |= HideFlags.HideInInspector; UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(c, s); }
Selection.activeGameObject = go;
var it = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
var insp = Resources.FindObjectsOfTypeAll(it).Cast<EditorWindow>().FirstOrDefault(w => !w.docked);
if (insp == null) { insp = (EditorWindow)ScriptableObject.CreateInstance(it); insp.Show(); }
insp.position = new Rect(800, 120, W, H); ActiveEditorTracker.sharedTracker.ForceRebuild(); insp.wantsMouseMove = true; insp.Repaint();
return "ok " + string.Join(",", go.GetComponents<Component>().Select(c => c.GetType().Name));
