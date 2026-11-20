// Unity 7000.0.0a20 bug workaround (capture only, not saved): Package Manager's top toolbar container collapses to 0 height.
using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Collections.Generic;
var w = Resources.FindObjectsOfTypeAll<EditorWindow>().First(x => x.GetType().Name == "PackageManagerWindow");
var st = new Stack<VisualElement>(); st.Push(w.rootVisualElement); VisualElement tb = null;
while (st.Count > 0) { var e = st.Pop(); if (e.name == "topMenuToolbar") { tb = e; break; } foreach (var c in e.Children()) st.Push(c); }
tb.style.minHeight = 23.2f; tb.style.flexShrink = 0; if (tb.parent != null) { tb.parent.style.minHeight = 23.2f; tb.parent.style.flexShrink = 0; }
w.Repaint();
return "fixed " + tb.layout;
