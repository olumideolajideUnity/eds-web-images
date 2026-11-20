// Components > Help box link: Package Manager window floating at W x H pt, showing PKG's details.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
float W = 1000, H = 520; string PKG = "com.unity.pipeline";
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
UnityEditor.PackageManager.UI.Window.Open(PKG);
var cwT = typeof(Editor).Assembly.GetType("UnityEditor.ContainerWindow");
bool NotMain(EditorWindow x) { var p = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(x); var c = p?.GetType().GetProperty("window", BF)?.GetValue(p); return c != null && (int)cwT.GetProperty("showMode", BF).GetValue(c) != 4; }
var w = Resources.FindObjectsOfTypeAll<EditorWindow>().First(x => x.GetType().Name == "PackageManagerWindow");
string r = "type=" + w.GetType().FullName + " docked=" + w.docked + " notMain=" + NotMain(w);
if (NotMain(w)) { var d = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(w); var cw = d.GetType().GetProperty("window", BF).GetValue(d); cw.GetType().GetProperty("position", BF).SetValue(cw, new Rect(150, 80, W, H)); }
w.Repaint(); return r;
