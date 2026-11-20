using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Collections.Generic;
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
var hits = new List<VisualElement>(); var stack = new Stack<VisualElement>(); stack.Push(sv.rootVisualElement.panel.visualTree);
while (stack.Count > 0) { var e = stack.Pop(); if (e is TextElement te && te.text == "Create Sphere") hits.Add(e); foreach (var c in e.Children()) stack.Push(c); }
string r = "";
foreach (var h in hits) {
  var a = h; while (a.parent != null && a.parent.worldBound.width < 400) a = a.parent;   // climb to the menu container
  r += a.GetType().Name + "." + string.Join(".", a.GetClasses()) + a.worldBound + "; ";
  a.RemoveFromHierarchy();
}
foreach (var o in sv.overlayCanvas.overlays) if (o.id == "unity-spline-inspector") o.rootVisualElement.style.display = DisplayStyle.None;
sv.Repaint();
return hits.Count + " removed: " + r;
