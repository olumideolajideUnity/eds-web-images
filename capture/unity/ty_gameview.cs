// Typography profiler recording: a temporary floating Game view so the scene actually renders in Play mode (OPEN=false closes it).
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
bool OPEN = true;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var gvT = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
var cwT = typeof(Editor).Assembly.GetType("UnityEditor.ContainerWindow");
bool NotMain(EditorWindow x) { var p = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(x); var c = p?.GetType().GetProperty("window", BF)?.GetValue(p); return c != null && (int)cwT.GetProperty("showMode", BF).GetValue(c) != 4; }
var mine = Resources.FindObjectsOfTypeAll(gvT).Cast<EditorWindow>().Where(x => x.titleContent.text == "Game" && NotMain(x) && x.name == "EFDS_TyGame").ToList();
if (!OPEN) { foreach (var x in mine) x.Close(); return "closed " + mine.Count; }
if (mine.Count == 0) { var g = (EditorWindow)ScriptableObject.CreateInstance(gvT); g.name = "EFDS_TyGame"; g.Show(); g.position = new Rect(1000, 600, 480, 300); }
return "open";
