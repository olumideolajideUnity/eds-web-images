// Fold states for the Inheritance captures. MODE 0: collapse URP "Surface Options" for the Atrio Lit shader (saves the old
// pref to EFDS.saved.<key>), expand Transform, collapse the instance's other components. MODE 1: restore the saved pref.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
int MODE = 0;
var ext = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("UnityEditor.Rendering.MaterialEditorExtension")).First(x => x != null);
var pre = (string)ext.GetField("k_KeyPrefix", BindingFlags.Static|BindingFlags.NonPublic|BindingFlags.Public).GetValue(null);
var key = pre + AssetDatabase.LoadAssetAtPath<Material>("Assets/Atrio/Environment/WoodTrimKit/Materials/WoodTrimKit_Orange.mat").shader.name;
if (MODE == 1) { if (EditorPrefs.HasKey("EFDS.saved." + key)) { var v = EditorPrefs.GetInt("EFDS.saved." + key); if (v == int.MinValue) EditorPrefs.DeleteKey(key); else EditorPrefs.SetInt(key, v); EditorPrefs.DeleteKey("EFDS.saved." + key); } return "restored " + key; }
if (!EditorPrefs.HasKey("EFDS.saved." + key)) EditorPrefs.SetInt("EFDS.saved." + key, EditorPrefs.HasKey(key) ? EditorPrefs.GetInt(key) : int.MinValue);
EditorPrefs.SetInt(key, unchecked((int)0xFFFFFFFE));
var cube = GameObject.Find("Cube");
foreach (var c in cube.GetComponents<Component>()) UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(c, c is Transform);
return "key=" + key;
