// Color palette > Text Colors Base 1: Console with the original's three messages, logged for real from EFDS.Example
// (BadFile.cs) so the stack trace has a file link. STAGE 0 logs; STAGE 1 sizes the window and selects the first row.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
int STAGE = 0; float W = 670, H = 322, LIST = 0.52f;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var asm = typeof(Editor).Assembly;
var le = asm.GetType("UnityEditor.LogEntries");
var cwT = asm.GetType("UnityEditor.ConsoleWindow");
if (STAGE == 0) {
  var flagsT = cwT.GetNestedType("ConsoleFlags", BF);
  void Flag(string n, bool v) => le.GetMethod("SetConsoleFlag", BF).Invoke(null, new object[]{ (int)System.Enum.Parse(flagsT, n), v });
  Flag("ShowTimestamp", true); Flag("Collapse", false); Flag("LogLevelLog", true); Flag("LogLevelWarning", true); Flag("LogLevelError", true);
  le.GetMethod("Clear", BF).Invoke(null, null);
  System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.Example")).First(x => x != null).GetMethod("Log").Invoke(null, null);
  return "logged";
}
var w = Resources.FindObjectsOfTypeAll(cwT).Cast<EditorWindow>().FirstOrDefault(x => !x.docked);
if (w == null) { w = (EditorWindow)ScriptableObject.CreateInstance(cwT); w.Show(); }
var host = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(w);
var cw = host.GetType().GetProperty("window", BF).GetValue(host);
cw.GetType().GetProperty("position", BF).SetValue(cw, new Rect(200, 150, W, H));
var spl = cwT.GetField("spl", BF).GetValue(w);
spl.GetType().GetField("relativeSizes", BF).SetValue(spl, new float[]{ LIST, 1 - LIST });
var tot = System.Convert.ToSingle(spl.GetType().GetField("lastTotalSize", BF).GetValue(spl));
spl.GetType().GetField("realSizes", BF).SetValue(spl, new float[]{ LIST * tot, (1 - LIST) * tot });
var lv = cwT.GetField("m_ListView", BF).GetValue(w);
lv.GetType().GetField("row", BF)?.SetValue(lv, 0);
lv.GetType().GetField("selectedItems", BF)?.SetValue(lv, new bool[]{ true, false, false });
var entT = asm.GetType("UnityEditor.LogEntry"); var ent = System.Activator.CreateInstance(entT);
le.GetMethod("StartGettingEntries", BF)?.Invoke(null, null);
le.GetMethod("GetEntryInternal", BF).Invoke(null, new object[]{ 0, ent });
le.GetMethod("EndGettingEntries", BF)?.Invoke(null, null);
cwT.GetMethod("SetActiveEntry", BF).Invoke(w, new object[]{ ent });
var sp = lv.GetType().GetField("scrollPos", BF); if (sp != null) sp.SetValue(lv, Vector2.zero);
w.Repaint();
return "count=" + le.GetMethod("GetCount", BF).Invoke(null, null) + " msg=" + entT.GetField("message").GetValue(ent).ToString().Replace("\n", " | ").Replace("\"", "'");
