// Toggle > style labels: floating Preferences window (W x H pt) on the General page.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
float W = 620, H = 420;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var w = SettingsService.OpenUserPreferences("Preferences/General");
var host = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(w); var cw = host.GetType().GetProperty("window", BF).GetValue(host);
cw.GetType().GetProperty("position", BF).SetValue(cw, new Rect(250, 120, W, H));
MethodInfo sel = null; for (var tt = w.GetType(); tt != null && sel == null; tt = tt.BaseType) sel = tt.GetMethods(BF).FirstOrDefault(m => m.Name == "SelectProviderByName");
if (sel != null) { var ps = sel.GetParameters(); var a = new object[ps.Length]; a[0] = "Preferences/General"; for (int i = 1; i < ps.Length; i++) a[i] = ps[i].HasDefaultValue ? ps[i].DefaultValue : (ps[i].ParameterType == typeof(bool) ? (object)true : null); sel.Invoke(w, a); }
w.Repaint();
return w.GetType().Name + " " + w.position;
