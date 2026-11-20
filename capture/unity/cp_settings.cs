// Color palette > Text Colors Base 2: floating Project Settings (W x H pt incl. tab) on the Editor page.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
float W = 604, H = 346;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var t = typeof(Editor).Assembly.GetType("UnityEditor.ProjectSettingsWindow");
var w = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().FirstOrDefault(x => !x.docked);
if (w == null) { w = (EditorWindow)ScriptableObject.CreateInstance(t); w.Show(); }
var host = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(w);
var cw = host.GetType().GetProperty("window", BF).GetValue(host);
cw.GetType().GetProperty("position", BF).SetValue(cw, new Rect(250, 120, W, H));
MethodInfo sel = null; for (var tt = t; tt != null && sel == null; tt = tt.BaseType) sel = tt.GetMethods(BF).FirstOrDefault(m => m.Name == "SelectProviderByName");
if (sel != null) { var ps = sel.GetParameters(); var a = new object[ps.Length]; a[0] = "Project/Editor"; for (int i = 1; i < ps.Length; i++) a[i] = ps[i].HasDefaultValue ? ps[i].DefaultValue : (ps[i].ParameterType == typeof(bool) ? (object)true : null); sel.Invoke(w, a); }
w.Repaint();
return "sel=" + (sel != null) + " " + w.titleContent.text;
