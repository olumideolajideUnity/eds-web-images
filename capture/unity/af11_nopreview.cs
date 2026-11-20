// Timeline: turn Preview off (with Preview on, the Character2 activation track hides it at frame 0 and its tracks show warnings).
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Static|BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var asm = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "Unity.Timeline.Editor");
var w = Resources.FindObjectsOfTypeAll(asm.GetType("UnityEditor.Timeline.TimelineWindow")).Cast<EditorWindow>().First();
var state = w.GetType().GetProperty("state", BF).GetValue(w);
var p = state.GetType().GetProperty("previewMode", BF); var before = p.GetValue(state);
p.SetValue(state, false); w.Repaint();
return "preview " + before + " -> " + p.GetValue(state);
