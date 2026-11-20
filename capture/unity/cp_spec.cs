// Colour-palette control specimens with real UI Toolkit controls in a plain utility host (EFDS.EFDSPopupHost, no content), 420x100pt.
using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using UnityEditor.UIElements; using System.Linq;
string WHICH = "toggles";
var hostT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSPopupHost")).First(x => x != null);
foreach (var o in Resources.FindObjectsOfTypeAll(hostT).Cast<EditorWindow>().ToList()) o.Close();
var w = (EditorWindow)ScriptableObject.CreateInstance(hostT); w.titleContent = new GUIContent("EFDSPopupHost"); w.ShowUtility(); w.position = new Rect(400, 300, 420, 100);
var root = w.rootVisualElement; root.Clear();
root.style.alignItems = Align.Center; root.style.justifyContent = Justify.Center; root.style.flexDirection = FlexDirection.Row;
if (WHICH == "toggles") {
  var lab = new Label("Font Style"); lab.style.marginRight = 50; root.Add(lab);
  var g = new ToggleButtonGroup { allowEmptySelection = true, isMultipleSelection = true };
  foreach (var s in new[]{ "B", "I", "U", "S" }) g.Add(new Button { text = s });
  root.Add(g); var v = ToggleButtonGroupState.CreateFromOptions(new System.Collections.Generic.List<bool>{ true, false, true, false }); g.value = v;
} else if (WHICH == "dropdown") { var dd = new DropdownField(new System.Collections.Generic.List<string>{ "Dropdown" }, 0); dd.style.width = 160; root.Add(dd); }
else if (WHICH == "number") { var f = new IntegerField { value = 0 }; f.style.width = 50; root.Add(f); }
else if (WHICH.StartsWith("accent")) {   // Accent Borders: one control, panel-scaled so it renders crisply at the original's ~3.7 px/pt
  root.style.backgroundColor = EditorGUIUtility.isProSkin ? (Color)new Color32(56, 56, 56, 255) : (Color)new Color32(226, 226, 226, 255);   // the diagram's page
  VisualElement c = WHICH == "accent_tf" ? (VisualElement)new TextField { value = "Text Field" } : new DropdownField(new System.Collections.Generic.List<string>{ "Dropdown" }, 0);
  c.style.width = WHICH == "accent_tf" ? 99.5f : 106.5f; c.style.scale = new Scale(new Vector3(1.48f, 1.48f, 1)); root.Add(c);
}
else { var s = new Slider(0, 100) { value = 20 }; s.style.width = 120; s.style.marginRight = 40; root.Add(s);
       var m = new MinMaxSlider(15, 30, 0, 100); m.style.width = 120; root.Add(m); }   // (minValue, maxValue, lowLimit, highLimit)
w.Repaint(); return WHICH;
