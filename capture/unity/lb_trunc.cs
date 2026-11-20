// Components > Label truncated: a UI Toolkit Toggle whose label ("Allow Dynamic Resolution") is wider than its label column,
// in a plain host window (214 x 184pt), so Unity truncates it as it would in an Inspector. Reports label rect to C:/tmp/lb_trunc.json.
using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq;
var hostT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSPopupHost")).First(x => x != null);
foreach (var o in Resources.FindObjectsOfTypeAll(hostT).Cast<EditorWindow>().ToList()) o.Close();
var w = (EditorWindow)ScriptableObject.CreateInstance(hostT); w.titleContent = new GUIContent("EFDSPopupHost"); w.ShowUtility(); w.position = new Rect(500, 300, 214, 184);
var root = w.rootVisualElement; root.Clear();
var t = new Toggle("Allow Dynamic Resolution") { value = false, tooltip = "Allow Dynamic Resolution" };
t.style.position = Position.Absolute; t.style.left = 60; t.style.top = 84; root.Add(t);
var lab = t.Q<Label>(className: "unity-base-field__label"); lab.style.minWidth = 118; lab.style.width = 118; lab.style.maxWidth = 118;
root.schedule.Execute(() => System.IO.File.WriteAllText("C:/tmp/lb_trunc.json", "{\"label\":[" + lab.worldBound.x + "," + lab.worldBound.y + "," + lab.worldBound.width + "," + lab.worldBound.height + "],\"overflow\":\"" + lab.resolvedStyle.textOverflow + "\",\"truncated\":" + (lab.GetType().GetProperty("isElided", System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Public)?.GetValue(lab) ?? "null").ToString().ToLower() + "}")).StartingIn(400);
w.Repaint(); return "ok";
