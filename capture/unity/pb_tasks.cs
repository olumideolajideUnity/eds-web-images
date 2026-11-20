// Components > Progress bar (bg-tasks-win): real background tasks reported through UnityEditor.Progress (the API that feeds the
// Background Tasks window), in the original's states, then the Background Tasks window floated at W x H pt.
using UnityEditor; using UnityEngine; using System.Linq;
float W = 400, H = 280;
foreach (var p in Progress.EnumerateItems().ToList()) { try { Progress.Remove(p.id); } catch {} }
int Done(string n, string d, Progress.Status st, float v) { var id = Progress.Start(n, d, Progress.Options.Sticky); Progress.Report(id, v, d); Progress.Finish(id, st); return id; }
var a = Progress.Start("Long task", "This task will become unresponsive"); Progress.Report(a, 0.25f, "This task will become unresponsive");
Done("Cancelled task", "Cancelled", Progress.Status.Canceled, 0.4f);
Done("Success task", "Done", Progress.Status.Succeeded, 1f);
var s1 = Progress.Start("Default Steps No time shown", "Doing random stuff", Progress.Options.Sticky); Progress.Report(s1, 5, 5, "Doing random stuff"); Progress.Finish(s1, Progress.Status.Succeeded);
var s2 = Progress.Start("Default Steps Task ETA", "Doing random stuff", Progress.Options.Sticky); Progress.Report(s2, 88, 88, "Doing random stuff"); Progress.Finish(s2, Progress.Status.Succeeded);
var f = Progress.Start("Failed task", "Something went wrong", Progress.Options.Sticky); Progress.Report(f, 0.6f, "Something went wrong"); Progress.Finish(f, Progress.Status.Failed);
var wt = System.AppDomain.CurrentDomain.GetAssemblies().Select(x => x.GetType("UnityEditor.ProgressWindow")).First(x => x != null);
var w = Resources.FindObjectsOfTypeAll(wt).Cast<EditorWindow>().FirstOrDefault() ?? (EditorWindow)ScriptableObject.CreateInstance(wt);
w.Show(); var host = typeof(EditorWindow).GetField("m_Parent", System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic).GetValue(w);
var cw = host.GetType().GetProperty("window").GetValue(host); cw.GetType().GetProperty("position").SetValue(cw, new Rect(300, 150, W, H));
w.Repaint(); return "items=" + Progress.EnumerateItems().Count() + " title=" + w.titleContent.text;
