// Accessibility (a11y-1/2): Console showing real log entries whose text is coloured with rich text in Unity 7's
// warning-text / error-text tokens. STAGE 0 clears and logs SET on the next editor update (so the entries are real Console
// entries); STAGE 1 sizes the window (W x H pt incl. tab), clears the selection and shows only the list.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
int STAGE = 0; string SET = "1Y"; float W = 640, H = 320; bool TS = false;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var asm = typeof(Editor).Assembly; var le = asm.GetType("UnityEditor.LogEntries"); var cwT = asm.GetType("UnityEditor.ConsoleWindow");
string WARN = EditorGUIUtility.isProSkin ? "#F4BC02" : "#333308", ERR = EditorGUIUtility.isProSkin ? "#D32222" : "#5A0000";   // live tokens per skin (unity/cp_tokens.cs)
string C(string col, string s) => "<color=" + col + ">" + s + "</color>";
if (STAGE == 0) {
  var flagsT = cwT.GetNestedType("ConsoleFlags", BF);
  void Flag(string n, bool v) => le.GetMethod("SetConsoleFlag", BF).Invoke(null, new object[]{ (int)System.Enum.Parse(flagsT, n), v });
  Flag("ShowTimestamp", TS); Flag("Collapse", false); Flag("LogLevelLog", true); Flag("LogLevelWarning", true); Flag("LogLevelError", true);
  le.GetMethod("Clear", BF).Invoke(null, null);
  const string caution = "Caution message, something is wrong but it's not that bad", crit = "Warning message, something has gone critically wrong!";
  EditorApplication.CallbackFunction cb = null;
  cb = () => {
    EditorApplication.update -= cb;
    if (SET == "1Y") Debug.LogError(C(ERR, "Error!\nSomething has gone critically wrong!"));
    else if (SET == "1N") Debug.LogError(C(ERR, "Success!\nYour operation has succeeded."));
    else {   // no stack trace: Unity 7 trunk would show an internal interop frame as each row's second line
      Debug.LogFormat(LogType.Warning, LogOption.NoStacktrace, null, "{0}", C(WARN, caution));
      Debug.LogFormat(LogType.Warning, LogOption.NoStacktrace, null, "{0}", C(WARN, caution));
      Debug.LogFormat(LogType.Error, LogOption.NoStacktrace, null, "{0}", C(ERR, crit)); }
  };
  EditorApplication.update += cb;
  return "logging " + SET;
}
var w = Resources.FindObjectsOfTypeAll(cwT).Cast<EditorWindow>().FirstOrDefault(x => !x.docked);
if (w == null) { w = (EditorWindow)ScriptableObject.CreateInstance(cwT); w.Show(); }
var host = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(w);
var cw = host.GetType().GetProperty("window", BF).GetValue(host);
cw.GetType().GetProperty("position", BF).SetValue(cw, new Rect(200, 150, W, H));
var spl = cwT.GetField("spl", BF).GetValue(w);
var tot = System.Convert.ToSingle(spl.GetType().GetField("lastTotalSize", BF).GetValue(spl));
spl.GetType().GetField("relativeSizes", BF).SetValue(spl, new float[]{ 0.85f, 0.15f });
spl.GetType().GetField("realSizes", BF).SetValue(spl, new float[]{ 0.85f * tot, 0.15f * tot });
var lv = cwT.GetField("m_ListView", BF).GetValue(w);
lv.GetType().GetField("row", BF)?.SetValue(lv, -1);
lv.GetType().GetField("selectedItems", BF)?.SetValue(lv, new bool[]{ false, false, false });
var sp = lv.GetType().GetField("scrollPos", BF); if (sp != null) sp.SetValue(lv, Vector2.zero);
cwT.GetField("m_ActiveText", BF)?.SetValue(w, "");
w.Repaint();
return "count=" + le.GetMethod("GetCount", BF).Invoke(null, null);
