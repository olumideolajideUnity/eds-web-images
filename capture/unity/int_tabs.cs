// Interactions > checked.png tab rows: two real dockable windows (EFDS.EFDSMessageSpecimens) docked together in a floating
// window (W x H pt), titled FIRST (selected) and "Idle tab"; FOCUSED gives the dock area keyboard focus (else the main window has it).
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
float W = 240, H = 60; string FIRST = "Checked tab"; bool FOCUSED = false;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var t = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSMessageSpecimens")).First(x => x != null);
foreach (var o in Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().ToList()) o.Close();
var a = (EditorWindow)ScriptableObject.CreateInstance(t); a.titleContent = new GUIContent(FIRST); t.GetField("mode").SetValue(a, 5); a.Show();
var ht = System.AppDomain.CurrentDomain.GetAssemblies().Select(x => x.GetType("EFDS.EFDSPopupHost")).First(x => x != null);   // different type, so the capture helper only matches the first tab
foreach (var o in Resources.FindObjectsOfTypeAll(ht).Cast<EditorWindow>().ToList()) o.Close();
var b = (EditorWindow)ScriptableObject.CreateInstance(ht); b.titleContent = new GUIContent("Idle tab");
var dock = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(a);
dock.GetType().GetMethods(BF).First(m => m.Name == "AddTab" && m.GetParameters().Length == 2).Invoke(dock, new object[]{ b, false });
dock.GetType().GetProperty("selected", BF)?.SetValue(dock, 0);
var cw = dock.GetType().GetProperty("window", BF).GetValue(dock);
cw.GetType().GetProperty("position", BF).SetValue(cw, new Rect(300, 300, W, H));
a.ShowTab();
if (FOCUSED) a.Focus(); else { var sv = SceneView.lastActiveSceneView; if (sv) sv.Focus(); }
a.Repaint(); b.Repaint();
return "focused=" + (EditorWindow.focusedWindow == a) + " now=" + EditorWindow.focusedWindow;
