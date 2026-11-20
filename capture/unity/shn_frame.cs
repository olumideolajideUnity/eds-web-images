// Docked Scene view: aerial framing of Atrio's Environment; close floating EFDS Inspector; refresh docked Inspector on the camera.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
float PITCH = 40, YAW = -35, SIZEK = 0.55f;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var it = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
foreach (var w in Resources.FindObjectsOfTypeAll(it).Cast<EditorWindow>().Where(x => !x.docked).ToList()) w.Close();
var env = GameObject.Find("Environment");
var rs = env.GetComponentsInChildren<Renderer>(); var b = rs[0].bounds; foreach (var r in rs) b.Encapsulate(r.bounds);
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(s => s.docked);
sv.LookAtDirect(b.center, Quaternion.Euler(PITCH, YAW, 0), b.extents.magnitude * SIZEK);
var cam = Selection.activeGameObject; Selection.activeGameObject = null; Selection.activeGameObject = cam;
foreach (var w in Resources.FindObjectsOfTypeAll(it).Cast<EditorWindow>()) { (it.GetProperty("tracker", BF)?.GetValue(w) as ActiveEditorTracker)?.ForceRebuild(); w.Repaint(); }
sv.Repaint();
return "bounds=" + b + " sel=" + Selection.activeGameObject;
