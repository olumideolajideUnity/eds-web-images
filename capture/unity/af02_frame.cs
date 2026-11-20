using UnityEditor; using UnityEngine; using System.Linq; using UnityEngine.UIElements;
var w = Resources.FindObjectsOfTypeAll<EditorWindow>().First(x => x.GetType().Name == "MaterialGraphEditWindow");
w.position = new Rect(300, 120, 700, 437);
w.Focus();
var gv = w.rootVisualElement.Query<VisualElement>().ToList().FirstOrDefault(e => e.GetType().Name.Contains("MaterialGraphView") || e.GetType().BaseType?.Name == "GraphView");
string res = "graphView=" + (gv?.GetType().Name ?? "none");
if (gv != null) { var m = gv.GetType().GetMethod("FrameAll"); m?.Invoke(gv, null); res += " framed=" + (m != null); }
w.Repaint();
return res + " pos=" + w.position;
