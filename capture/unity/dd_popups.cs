// Components > Dropdown: real Editor popup menus. WHICH = gizmos (UnityEditor.AnnotationWindow via its own ShowAtPosition)
// or camera (UnityEditor.SceneViewCameraWindow PopupWindowContent, hosted in EFDS.EFDSPopupHost so it stays open).
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
string WHICH = "gizmos"; float H = 220;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var asm = typeof(Editor).Assembly;
if (WHICH == "gizmos") {      // AnnotationWindow set up exactly as its Init does, but shown as a utility window so it stays open
  var t = asm.GetType("UnityEditor.AnnotationWindow");
  foreach (var o in Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().ToList()) o.Close();
  var w = (EditorWindow)ScriptableObject.CreateInstance(t);
  t.GetField("m_MonoScriptIconsChanged", BF).SetValue(w, new System.Collections.Generic.List<MonoScript>());
  t.GetField("m_SyncWithState", BF).SetValue(w, true); t.GetField("m_IsGameView", BF).SetValue(w, false);
  t.GetMethod("SyncToState", BF).Invoke(w, null);
  float frame = System.Convert.ToSingle(t.GetField("k_FrameWidth", BF).GetValue(null)), width = System.Convert.ToSingle(t.GetField("k_WindowWidth", BF).GetValue(null));
  float top = System.Convert.ToSingle(t.GetMethod("GetTopSectionHeight", BF).Invoke(w, null));
  float list = System.Convert.ToSingle(t.GetMethod("DrawNormalList", BF).Invoke(w, new object[]{ false, 100f, 0f, 10000f }));
  float hgt = Mathf.Min(2 * frame + top + list, 900);
  w.ShowUtility(); w.position = new Rect(600, 150, width, Mathf.Min(hgt, H));
  return "gizmos " + w.position + " fullH=" + hgt;
} else {
  var sv = SceneView.lastActiveSceneView;
  var ct = asm.GetType("UnityEditor.SceneViewCameraWindow");
  var content = (PopupWindowContent)System.Activator.CreateInstance(ct, BF, null, new object[]{ sv }, null);
  var hostT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSPopupHost")).First(x => x != null);
  var host = (EditorWindow)hostT.GetMethod("Open").Invoke(null, new object[]{ content });
  return "camera " + host.position + " size=" + content.GetWindowSize();
}
