// Make component tools / tool contexts available for the current selection.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
ActiveEditorTracker.sharedTracker.ForceRebuild();
var it = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
foreach (var w in Resources.FindObjectsOfTypeAll(it).Cast<EditorWindow>()) { ((ActiveEditorTracker)it.GetProperty("tracker", BF).GetValue(w)).ForceRebuild(); w.Repaint(); }
typeof(Editor).Assembly.GetTypes().First(x => x.Name == "EditorToolManager").GetMethod("ForceTrackerRebuild", BF).Invoke(null, null);
return "editors=" + string.Join(",", ActiveEditorTracker.sharedTracker.activeEditors.Select(e => e.GetType().Name));
