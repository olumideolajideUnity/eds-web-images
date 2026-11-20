using UnityEditor; using UnityEngine; using System.Linq; using UnityEngine.UIElements;
var w = Resources.FindObjectsOfTypeAll<EditorWindow>().First(x => x.GetType().Name == "MaterialGraphEditWindow");
var gv = (UnityEditor.Experimental.GraphView.GraphView)w.rootVisualElement.Query<VisualElement>().ToList().First(e => e.GetType().Name == "MaterialGraphView");
var nodes = gv.nodes.ToList();
var r = nodes[0].GetPosition();
foreach (var n in nodes) { var p = n.GetPosition(); r = Rect.MinMaxRect(Mathf.Min(r.xMin, p.xMin), Mathf.Min(r.yMin, p.yMin), Mathf.Max(r.xMax, p.xMax), Mathf.Max(r.yMax, p.yMax)); }
// leave room on the right for the Main Preview (~135 pt)
float vw = gv.layout.width - 145, vh = gv.layout.height - 16;
float s = Mathf.Min(vw / r.width, vh / r.height);
var pos = new Vector3(8 - r.xMin * s + (vw - r.width * s) / 2, 8 - r.yMin * s + (vh - r.height * s) / 2, 0);
gv.UpdateViewTransform(pos, new Vector3(s, s, 1));
w.Repaint();
return "nodes=" + nodes.Count + " bounds=" + r + " scale=" + s + " minScale=" + gv.minScale;
