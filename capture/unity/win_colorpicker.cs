// Unity's ColorPicker set up exactly as ColorPicker.Show does (for the Directional Light's colour), but shown as a utility window
// instead of an aux window so it stays open for capture; plus a floating Inspector on the light with only the Light component open.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
float IW = 300, IH = 420;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var light = Object.FindFirstObjectByType<Light>(); light.color = new Color(1f, 0.93f, 0.84f); UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(light.gameObject.scene); UnityEditor.SceneManagement.EditorSceneManager.SaveScene(light.gameObject.scene);
foreach (var c in light.GetComponents<Component>()) UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(c, c is Light);
Selection.activeGameObject = light.gameObject;
var it = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
var iw = Resources.FindObjectsOfTypeAll(it).Cast<EditorWindow>().FirstOrDefault(x => !x.docked);
if (iw == null) { iw = (EditorWindow)ScriptableObject.CreateInstance(it); iw.Show(); }
iw.position = new Rect(150, 100, IW, IH); (it.GetProperty("tracker", BF)?.GetValue(iw) as ActiveEditorTracker)?.ForceRebuild(); iw.Repaint();
var cpT = typeof(Editor).Assembly.GetType("UnityEditor.ColorPicker");
foreach (var w in Resources.FindObjectsOfTypeAll(cpT).Cast<EditorWindow>().ToList()) w.Close();
var cp = (EditorWindow)cpT.GetProperty("instance", BF).GetValue(null);
var cm = typeof(Editor).Assembly.GetType("UnityEditor.ColorMutator");
cpT.GetField("m_HDR", BF).SetValue(cp, false);
cpT.GetField("m_Color", BF).SetValue(cp, System.Activator.CreateInstance(cm, light.color));
cpT.GetField("m_ShowAlpha", BF).SetValue(cp, false);
cp.titleContent = new GUIContent("Color");
var styles = cpT.GetNestedType("Styles", BF);
float fw = System.Convert.ToSingle(styles.GetField("fixedWindowWidth", BF)?.GetValue(null) ?? styles.GetProperty("fixedWindowWidth", BF).GetValue(null));
float h = EditorPrefs.GetInt("CPickerHeight", 360);
cp.minSize = new Vector2(fw, h); cp.maxSize = new Vector2(fw, h);
cpT.GetMethod("InitializePresetsLibraryIfNeeded", BF)?.Invoke(cp, null);
cp.ShowUtility(); cp.position = new Rect(600, 120, fw, h); cp.Repaint();
return "picker " + cp.position + " light=" + light.color;
