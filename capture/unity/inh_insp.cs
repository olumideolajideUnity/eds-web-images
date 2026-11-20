// Floating Inspector W x H showing TARGET ("instance" = the Cube instance, else an asset path).
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
string TARGET = "instance"; float W = 320, H = 300;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
Object o = TARGET == "instance" ? (Object)GameObject.Find("Cube") : AssetDatabase.LoadMainAssetAtPath(TARGET);
Selection.activeObject = o;
var it = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
var w = Resources.FindObjectsOfTypeAll(it).Cast<EditorWindow>().FirstOrDefault(x => !x.docked);
if (w == null) { w = (EditorWindow)ScriptableObject.CreateInstance(it); w.Show(); }
w.position = new Rect(300, 120, W, H);
(it.GetProperty("tracker", BF)?.GetValue(w) as ActiveEditorTracker)?.ForceRebuild();
w.Repaint(); return "sel=" + o;
