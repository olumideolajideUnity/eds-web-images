// Components > Color field anatomy: a UI Toolkit ColorField (COL) in a plain host window (800 x 300pt, field at 40..216 x ~147pt),
// and Unity's ColorPicker set up as ColorPicker.Show does for that colour (alpha shown), as a utility window so it stays open.
using UnityEditor; using UnityEditor.UIElements; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
Color COL = new Color32(58, 80, 128, 255);
var hostT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSPopupHost")).First(x => x != null);
foreach (var o in Resources.FindObjectsOfTypeAll(hostT).Cast<EditorWindow>().ToList()) o.Close();
var w = (EditorWindow)ScriptableObject.CreateInstance(hostT); w.titleContent = new GUIContent("EFDSPopupHost"); w.ShowUtility(); w.position = new Rect(200, 150, 800, 300);
var root = w.rootVisualElement; root.Clear();
var cf = new ColorField { value = COL, showEyeDropper = true, showAlpha = true }; cf.style.position = Position.Absolute; cf.style.left = 40; cf.style.width = 176; cf.style.top = 138; root.Add(cf);
root.schedule.Execute(() => System.IO.File.WriteAllText("C:/tmp/cf_an.json", "{\"field\":[" + cf.layout.x + "," + cf.layout.y + "," + cf.layout.width + "," + cf.layout.height + "]," +
  string.Join(",", cf.Query<VisualElement>().ToList().Where(e => e.ClassListContains("unity-color-field__color") || e.ClassListContains("unity-color-field__eyedropper") || e.ClassListContains("unity-color-field__alpha")).Select(e => "\"" + e.GetClasses().First() + "\":[" + e.worldBound.x + "," + e.worldBound.y + "," + e.worldBound.width + "," + e.worldBound.height + "]")) + "}")).StartingIn(300);
var cpT = typeof(Editor).Assembly.GetType("UnityEditor.ColorPicker");
foreach (var x in Resources.FindObjectsOfTypeAll(cpT).Cast<EditorWindow>().ToList()) x.Close();
var cp = (EditorWindow)cpT.GetProperty("instance", BF).GetValue(null);
var cm = typeof(Editor).Assembly.GetType("UnityEditor.ColorMutator");
cpT.GetField("m_HDR", BF).SetValue(cp, false);
cpT.GetField("m_Color", BF).SetValue(cp, System.Activator.CreateInstance(cm, COL));
cpT.GetField("m_ShowAlpha", BF).SetValue(cp, true);
cp.titleContent = new GUIContent("Color");
var styles = cpT.GetNestedType("Styles", BF);
float fw = System.Convert.ToSingle(styles.GetField("fixedWindowWidth", BF)?.GetValue(null) ?? styles.GetProperty("fixedWindowWidth", BF).GetValue(null));
float h = 400; cp.minSize = new Vector2(fw, h); cp.maxSize = new Vector2(fw, h);
cpT.GetMethod("InitializePresetsLibraryIfNeeded", BF)?.Invoke(cp, null);
cp.ShowUtility(); cp.position = new Rect(1100, 120, fw, h); cp.Repaint();
return "picker w=" + fw;
