// Scroll the Project Settings content panel (IMGUI scroll position m_PosRight) to Y pt.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
float Y = 0;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var t = typeof(Editor).Assembly.GetType("UnityEditor.ProjectSettingsWindow");
var w = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().First(x => !x.docked);
var f = t.BaseType.GetField("m_PosRight", BF) ?? t.GetField("m_PosRight", BF);
f.SetValue(w, new Vector2(0, Y));
w.Repaint();
return "ok";
