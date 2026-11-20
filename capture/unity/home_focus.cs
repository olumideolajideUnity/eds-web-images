using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
foreach (var b in Resources.FindObjectsOfTypeAll<EditorWindow>().Where(x => x.GetType().Name == "UIBuilderWindow").ToList()) b.Close();
var cwT = typeof(Editor).Assembly.GetType("UnityEditor.ContainerWindow");
bool NotMain(EditorWindow x) { var p = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(x); var c = p?.GetType().GetProperty("window", BF)?.GetValue(p); return c != null && (int)cwT.GetProperty("showMode", BF).GetValue(c) != 4; }
var pw = Resources.FindObjectsOfTypeAll<EditorWindow>().First(x => x.GetType().Name == "ProjectBrowser" && NotMain(x));
var d = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(pw); var c2 = d.GetType().GetProperty("window", BF).GetValue(d);
c2.GetType().GetProperty("position", BF).SetValue(c2, new Rect(1020, 330, 260, 230));
EditorGUIUtility.PingObject(AssetDatabase.LoadMainAssetAtPath("Assets/Atrio/Environment"));
var iw = Resources.FindObjectsOfTypeAll<EditorWindow>().First(x => x.GetType().Name == "InspectorWindow" && NotMain(x));
iw.Focus();
var near = iw.rootVisualElement.Query<FloatField>().ToList().FirstOrDefault(f => f.label == "Near" || (f.parent != null && f.parent.Query<Label>().ToList().Any(l => l.text.StartsWith("Clipping"))));
near?.Focus();
var lab = iw.rootVisualElement.Query<Label>().ToList().FirstOrDefault(l => l.text.StartsWith("Clipping"));
return "near=" + (near != null) + " label=" + lab?.text + " " + lab?.worldBound + " color=" + lab?.resolvedStyle.color;
