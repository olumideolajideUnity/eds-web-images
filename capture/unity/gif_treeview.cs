// treeview.gif setup (user screen recording): a new Untitled scene with plain GameObjects named as in the original
// (Main Camera, Directional Light, Parent > Child 1-4, Sibling of parent; empties so every row shows the GameObject icon),
// Parent expanded, nothing selected, and one floating Unity 7 Hierarchy at the original's framing (709x445 px at 1.5 px/pt = 473x297 pt).
using UnityEditor; using UnityEditor.SceneManagement; using UnityEngine; using System.Linq;
float W = 473, H = 297;
EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
new GameObject("Main Camera"); new GameObject("Directional Light");
var parent = new GameObject("Parent"); GameObject last = null;
for (int i = 1; i <= 4; i++) { last = new GameObject("Child " + i); last.transform.SetParent(parent.transform); }
new GameObject("Sibling of parent");
EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
var BF = System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic;
var t = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("Unity.Hierarchy.Editor.HierarchyWindow")).First(x => x != null);
var floats = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().Where(x => !x.docked).ToList();
foreach (var extra in floats.Skip(1)) extra.Close();
var w = floats.FirstOrDefault(); if (w == null) { w = (EditorWindow)ScriptableObject.CreateInstance(t); w.Show(); }
var d = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(w); var cw = d.GetType().GetProperty("window", BF).GetValue(d);
cw.GetType().GetProperty("position", BF).SetValue(cw, new Rect(500, 250, W, H));
t.GetMethods(BF).First(m => m.Name == "SetSearchText" && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(string)).Invoke(w, new object[]{ "" });
Selection.activeObject = last; EditorGUIUtility.PingObject(last); Selection.activeObject = null;
w.Focus(); w.Repaint();
return "scene " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + " hier " + w.position;
