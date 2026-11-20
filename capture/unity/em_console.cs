// Errors & messaging: Console with the original's messages, logged for real; timestamps on, collapse off, first row selected.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
float W = 670, H = 230;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var asm = typeof(Editor).Assembly;
var le = asm.GetType("UnityEditor.LogEntries");
var cwT = asm.GetType("UnityEditor.ConsoleWindow");
var flagsT = cwT.GetNestedType("ConsoleFlags", BF);
void Flag(string n, bool v) => le.GetMethod("SetConsoleFlag", BF).Invoke(null, new object[]{ (int)System.Enum.Parse(flagsT, n), v });
Flag("ShowTimestamp", true); Flag("Collapse", false); Flag("LogLevelLog", true); Flag("LogLevelWarning", true); Flag("LogLevelError", true);
le.GetMethod("Clear", BF).Invoke(null, null);
System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSMessageSpecimens")).First(x => x != null).GetMethod("LogConsoleExample").Invoke(null, null);
var w = Resources.FindObjectsOfTypeAll(cwT).Cast<EditorWindow>().FirstOrDefault(x => !x.docked);
if (w == null) { w = (EditorWindow)ScriptableObject.CreateInstance(cwT); w.Show(); }
w.position = new Rect(200, 200, W, H);
var spl = cwT.GetField("spl", BF).GetValue(w);
// collapse the message detail pane (the original's Console shows only the list), select the first row, scroll to top
var sT = spl.GetType();
sT.GetField("relativeSizes", BF).SetValue(spl, new float[]{ 0.72f, 0.28f });
var lv = cwT.GetField("m_ListView", BF).GetValue(w);
lv.GetType().GetField("row", BF)?.SetValue(lv, 0);
lv.GetType().GetField("selectedItems", BF)?.SetValue(lv, new bool[]{ true, false, false });
var entT = asm.GetType("UnityEditor.LogEntry"); var ent = System.Activator.CreateInstance(entT);
le.GetMethod("StartGettingEntries", BF)?.Invoke(null, null);
le.GetMethod("GetEntryInternal", BF).Invoke(null, new object[]{ 0, ent });
le.GetMethod("EndGettingEntries", BF)?.Invoke(null, null);
cwT.GetMethod("SetActiveEntry", BF).Invoke(w, new object[]{ ent });
var sp = lv.GetType().GetField("scrollPos", BF); if (sp != null) sp.SetValue(lv, Vector2.zero);
var members = "lv=" + string.Join(",", lv.GetType().GetFields(BF).Select(f => f.Name));
w.Repaint();
return members;
