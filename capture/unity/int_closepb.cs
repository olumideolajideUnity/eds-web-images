using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var cwT = typeof(Editor).Assembly.GetType("UnityEditor.ContainerWindow");
bool NotMain(EditorWindow x) { var p = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(x); var c = p?.GetType().GetProperty("window", BF)?.GetValue(p); return c != null && (int)cwT.GetProperty("showMode", BF).GetValue(c) != 4; }
int n = 0; foreach (var w in Resources.FindObjectsOfTypeAll<EditorWindow>().Where(x => x.GetType().Name == "ProjectBrowser" && NotMain(x)).ToList()) { w.Close(); n++; }
UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
return "closed " + n + " pro=" + EditorGUIUtility.isProSkin;
