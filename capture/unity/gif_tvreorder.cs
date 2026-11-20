// tv-reorder half.gif setup (user screen recording): an empty scene saved as Assets/EFDS_Screenshots/GIF/SampleScene.unity (so the
// Hierarchy reads "SampleScene*" like the original), then plain GameObjects in the original's order: Main Camera, Canvas (two
// "Text (TMP)" children, collapsed), EventSystem, Directional Light, Cube, Sphere. One floating Unity 7 Hierarchy, W x H pt
// (original 800x600 px at 2 px/pt).
using UnityEditor; using UnityEditor.SceneManagement; using UnityEngine; using System.Linq;
float W = 400, H = 300;
var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
System.IO.Directory.CreateDirectory("Assets/EFDS_Screenshots/GIF");
EditorSceneManager.SaveScene(scene, "Assets/EFDS_Screenshots/GIF/SampleScene.unity");
new GameObject("Main Camera");
var canvas = new GameObject("Canvas");
new GameObject("Text (TMP)").transform.SetParent(canvas.transform); new GameObject("Text (TMP)").transform.SetParent(canvas.transform);
new GameObject("EventSystem"); new GameObject("Directional Light"); new GameObject("Cube"); new GameObject("Sphere");
EditorSceneManager.MarkSceneDirty(scene);
var BF = System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic;
var t = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("Unity.Hierarchy.Editor.HierarchyWindow")).First(x => x != null);
var floats = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().Where(x => !x.docked).ToList();
foreach (var extra in floats.Skip(1)) extra.Close();
var w = floats.FirstOrDefault(); if (w == null) { w = (EditorWindow)ScriptableObject.CreateInstance(t); w.Show(); }
var d = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(w); var cw = d.GetType().GetProperty("window", BF).GetValue(d);
var r = (Rect)cw.GetType().GetProperty("position", BF).GetValue(cw); r.width = W; r.height = H;
cw.GetType().GetProperty("position", BF).SetValue(cw, r);
t.GetMethods(BF).First(m => m.Name == "SetSearchText" && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(string)).Invoke(w, new object[]{ "" });
Selection.activeObject = null; w.Focus(); w.Repaint();
return "scene " + scene.name + " dirty " + scene.isDirty + " hier " + r;
