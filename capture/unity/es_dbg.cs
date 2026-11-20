using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Static|BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var t = typeof(Editor).Assembly.GetType("UnityEditor.GUIViewDebuggerWindow");
foreach (var old in Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().ToList()) old.Close();
var w = (EditorWindow)ScriptableObject.CreateInstance(t);
w.Show(); w.position = new Rect(150, 150, 800, 300);
t.GetMethod("InitDebuggerWindow", BF)?.Invoke(w, null);
// inspect the docked Inspector's view
var it = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
var insp = Resources.FindObjectsOfTypeAll(it).Cast<EditorWindow>().First(x => x.docked);
var host = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(insp);
t.GetProperty("inspected", BF).SetValue(w, host);
w.Repaint();
return "inspected=" + t.GetProperty("inspected", BF).GetValue(w) + " type=" + t.GetProperty("instructionType", BF)?.GetValue(w);
