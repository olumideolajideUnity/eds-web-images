// Fundamentals > Interactions specimens built from real UI Toolkit controls in a plain host window (EFDS.EFDSPopupHost):
// WHICH = rounded (icon button, button, integer field; 390 x 180pt), containers (editable fields vs read-only labels; 390 x 254pt),
// assign0 / assign1 / assign2 (an ObjectField for a Material: idle / drag-hover accept state / assigned; 266 x 300pt).
// Reports element rects (pt) to C:/tmp/int_misc.json.
using UnityEditor; using UnityEditor.UIElements; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection;
using System.Collections.Generic;
string WHICH = "rounded";
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var size = WHICH == "rounded" ? new Vector2(390, 180) : WHICH == "containers" ? new Vector2(390, 254) : new Vector2(266, 300);
var hostT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSPopupHost")).First(x => x != null);
foreach (var o in Resources.FindObjectsOfTypeAll(hostT).Cast<EditorWindow>().ToList()) o.Close();
var w = (EditorWindow)ScriptableObject.CreateInstance(hostT); w.titleContent = new GUIContent("EFDSPopupHost"); w.ShowUtility(); w.position = new Rect(400, 200, size.x, size.y);
var root = w.rootVisualElement; root.Clear();
var named = new Dictionary<string, VisualElement>();
T At<T>(string key, T e, float x, float y, float width = -1) where T : VisualElement {
  e.style.position = Position.Absolute; e.style.left = x; e.style.top = y; if (width > 0) e.style.width = width; root.Add(e); named[key] = e; return e; }
void LabelWidth(VisualElement f, float px) { var l = f.Q(className: "unity-base-field__label"); if (l != null) { l.style.minWidth = px; l.style.width = px; } }
if (WHICH == "rounded") {
  var icon = EditorGUIUtility.Load("d_FolderOpened Icon") as Texture2D ?? EditorGUIUtility.FindTexture("FolderOpened Icon");
  var ib = At("icon", new Button { iconImage = Background.FromTexture2D(icon) }, 175, 32, 40); ib.style.height = 20;
  var b = At("button", new Button { text = "Add Component" }, 118, 79, 155); b.style.height = 22;
  At("int", new IntegerField { value = 20 }, 149, 125, 92);
} else if (WHICH == "containers") {
  At("meta1", new Label("Meta data") { style = { unityFontStyleAndWeight = FontStyle.Bold } }, 17, 80);
  float[] ys = { 98, 117, 136 }; string[] labs = { "ID", "Width", "Height" }; int[] vals = { -56, 260, 64 };
  for (int i = 0; i < 3; i++) { var f = At("f" + i, new IntegerField(labs[i]) { value = vals[i] }, 17, ys[i], 160); LabelWidth(f, 56); }
  var dd = At("status", new DropdownField("Status", new List<string>{ "Disabled", "Enabled" }, 0), 17, 194, 160); LabelWidth(dd, 56);
  At("meta2", new Label("Meta data") { style = { unityFontStyleAndWeight = FontStyle.Bold } }, 212, 80);
  for (int i = 0; i < 3; i++) { At("l" + i, new Label(labs[i] + ":"), 212, ys[i] + 2); At("v" + i, new Label(vals[i].ToString()), 278, ys[i] + 2); }
  At("sl", new Label("Status:"), 212, 196); At("sv", new Label("Disabled"), 278, 196);
} else {
  var of = At("of", new ObjectField { objectType = typeof(Material), allowSceneObjects = false }, 35, 140, 198);
  if (WHICH == "assign2") of.value = AssetDatabase.LoadAssetAtPath<Material>(AssetDatabase.FindAssets("Easel t:Material").Select(AssetDatabase.GUIDToAssetPath).First());
  if (WHICH == "assign2") root.schedule.Execute(() => { var ps = typeof(VisualElement).GetProperty("pseudoStates", BF); foreach (var e in new VisualElement[]{ of, of.Q(className: "unity-object-field__input") }) if (e != null) ps.SetValue(e, System.Enum.ToObject(ps.PropertyType, 64)); }).StartingIn(150);   // focused after the drop
  if (WHICH == "assign1") root.schedule.Execute(() => { var disp = of.Q(className: "unity-object-field-display"); disp?.AddToClassList("unity-object-field-display--accept-drop"); }).StartingIn(150);
}
root.schedule.Execute(() => System.IO.File.WriteAllText("C:/tmp/int_misc.json", "{" + string.Join(",", named.Select(kv => "\"" + kv.Key + "\":[" + kv.Value.layout.x + "," + kv.Value.layout.y + "," + kv.Value.layout.width + "," + kv.Value.layout.height + "]")) + "}")).StartingIn(300);
w.Repaint();
return WHICH;
