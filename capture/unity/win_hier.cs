// Windowing (anatomy / states): floating Unity 7 Hierarchy window (Unity.Hierarchy.Editor.HierarchyWindow) at 607 x 400 pt.
using UnityEditor; using UnityEngine; using System.Linq;
var t = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("Unity.Hierarchy.Editor.HierarchyWindow")).First(x => x != null);   // Unity 7 Hierarchy module
var w = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().FirstOrDefault(x => !x.docked);
if (w == null) { w = (EditorWindow)ScriptableObject.CreateInstance(t); w.Show(); }
w.position = new Rect(100, 100, 607, 400); w.Repaint(); return t.FullName + " " + w.position;
