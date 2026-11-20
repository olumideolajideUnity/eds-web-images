using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Collections.Generic;
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
RenderSettings.skybox = null;
var vo = sv.overlayCanvas.overlays.First(o => o.displayName == "View Options");
var all = new List<VisualElement>(); var st = new Stack<VisualElement>(); st.Push(vo.rootVisualElement);
while (st.Count > 0) { var e = st.Pop(); all.Add(e); foreach (var c in e.Children()) st.Push(c); }
var fxb = all.Where(e => (e.tooltip ?? "").ToLower().Contains("effect") || e.name.ToLower().Contains("fx")).OrderBy(e => e.worldBound.width).LastOrDefault();
var b = fxb == null ? Rect.zero : fxb.worldBound;
System.IO.File.WriteAllText("OUT", "{\"btn\":[" + b.x + "," + b.y + "," + b.width + "," + b.height + "]}");
sv.Repaint();
return "btn=" + (fxb == null ? "none" : fxb.GetType().Name + "#" + fxb.name + " " + b) + " dirty=" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().isDirty;
