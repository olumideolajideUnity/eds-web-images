// Frame the offline Services page like the original: hide settings tree, splitter and search toolbar; dump visible offline content.
using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection;
float W = 560, H = 360;
var ps = Resources.FindObjectsOfTypeAll<EditorWindow>().First(x => x.GetType().Name == "ProjectSettingsWindow");
ps.position = new Rect(150, 150, W, H);
var root = ps.rootVisualElement;
root.Q<IMGUIContainer>(className: "settings-tree-imgui-container").style.display = DisplayStyle.None;
foreach (var d in root.Query(className: "unity-two-pane-split-view__dragline-anchor").ToList()) d.style.display = DisplayStyle.None;
root.Children().First().style.display = DisplayStyle.None;           // top IMGUI search toolbar
var panel = root.Q(className: "settings-panel");
panel.parent.style.flexGrow = 1; panel.style.width = StyleKeyword.Auto; panel.style.flexGrow = 1;
string Txt(VisualElement e) => e is TextElement t ? t.text : "";
var vis = panel.Query<VisualElement>().ToList().Where(e => e.resolvedStyle.display != DisplayStyle.None && (e is TextElement || e.name.Length > 0))
  .Select(e => e.GetType().Name + "#" + e.name + "." + string.Join(".", e.GetClasses()) + " '" + Txt(e) + "' " + e.worldBound);
ps.Repaint();
return string.Join("\n", vis.Take(60));
