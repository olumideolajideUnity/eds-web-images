// Close temporary capture windows: EFDS host windows, the Gizmos window and any ColorPicker.
using UnityEditor; using UnityEngine; using System.Linq;
int n = 0;
foreach (var tn in new[]{ "EFDS.EFDSPopupHost", "EFDS.EFDSMessageSpecimens" }) { var t = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType(tn)).FirstOrDefault(x => x != null); if (t != null) foreach (var o in Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().ToList()) { o.Close(); n++; } }
foreach (var tn in new[]{ "UnityEditor.AnnotationWindow" }) { var at = typeof(Editor).Assembly.GetType(tn); foreach (var o in Resources.FindObjectsOfTypeAll(at).Cast<EditorWindow>().ToList()) { o.Close(); n++; } }
var cpT = typeof(Editor).Assembly.GetType("UnityEditor.ColorPicker"); foreach (var o in Resources.FindObjectsOfTypeAll(cpT).Cast<EditorWindow>().ToList()) { o.Close(); n++; }
return "closed " + n;
