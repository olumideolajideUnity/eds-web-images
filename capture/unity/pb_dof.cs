// Components > Progress bar overview: a never-saved object carrying only the EFDS.DepthOfField specimen (Transform hidden), shown
// in a floating Inspector W x H pt. The capture's tab strip is spliced onto the component (no GameObject header), as in the original.
using UnityEditor; using UnityEngine; using System.Linq;
float W = 320, H = 300;
foreach (var old in Resources.FindObjectsOfTypeAll<GameObject>().Where(g => g.GetComponent<EFDS.DepthOfField>()).ToList()) Object.DestroyImmediate(old);
var go = new GameObject("Global Volume"); go.hideFlags = HideFlags.DontSave; go.transform.hideFlags = HideFlags.HideInInspector;
var c = go.AddComponent<EFDS.DepthOfField>();
var icon = EditorGUIUtility.IconContent((EditorGUIUtility.isProSkin ? "d_" : "") + "Volume Icon").image as Texture2D;
if (icon != null) EditorGUIUtility.SetIconForObject(MonoScript.FromMonoBehaviour(c), icon);
UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(c, true);
Selection.activeGameObject = go;
var it = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
var insp = Resources.FindObjectsOfTypeAll(it).Cast<EditorWindow>().FirstOrDefault(w => !w.docked);
if (insp == null) { insp = (EditorWindow)ScriptableObject.CreateInstance(it); insp.Show(); }
insp.position = new Rect(800, 120, W, H); ActiveEditorTracker.sharedTracker.ForceRebuild(); insp.Repaint();
return "dof icon=" + (icon != null);
