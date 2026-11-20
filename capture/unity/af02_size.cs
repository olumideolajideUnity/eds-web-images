using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var w = Resources.FindObjectsOfTypeAll<EditorWindow>().First(x => x.GetType().Name == "MaterialGraphEditWindow");
var parent = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(w);           // DockArea / HostView
var container = parent.GetType().GetProperty("window", BF).GetValue(parent);      // ContainerWindow
var posProp = container.GetType().GetProperty("position", BF);
posProp.SetValue(container, new Rect(300, 120, 700, 457));
// list the graph editor view's settings so we can hide Blackboard / Graph Inspector
var gev = w.GetType().GetProperty("graphEditorView", BF)?.GetValue(w) ?? w.GetType().GetField("m_GraphEditorView", BF)?.GetValue(w);
string fields = gev == null ? "no gev" : string.Join(",", gev.GetType().GetFields(BF).Select(f => f.Name).Where(n => n.ToLower().Contains("setting") || n.ToLower().Contains("blackboard") || n.ToLower().Contains("inspector")));
return "container=" + posProp.GetValue(container) + " win=" + w.position + " gev fields: " + fields;
