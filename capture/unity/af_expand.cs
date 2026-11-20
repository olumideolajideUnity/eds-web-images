// Expand every component of the selected GameObject in the Inspector (and rebuild the tracker).
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var go = Selection.activeGameObject; if (!go) return "no selection";
foreach (var c in go.GetComponents<Component>()) UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(c, true);
var r = go.GetComponent<Renderer>(); if (r) foreach (var m in r.sharedMaterials) if (m) UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(m, false);
ActiveEditorTracker.sharedTracker.ForceRebuild();
var it = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
foreach (var w in Resources.FindObjectsOfTypeAll(it).Cast<EditorWindow>()) { (it.GetProperty("tracker", BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic)?.GetValue(w) as ActiveEditorTracker)?.ForceRebuild(); w.Repaint(); }
return "expanded " + go.name;
