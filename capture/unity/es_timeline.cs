// Floating Timeline window at W x H with either nothing selected (MODE 0) or a GameObject without a director selected (MODE 1).
using UnityEditor; using UnityEditor.SceneManagement; using UnityEngine; using System.Linq;
int MODE = 0; float W = 372, H = 222;
if (EditorSceneManager.GetActiveScene().path != "Assets/EFDS_Screenshots/GOAuthoring.unity") EditorSceneManager.OpenScene("Assets/EFDS_Screenshots/GOAuthoring.unity");
Selection.activeObject = MODE == 1 ? GameObject.Find("Cube") : null;
var t = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("UnityEditor.Timeline.TimelineWindow")).First(x => x != null);
var w = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().FirstOrDefault(x => !x.docked);
if (w == null) { w = (EditorWindow)ScriptableObject.CreateInstance(t); w.Show(); }
w.position = new Rect(200, 200, W, H);
w.Repaint();
return "timeline " + w.position + " sel=" + Selection.activeObject;
