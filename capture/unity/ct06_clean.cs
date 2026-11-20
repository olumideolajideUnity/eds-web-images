using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection; using System.Collections.Generic;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
int removed = 0;
var stack = new Stack<VisualElement>(); stack.Push(sv.rootVisualElement);
while (stack.Count > 0) { var e = stack.Pop();
  var f = e.GetType().GetField("m_ToolVariantDropdown", BF);
  if (f != null && f.GetValue(e) is VisualElement dd) { dd.RemoveFromHierarchy(); f.SetValue(e, null); removed++; }
  foreach (var c in e.Children()) stack.Push(c); }
foreach (var o in sv.overlayCanvas.overlays) if (o.id == "unity-spline-inspector") o.displayed = false;
var hs = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("UnityEditor.Splines.SplineHandleSettings")).First(t => t != null);
string props = string.Join(",", hs.GetProperties(BF).Where(p => p.PropertyType == typeof(bool)).Select(p => p.Name));
void Set(string n, bool v) { var p = hs.GetProperty(n, BF); if (p != null && p.CanWrite) p.SetValue(null, v); }
Set("FlowDirectionEnabled", true); Set("ShowAllTangents", true); Set("ShowKnotIndices", true); Set("ShowMesh", false);
sv.Repaint();
return "removedDropdowns=" + removed + " props=" + props;
