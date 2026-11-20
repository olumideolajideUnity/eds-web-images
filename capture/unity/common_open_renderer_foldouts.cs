using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection; using System.Collections.Generic;
var log = new List<string>();
var BF = BindingFlags.NonPublic|BindingFlags.Public|BindingFlags.Instance;
void Walk(object o, int depth, string path) {
  if (o == null || depth > 2) return;
  foreach (var f in o.GetType().GetFields(BF)) {
    var v = f.GetValue(o); if (v == null) continue;
    var tn = f.FieldType.Name;
    if (tn == "SavedBool") {
      var n = f.Name.ToLower();
      if ((n.Contains("lighting") && !n.Contains("lightmap")) || n.Contains("probe")) {
        f.FieldType.GetProperty("value").SetValue(v, true); log.Add(path + "." + f.Name + "=true");
      }
    } else if (tn == "RendererLightingSettings" || tn.Contains("Probe")) Walk(v, depth + 1, path + "." + f.Name);
  }
}
var inspType = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
var tracker = (ActiveEditorTracker)inspType.GetProperty("tracker", BF).GetValue(insp);
foreach (var ed in tracker.activeEditors) Walk(ed, 0, ed.GetType().Name);
insp.Repaint();
return string.Join("\n", log);
