// Remove any open (or stale) ProBuilder shape-variant menus from the floating Scene view's panel.
using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Collections.Generic;
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
var hits = new List<VisualElement>(); var st = new Stack<VisualElement>(); st.Push(sv.rootVisualElement.panel.visualTree);
while (st.Count > 0) { var e = st.Pop(); if (e is TextElement te && te.text == "Create Sphere") hits.Add(e); foreach (var c in e.Children()) st.Push(c); }
foreach (var h in hits) { var a = h; while (a.parent != null && a.parent.worldBound.width < 400) a = a.parent; a.RemoveFromHierarchy(); }
sv.Repaint(); return "removed " + hits.Count;
