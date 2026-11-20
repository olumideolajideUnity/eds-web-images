// Remove the demo background tasks (cancel the running one) and close the Background Tasks window.
using UnityEditor; using UnityEngine; using System.Linq;
int n = 0; foreach (var p in Progress.EnumerateItems().ToList()) { try { if (p.running) Progress.Finish(p.id, Progress.Status.Canceled); Progress.Remove(p.id); n++; } catch {} }
var wt = System.AppDomain.CurrentDomain.GetAssemblies().Select(x => x.GetType("UnityEditor.ProgressWindow")).First(x => x != null);
foreach (var w in Resources.FindObjectsOfTypeAll(wt).Cast<EditorWindow>().ToList()) w.Close();
return "removed " + n + " left=" + Progress.EnumerateItems().Count();
