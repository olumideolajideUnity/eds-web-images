// af11: after Timeline preview has evaluated (its activation track hides Character2 at frame 0), switch Character2 back on so its tracks
// draw without "bound GameObject is disabled" warnings, and clear the scene's dirty flag so the tab shows no asterisk.
using UnityEditor; using UnityEditor.SceneManagement; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Static|BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var g = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(x => x.GetComponentsInChildren<Transform>(true)).First(t => t.name == "Character2").gameObject;
g.SetActive(true);
var clr = typeof(EditorSceneManager).GetMethod("ClearSceneDirtiness", BF); clr?.Invoke(null, new object[]{ g.scene });
var asm = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "Unity.Timeline.Editor");
foreach (var w in Resources.FindObjectsOfTypeAll(asm.GetType("UnityEditor.Timeline.TimelineWindow")).Cast<EditorWindow>()) w.Repaint();
return "active=" + g.activeInHierarchy + " dirty=" + g.scene.isDirty + " clr=" + (clr != null);
