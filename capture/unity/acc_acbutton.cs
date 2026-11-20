// Accessibility a11y-4: the Inspector's own Add Component button style ("AC Button") drawn by IMGUI in a plain host window on
// the Inspector background, at the Inspector's rect (229.6 x 24pt), enabled (top, y=10) and disabled (GUI.enabled = false, y=50).
using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq;
var hostT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSPopupHost")).First(x => x != null);
foreach (var o in Resources.FindObjectsOfTypeAll(hostT).Cast<EditorWindow>().ToList()) o.Close();
var w = (EditorWindow)ScriptableObject.CreateInstance(hostT); w.titleContent = new GUIContent("EFDSPopupHost"); w.ShowUtility(); w.position = new Rect(400, 300, 300, 90);
var root = w.rootVisualElement; root.Clear(); root.style.backgroundColor = EditorGUIUtility.isProSkin ? (Color)new Color32(50, 50, 50, 255) : (Color)new Color32(215, 215, 215, 255);   // Inspector bg per skin (measured)
var st = new GUIStyle("AC Button");
var c = new IMGUIContainer(() => {
  GUI.Button(new Rect(35.2f, 10, 229.6f, 24), "Add Component", st);
  using (new EditorGUI.DisabledScope(true)) GUI.Button(new Rect(35.2f, 50, 229.6f, 24), "Add Component", st);
});
c.style.flexGrow = 1; root.Add(c); w.Repaint();
return "style=" + st.name + " font=" + st.fontSize;
