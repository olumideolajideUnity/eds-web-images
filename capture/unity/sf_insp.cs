// Components > Search field: the real Unity 7 condensed Inspector search (Unity.UIToolkit.Editor.InspectorSearchField) in a plain host
// window (400x300pt), collapsed (top) and active/expanded (bottom). Rects (pt) to C:/tmp/sf_insp.json.
using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection; using System.Collections.Generic;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var hostT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSPopupHost")).First(x => x != null);
foreach (var o in Resources.FindObjectsOfTypeAll(hostT).Cast<EditorWindow>().ToList()) o.Close();
var w = (EditorWindow)ScriptableObject.CreateInstance(hostT); w.titleContent = new GUIContent("EFDSPopupHost"); w.ShowUtility(); w.position = new Rect(200, 150, 400, 300);
var root = w.rootVisualElement; root.Clear();
var t = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("Unity.UIToolkit.Editor.InspectorSearchField")).First(x => x != null);
var named = new Dictionary<string, VisualElement>();
foreach (var (k, y, act) in new[]{ ("collapsed", 20f, false), ("expanded", 150f, true) }) {
  var f = (VisualElement)System.Activator.CreateInstance(t, true); f.style.position = Position.Absolute; f.style.left = 20; f.style.top = y; f.style.width = 360;
  root.Add(f); named[k] = f; if (act) t.GetProperty("active", BF).SetValue(f, true); }
root.schedule.Execute(() => {
  var sb = new System.Text.StringBuilder();
  void Walk(VisualElement e, int d) { sb.AppendLine(new string(' ', d) + e.GetType().Name + " #" + e.name + " ." + string.Join(".", e.GetClasses()) + " " + e.worldBound + " disp=" + e.resolvedStyle.display); foreach (var c in e.Children()) Walk(c, d + 1); }
  foreach (var kv in named) { sb.AppendLine("== " + kv.Key); Walk(kv.Value, 0); }
  System.IO.File.WriteAllText("C:/tmp/sf_insp_tree.txt", sb.ToString());
}).StartingIn(600);
w.Repaint(); return "ok";
