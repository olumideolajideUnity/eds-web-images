// Components > Slider 'how to use' (htut-Y / htut-N / layout): a never-saved 'Main Camera' object (tag MainCamera) whose real
// Transform is hidden from the Inspector, carrying one specimen component: MODE "Y" = EFDS.Camera (Field of View slider, Camera
// icon), "N" = EFDS.Transform (Position drawn as three sliders, Transform icon). Sizes the floating Inspector to W x H pt.
using UnityEditor; using UnityEngine; using System.Linq;
string MODE = "Y"; float W = 357, H = 230;
foreach (var old in Resources.FindObjectsOfTypeAll<GameObject>().Where(g => g.name == "Main Camera" && (g.GetComponent<EFDS.Camera>() || g.GetComponent<EFDS.Transform>())).ToList()) Object.DestroyImmediate(old);
var go = new GameObject("Main Camera"); go.hideFlags = HideFlags.DontSave; go.tag = "MainCamera";
go.transform.hideFlags = HideFlags.HideInInspector;
MonoBehaviour c = MODE == "Y" ? go.AddComponent<EFDS.Camera>() : (MonoBehaviour)go.AddComponent<EFDS.Transform>();
var icon = EditorGUIUtility.IconContent(MODE == "Y" ? "Camera Icon" : "Transform Icon").image as Texture2D;
EditorGUIUtility.SetIconForObject(MonoScript.FromMonoBehaviour(c), icon);
UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(c, true);
Selection.activeGameObject = go;
var it = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
var insp = Resources.FindObjectsOfTypeAll(it).Cast<EditorWindow>().FirstOrDefault(w => !w.docked);
if (insp == null) { insp = (EditorWindow)ScriptableObject.CreateInstance(it); insp.Show(); }
insp.position = new Rect(800, 120, W, H);
EFDS.Rects.All.Clear(); ActiveEditorTracker.sharedTracker.ForceRebuild(); insp.Repaint();
return "ok " + MODE + " icon=" + (icon != null) + " insp=" + insp.position;
