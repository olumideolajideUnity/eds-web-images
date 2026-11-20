// Typography (a11y-11): floating Profiler window (W x H pt incl. tab). STAGE 0 opens it and starts recording the Editor
// (Play mode on the open scene unless PLAYEDITOR); STAGE 1 stops recording (still playing), STAGE 2 also exits Play mode; both stop recording and selects frame SEL frames before the newest.
using UnityEditor; using UnityEditorInternal; using UnityEngine; using System.Linq; using System.Reflection;
int STAGE = 0; bool PLAYEDITOR = false; float W = 630, H = 480; int SEL = 60;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var w = Resources.FindObjectsOfTypeAll<ProfilerWindow>().FirstOrDefault(x => !x.docked);
if (w == null) { w = ScriptableObject.CreateInstance<ProfilerWindow>(); w.Show(); }
w.minSize = new Vector2(300, 200);
var tw = typeof(ProfilerWindow);
bool SHOWHL = false; bool SHOTS = false;   // Highlights view and Screenshots module (restored with SHOWHL/SHOTS = true afterwards)
tw.GetMethod("SetBottleneckViewVisible", BF).Invoke(w, new object[]{ SHOWHL });
foreach (var m in (System.Collections.IEnumerable)tw.GetField("m_AllModules", BF).GetValue(w))
  if (m.GetType().Name == "ScreenshotsProfilerModule") m.GetType().GetProperty("active", BF).SetValue(m, SHOTS);
var host = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(w);
var cw = host.GetType().GetProperty("window", BF).GetValue(host);
cw.GetType().GetProperty("position", BF).SetValue(cw, new Rect(250, 120, W, H));
var sp = (UnityEngine.UIElements.TwoPaneSplitView)tw.GetProperty("MainSplitView", BF).GetValue(w);
float CHARTS = 330;   // pt for the charts pane (the original shows only charts in its crop)
if (sp != null) { var fixedIsCharts = sp.fixedPaneIndex == 0; var d = fixedIsCharts ? CHARTS : H - 46 - CHARTS; sp.fixedPaneInitialDimension = d; sp.fixedPane.style.height = d; }
if (STAGE == 0) { ProfilerDriver.ClearAllFrames(); ProfilerDriver.profileEditor = PLAYEDITOR; ProfilerDriver.enabled = true; if (!PLAYEDITOR) EditorApplication.isPlaying = true; w.Repaint(); return "recording"; }
ProfilerDriver.enabled = false; if (STAGE == 2) EditorApplication.isPlaying = false;
var last = ProfilerDriver.lastFrameIndex;
w.selectedFrameIndex = last - SEL;
w.Repaint();
return "frames " + ProfilerDriver.firstFrameIndex + ".." + last + " sel=" + w.selectedFrameIndex;
