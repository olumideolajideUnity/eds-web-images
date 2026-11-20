// List displayed overlays on every Scene view (to C:/tmp/ov.txt): view position, overlay id, collapsed, container.
using UnityEditor; using UnityEditor.Overlays; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var sb = new System.Text.StringBuilder();
foreach (var sv in Resources.FindObjectsOfTypeAll<SceneView>()) {
  sb.AppendLine("== " + sv.position + " docked=" + sv.docked);
  var canvas = sv.GetType().GetProperty("overlayCanvas", BF).GetValue(sv);
  foreach (var o in ((System.Collections.IEnumerable)canvas.GetType().GetProperty("overlays", BF).GetValue(canvas)).Cast<Overlay>())
    if (o.displayed) sb.AppendLine(o.id + " collapsed=" + o.collapsed + " cont=" + (o.GetType().GetProperty("container", BF)?.GetValue(o) as UnityEngine.UIElements.VisualElement)?.name); }
System.IO.File.WriteAllText("C:/tmp/ov.txt", sb.ToString()); return "ok";
