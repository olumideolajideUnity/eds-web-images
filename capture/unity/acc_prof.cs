// Accessibility a11y-3: floating Profiler (W x H pt incl. tab) showing only the KEEP modules (CPU Usage and Rendering;
// Realtime GI cannot be enabled in Unity 7), colour-blind condition CB (0 = Default). RESTORE puts back the module states saved on the first run and CB 0.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
string KEEP = "CPUProfilerModule,RenderingProfilerModule"; float W = 697, H = 420; int CB = 0; bool RESTORE = false; bool HL = false; string SAVE = "C:/tmp/prof_modules.txt";
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var tw = typeof(ProfilerWindow);
var w = Resources.FindObjectsOfTypeAll<ProfilerWindow>().FirstOrDefault(x => !x.docked);
if (w == null) { w = ScriptableObject.CreateInstance<ProfilerWindow>(); w.Show(); }
w.minSize = new Vector2(300, 200);
var mods = ((System.Collections.IEnumerable)tw.GetField("m_AllModules", BF).GetValue(w)).Cast<object>().ToList();
PropertyInfo Act(object m) => m.GetType().GetProperty("active", BF);
if (!System.IO.File.Exists(SAVE)) System.IO.File.WriteAllLines(SAVE, mods.Select(m => m.GetType().Name + "=" + Act(m).GetValue(m)));
var saved = System.IO.File.ReadAllLines(SAVE).Select(l => l.Split('=')).ToDictionary(a => a[0], a => a[1] == "True");
var keep = KEEP.Split(',');
foreach (var m in mods) { var n = m.GetType().Name; var want = RESTORE ? (saved.TryGetValue(n, out var v) && v) : keep.Contains(n); if ((bool)Act(m).GetValue(m) != want) Act(m).SetValue(m, want); }
var uas = typeof(Editor).Assembly.GetType("UnityEditor.Accessibility.UserAccessiblitySettings");
var cbp = uas.GetProperty("colorBlindCondition", BF);
cbp.SetValue(null, System.Enum.ToObject(cbp.PropertyType, RESTORE ? 0 : CB));
tw.GetMethod("SetBottleneckViewVisible", BF).Invoke(w, new object[]{ RESTORE || HL });
var host = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(w);
var cw = host.GetType().GetProperty("window", BF).GetValue(host);
if (!RESTORE) cw.GetType().GetProperty("position", BF).SetValue(cw, new Rect(250, 120, W, H));
var sp = (UnityEngine.UIElements.TwoPaneSplitView)tw.GetProperty("MainSplitView", BF).GetValue(w);
float CHARTS = 330;
if (sp != null && !RESTORE) { var d = sp.fixedPaneIndex == 0 ? CHARTS : H - 46 - CHARTS; sp.fixedPaneInitialDimension = d; sp.fixedPane.style.height = d; }
w.Repaint();
return "cb=" + cbp.GetValue(null) + " names=" + string.Join(",", System.Enum.GetNames(cbp.PropertyType)) + " active=" + string.Join(",", mods.Where(m => (bool)Act(m).GetValue(m)).Select(m => m.GetType().Name.Replace("ProfilerModule","")));
