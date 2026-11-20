// Accessibility a11y-6: the Inspector's "AC Button" style at 230 x 24pt, drawn by IMGUI on the Inspector background:
// enabled (y=10), disabled (y=50, EditorGUI.DisabledScope) and the 'don't' version (y=90): disabled with its text faded to 30%.
using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq;
var hostT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSPopupHost")).First(x => x != null);
foreach (var o in Resources.FindObjectsOfTypeAll(hostT).Cast<EditorWindow>().ToList()) o.Close();
var w = (EditorWindow)ScriptableObject.CreateInstance(hostT); w.titleContent = new GUIContent("EFDSPopupHost"); w.ShowUtility(); w.position = new Rect(400, 300, 250, 130);
var root = w.rootVisualElement; root.Clear(); root.style.backgroundColor = EditorGUIUtility.isProSkin ? (Color)new Color32(50, 50, 50, 255) : (Color)new Color32(215, 215, 215, 255);   // Inspector bg per skin (measured)
var st = new GUIStyle("AC Button");
var c = new IMGUIContainer(() => {
  GUI.Button(new Rect(10, 10, 230, 24), "Add Component", st);
  using (new EditorGUI.DisabledScope(true)) GUI.Button(new Rect(10, 50, 230, 24), "Add Component", st);
  using (new EditorGUI.DisabledScope(true)) { var cc = GUI.contentColor; GUI.contentColor = new Color(1, 1, 1, 0.3f); GUI.Button(new Rect(10, 90, 230, 24), "Add Component", st); GUI.contentColor = cc; }
});
c.style.flexGrow = 1; root.Add(c); w.Repaint();
return "ok";
