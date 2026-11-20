using UnityEditor; using UnityEngine; using System.Linq;
var t = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("Unity.PlasticSCM.Editor.UVCSWindow")).First(x => x != null);
var w = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().FirstOrDefault();
if (w == null) { w = (EditorWindow)ScriptableObject.CreateInstance(t); w.Show(); }
w.minSize = new Vector2(300, 200); w.position = new Rect(560, 150, 600, 324);
w.Repaint();
return "title=" + w.titleContent.text + " docked=" + w.docked + " " + w.position;
