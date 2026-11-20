// Toolbar page, 'don't' example (the original is constructed too): a real UI Toolkit Toolbar of ToolbarButtons Audio / Control / Signal
// and a kebab ToolbarMenu, to set into the Timeline track toolbar. Host 300x40pt; toolbar rect (pt) to C:/tmp/tl_strip.json.
using UnityEditor; using UnityEditor.UIElements; using UnityEngine; using UnityEngine.UIElements; using System.Linq;
var hostT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSPopupHost")).First(x => x != null);
foreach (var o in Resources.FindObjectsOfTypeAll(hostT).Cast<EditorWindow>().ToList()) o.Close();
var w = (EditorWindow)ScriptableObject.CreateInstance(hostT); w.titleContent = new GUIContent("EFDSPopupHost"); w.ShowUtility(); w.position = new Rect(200, 150, 300, 40);
var root = w.rootVisualElement; root.Clear();
var tb = new Toolbar(); tb.style.position = Position.Absolute; tb.style.left = 0; tb.style.top = 10;
foreach (var s in new[]{ "Audio", "Control", "Signal" }) tb.Add(new ToolbarButton { text = s });
var more = new ToolbarMenu(); more.AddToClassList("unity-toolbar-menu--popup"); var ic = new Image { image = EditorGUIUtility.IconContent("_Menu").image }; ic.style.width = 16; ic.style.height = 16; more.Insert(0, ic); tb.Add(more);
root.Add(tb);
root.schedule.Execute(() => { var b = tb.worldBound; float right = tb.Children().Max(c => c.worldBound.xMax);
  System.IO.File.WriteAllText("C:/tmp/tl_strip.json", "[" + b.x + "," + b.y + "," + right + "," + b.height + "]"); }).StartingIn(500);
w.Repaint(); return "ok";
