// Components > Color field 'colorPickers': Unity's ColorPicker for #68615B (alpha shown, swatches folded like the original), set up
// as ColorPicker.Show does but as a utility window so it stays open for capture (capwin ColorPicker). H = window height (pt).
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
float H = 470;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var cpT = typeof(Editor).Assembly.GetType("UnityEditor.ColorPicker");
foreach (var w in Resources.FindObjectsOfTypeAll(cpT).Cast<EditorWindow>().ToList()) w.Close();
var cp = (EditorWindow)cpT.GetProperty("instance", BF).GetValue(null);
var cm = typeof(Editor).Assembly.GetType("UnityEditor.ColorMutator");
cpT.GetField("m_HDR", BF).SetValue(cp, false);
cpT.GetField("m_Color", BF).SetValue(cp, System.Activator.CreateInstance(cm, (Color)new Color32(104, 97, 91, 255)));
cpT.GetField("m_ShowAlpha", BF).SetValue(cp, true);
cp.titleContent = new GUIContent("Color");
var styles = cpT.GetNestedType("Styles", BF);
float fw = System.Convert.ToSingle(styles.GetField("fixedWindowWidth", BF)?.GetValue(null) ?? styles.GetProperty("fixedWindowWidth", BF).GetValue(null));
cpT.GetMethod("InitializePresetsLibraryIfNeeded", BF)?.Invoke(cp, null);
string bools = string.Join(",", cpT.GetFields(BF).Where(f => f.FieldType == typeof(bool) && !f.IsStatic).Select(f => f.Name + "=" + f.GetValue(cp)));
foreach (var f in cpT.GetFields(BF).Where(f => f.FieldType == typeof(bool) && !f.IsStatic && (f.Name.Contains("Preset") || f.Name.Contains("Swatch") || f.Name.Contains("Default")))) f.SetValue(cp, false);
EditorPrefs.SetBool("CPPresetsFoldout", false); EditorPrefs.SetBool("CPSwatchesFoldout", false);
cp.minSize = new Vector2(fw, H); cp.maxSize = new Vector2(fw, H);
cp.ShowUtility(); cp.position = new Rect(600, 120, fw, H); cp.Repaint();
return "picker " + cp.position + " " + bools;
