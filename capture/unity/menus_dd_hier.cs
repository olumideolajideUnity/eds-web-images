// Components > Dropdown menus-dd: floating Hierarchy (W x H pt) on the open scene, nothing selected, root expanded one level.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
float W = 156, H = 300;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
Selection.activeObject = null;
var t = typeof(Editor).Assembly.GetType("UnityEditor.SceneHierarchyWindow");
var cwT = typeof(Editor).Assembly.GetType("UnityEditor.ContainerWindow");
bool NotMain(EditorWindow x) { var p = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(x); var c = p?.GetType().GetProperty("window", BF)?.GetValue(p); return c != null && (int)cwT.GetProperty("showMode", BF).GetValue(c) != 4; }
var w = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().FirstOrDefault(NotMain);
if (w == null) { w = (EditorWindow)ScriptableObject.CreateInstance(t); w.Show(); }
w.minSize = new Vector2(100, 100);
var host = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(w); var cw = host.GetType().GetProperty("window", BF).GetValue(host);
cw.GetType().GetProperty("position", BF).SetValue(cw, new Rect(100, 150, W, H));
var sh = t.GetField("m_SceneHierarchy", BF).GetValue(w);
sh.GetType().GetMethod("SetScenesExpanded", BF).Invoke(sh, new object[]{ new System.Collections.Generic.List<string>{ UnityEngine.SceneManagement.SceneManager.GetActiveScene().name } });
w.Repaint(); return "hier " + cw.GetType().GetProperty("position", BF).GetValue(cw);
