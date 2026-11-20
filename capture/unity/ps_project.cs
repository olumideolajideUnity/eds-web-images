// Floating Project browser (W x H pt) in two-column layout on Assets/EFDS_Screenshots.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
float W = 520, H = 220;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var t = typeof(Editor).Assembly.GetType("UnityEditor.ProjectBrowser");
var w = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().FirstOrDefault(x => !x.docked);
if (w == null) { w = (EditorWindow)ScriptableObject.CreateInstance(t); w.Show(); }
w.position = new Rect(200, 150, W, H);
Selection.activeObject = AssetDatabase.LoadMainAssetAtPath("Assets/EFDS_Screenshots");
w.Repaint(); return "ok " + w.position;
