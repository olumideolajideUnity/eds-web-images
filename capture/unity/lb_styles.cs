// Components > Label styles (label-styles.png): UI Toolkit specimens in a plain host window, 800 x 440pt, dark:
// Regular (DropdownField label, Button, regular foldout row), Bold (bold label with child labels), Focused (focused IntegerField,
// and one with its text selected), semantic text (Console-style detail text with a link). Rects (pt) to C:/tmp/lb_styles.json.
using UnityEditor; using UnityEditor.UIElements; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection;
using System.Collections.Generic;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var hostT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSPopupHost")).First(x => x != null);
foreach (var o in Resources.FindObjectsOfTypeAll(hostT).Cast<EditorWindow>().ToList()) o.Close();
var w = (EditorWindow)ScriptableObject.CreateInstance(hostT); w.titleContent = new GUIContent("EFDSPopupHost"); w.ShowUtility(); w.position = new Rect(200, 100, 800, 440);
var root = w.rootVisualElement; root.Clear();
var named = new Dictionary<string, VisualElement>();
var ps = typeof(VisualElement).GetProperty("pseudoStates", BF);
T At<T>(string key, T e, float x, float y, float width = -1) where T : VisualElement { e.style.position = Position.Absolute; e.style.left = x; e.style.top = y; if (width > 0) e.style.width = width; root.Add(e); named[key] = e; return e; }
void LW(VisualElement f, float px) { root.schedule.Execute(() => { var l = f.Q(className: "unity-base-field__label"); if (l != null) { l.style.minWidth = px; l.style.width = px; } }); }
var folder = EditorGUIUtility.IconContent("Folder Icon").image as Texture2D;
// Regular
var dd = At("fov", new DropdownField("FOV Axis", new List<string>{ "Vertical" }, 0), 30, 89, 204); LW(dd, 76);
At("add", new Button { text = "Add Component" }, 28, 126, 210);
var fo = new Foldout { text = "Profile Analyzer", value = false }; var input = fo.Q(className: "unity-toggle__input");
var im = new Image { image = folder }; im.style.width = 16; im.style.height = 16; im.style.marginRight = 3; input.Insert(input.IndexOf(fo.Q(className: "unity-foldout__text")), im);
At("pa", fo, 62, 165);
// Bold
At("st", new Label("Shadow Type") { style = { unityFontStyleAndWeight = FontStyle.Bold } }, 330, 70);
At("bsa", new Label("Baked Shadow Angle"), 345, 90); At("rs", new Label("Realtime Shadows"), 345, 110);
// Focused / highlighted
var s1 = At("size1", new IntegerField("Size") { value = 100 }, 576, 89, 180); LW(s1, 36);
var s2 = At("size2", new IntegerField("Size") { value = 100 }, 576, 149, 180); LW(s2, 36);
root.schedule.Execute(() => {
  foreach (var e in new VisualElement[]{ s1, s1.Q(className: "unity-base-text-field__input"), s1.Q<Label>() }) if (e != null) ps.SetValue(e, System.Enum.ToObject(ps.PropertyType, 64));
  s2.Focus(); s2.SelectAll();
}).StartingIn(200);
// Semantic: console detail text with a file link (rich text link, as the Console draws it)
At("detail", new Label("UnityEngine.Debug:LogWarning (object)\nUnity.Editor:ExampleError () (at <a href=\"Library/Example/BadFile.cs\">Library/Example/BadFile.cs:1550</a>)"), 17, 296);
root.schedule.Execute(() => System.IO.File.WriteAllText("C:/tmp/lb_styles.json", "{" + string.Join(",", named.Select(kv => "\"" + kv.Key + "\":[" + kv.Value.worldBound.x + "," + kv.Value.worldBound.y + "," + kv.Value.worldBound.width + "," + kv.Value.worldBound.height + "]")) + "}")).StartingIn(500);
w.Focus(); w.Repaint(); return "ok";
