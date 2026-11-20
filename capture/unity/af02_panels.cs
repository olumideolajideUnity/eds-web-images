using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection; using UnityEngine.UIElements;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var w = Resources.FindObjectsOfTypeAll<EditorWindow>().First(x => x.GetType().Name == "MaterialGraphEditWindow");
var gev = w.GetType().GetProperty("graphEditorView", BF)?.GetValue(w) ?? w.GetType().GetField("m_GraphEditorView", BF)?.GetValue(w);
var s = gev.GetType().GetField("m_UserViewSettings", BF).GetValue(gev);
foreach (var n in new[]{"isBlackboardVisible","isInspectorVisible"}) {
  var f = s.GetType().GetField(n, BF); if (f != null) f.SetValue(s, false);
  var p = s.GetType().GetProperty(n, BF); if (p != null && p.CanWrite) p.SetValue(s, false);
}
var pv = s.GetType().GetField("isPreviewVisible", BF); if (pv != null) pv.SetValue(s, true);
var upd = gev.GetType().GetMethod("UpdateSubWindowsVisibility", BF); upd?.Invoke(gev, null);
var gv = w.rootVisualElement.Query<VisualElement>().ToList().First(e => e.GetType().Name == "MaterialGraphView");
gv.GetType().GetMethod("FrameAll").Invoke(gv, null);
w.Repaint();
return "settings: " + string.Join(",", s.GetType().GetFields(BF).Select(f => f.Name + "=" + f.GetValue(s))) + " update=" + (upd != null);
