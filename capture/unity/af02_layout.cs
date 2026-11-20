using UnityEditor; using UnityEngine; using System.Linq; using UnityEngine.UIElements;
var w = Resources.FindObjectsOfTypeAll<EditorWindow>().First(x => x.GetType().Name == "MaterialGraphEditWindow");
var gv = w.rootVisualElement.Query<VisualElement>().ToList().First(e => e.GetType().Name == "MaterialGraphView");
gv.GetType().GetMethod("FrameAll").Invoke(gv, null);
var prev = w.rootVisualElement.Query<VisualElement>().ToList().FirstOrDefault(e => e.GetType().Name.Contains("MasterPreview"));
string r = "preview=" + (prev?.GetType().Name ?? "none");
if (prev != null) {
  var host = prev.parent != null && prev.parent.GetType().Name.Contains("Window") ? prev.parent : prev;   // floating window wrapper if any
  var parentW = host.parent.layout.width;
  host.style.left = parentW - host.layout.width - 8; host.style.top = 8;
  r += " host=" + host.GetType().Name + " " + host.layout;
}
w.Repaint();
return r + " gv=" + gv.layout;
