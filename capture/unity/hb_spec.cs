// Components > Help box: real UI Toolkit HelpBoxes (Unity 7's built-in buttonText / linkText) in a plain host window, 800 x 300pt, dark.
// WHICH: actions | types. Rects (pt) to C:/tmp/hb_spec.json.
using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Collections.Generic;
string WHICH = "actions";
var hostT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSPopupHost")).First(x => x != null);
foreach (var o in Resources.FindObjectsOfTypeAll(hostT).Cast<EditorWindow>().ToList()) o.Close();
var w = (EditorWindow)ScriptableObject.CreateInstance(hostT); w.titleContent = new GUIContent("EFDSPopupHost"); w.ShowUtility(); w.position = new Rect(200, 150, 800, 300);
var root = w.rootVisualElement; root.Clear();
var named = new Dictionary<string, VisualElement>();
HelpBox HB(string key, string text, HelpBoxMessageType t, float x, float y, float wd, string button = null, string link = null) {
  var h = new HelpBox(text, t); if (button != null) h.buttonText = button; if (link != null) { h.linkText = link; h.linkHref = "https://docs.unity3d.com"; }
  h.style.position = Position.Absolute; h.style.left = x; h.style.top = y; h.style.width = wd; root.Add(h); named[key] = h; return h; }
if (WHICH == "actions") {
  HB("a", "This is an info (log) message without any actions.", HelpBoxMessageType.Info, 90, 32, 320);
  HB("b", "This is an info (log) message with a link added using rich text tag. <a href=\"https://docs.unity3d.com\">Read more...</a>", HelpBoxMessageType.Info, 190, 80, 320);
  HB("c", "This is an info (log) message with a call to action button.", HelpBoxMessageType.Info, 290, 137, 320, "Action");
  HB("d", "This is an info (log) message with a call to action button and a documentation link.", HelpBoxMessageType.Info, 390, 209, 320, "Action", "Read more...");
} else {
  HB("err", "This is a critical error message.", HelpBoxMessageType.Error, 80, 50, 300, "Action", "Hyperlink");
  HB("warn", "This is a warning message.", HelpBoxMessageType.Warning, 250, 125, 300, "Action", "Hyperlink");
  HB("info", "This is an info (log) message.", HelpBoxMessageType.Info, 420, 198, 300, "Action", "Hyperlink");
}
root.schedule.Execute(() => System.IO.File.WriteAllText("C:/tmp/hb_spec.json", "{" + string.Join(",", named.Select(kv => "\"" + kv.Key + "\":[" + kv.Value.worldBound.x + "," + kv.Value.worldBound.y + "," + kv.Value.worldBound.width + "," + kv.Value.worldBound.height + "]")) + "}")).StartingIn(400);
w.Repaint(); return WHICH;
