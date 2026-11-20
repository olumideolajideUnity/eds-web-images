using UnityEditor; using UnityEngine; using System.Linq;
var t = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("UnityEditor.Splines.SplineHandleSettingsWindow")).First(x => x != null);
foreach (var w in Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().ToList()) w.Close();
var win = (EditorWindow)ScriptableObject.CreateInstance(t);
win.ShowUtility();
win.position = new Rect(900, 300, 150, 80);
win.Repaint();
return "shown " + win.position;
