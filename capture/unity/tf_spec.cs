// Components > Text field specimens (dark): real UI Toolkit TextFields (+ Foldout / ProgressBar / Toggle / Button for the behaviour panel)
// and IMGUI EditorGUI.TextField / TextArea, in a plain host window. Rects (pt) and box-model values to C:/tmp/tf_spec.json.
// WHICH: variations (800x300) | lapY, lapN (390x240) | anatomy, margins, hscale, vscale (800x300) | states, themes (400x300, dark half)
using UnityEditor; using UnityEditor.UIElements; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection; using System.Collections.Generic;
string WHICH = "variations";
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var hostT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSPopupHost")).First(x => x != null);
foreach (var o in Resources.FindObjectsOfTypeAll(hostT).Cast<EditorWindow>().ToList()) o.Close();
var size = WHICH.StartsWith("lap") ? new Vector2(390, 240) : WHICH == "states" || WHICH == "themes" || WHICH == "overview" ? new Vector2(400, 300) : new Vector2(800, 300);
var w = (EditorWindow)ScriptableObject.CreateInstance(hostT); w.titleContent = new GUIContent("EFDSPopupHost"); w.ShowUtility(); w.position = new Rect(200, 150, size.x, size.y);
var root = w.rootVisualElement; root.Clear();
var ps = typeof(VisualElement).GetProperty("pseudoStates", BF);
var named = new Dictionary<string, VisualElement>();
void PS(VisualElement e, int s) { if (e != null) ps.SetValue(e, System.Enum.ToObject(ps.PropertyType, s)); }
T At<T>(string key, T e, float x, float y, float wd, float scale = 1, VisualElement parent = null) where T : VisualElement {
  e.style.position = Position.Absolute; e.style.left = x; e.style.top = y; if (wd > 0) e.style.width = wd;
  if (scale != 1) { e.style.scale = new Scale(new Vector3(scale, scale, 1)); e.style.transformOrigin = new TransformOrigin(0, 0); }
  (parent ?? root).Add(e); named[key] = e; return e; }
void LW(VisualElement f, float px) { root.schedule.Execute(() => { var l = f.Q(className: "unity-base-field__label"); if (l != null) { l.style.minWidth = px; l.style.width = px; } }); }
void RealFocus(VisualElement e) { root.schedule.Execute(() => { w.Focus(); e.Focus(); (e as TextField)?.SelectRange(0, 0); }).StartingIn(250);
  root.schedule.Execute(() => (e as TextField)?.SelectNone()).StartingIn(400); }
void HoverAt(VisualElement e) { root.schedule.Execute(() => { var c = e.Q(className: "unity-base-text-field__input").worldBound.center; w.wantsMouseMove = true; w.Focus();
  w.SendEvent(new Event { type = EventType.MouseMove, mousePosition = c }); }).StartingIn(120); }
TextField TF(string label, string val, bool multi = false) { var f = label == null ? new TextField { value = val, multiline = multi } : new TextField(label) { value = val, multiline = multi };
  if (multi) f.Q(className: "unity-base-text-field__input").style.whiteSpace = WhiteSpace.Normal; return f; }
VisualElement Box(string k, float x, float y, float wd, float h) {   // inspector-like box, as the Numeric field layout image
  var box = new VisualElement(); box.AddToClassList("unity-inspector-element");
  box.style.borderTopWidth = box.style.borderBottomWidth = box.style.borderLeftWidth = box.style.borderRightWidth = 1;
  box.style.borderTopColor = box.style.borderBottomColor = box.style.borderLeftColor = box.style.borderRightColor = EditorGUIUtility.isProSkin ? (Color)new Color32(25, 25, 25, 255) : (Color)new Color32(153, 153, 153, 255);
  box.style.paddingTop = 18; At(k, box, x, y, wd); box.style.height = h; return box; }
if (WHICH == "variations") {
  var f = At("uit", TF("Text Field", "is for both single or multiple lines of text", true), 68, 131, 236); LW(f, 62);
  string a = "is for single lines of text", b = "dynamically resizes to accommodate multiple lines of text";
  var gui = new IMGUIContainer(() => {
    EditorGUIUtility.labelWidth = 66;
    a = EditorGUI.TextField(new Rect(0, 0, 236, EditorGUIUtility.singleLineHeight), "Text Field", a);
    var st = new GUIStyle(EditorStyles.textArea) { wordWrap = true }; var r = new Rect(0, 134, 236, 0);
    var fr = EditorGUI.PrefixLabel(r, new GUIContent("Text Area")); fr.height = st.CalcHeight(new GUIContent(b), fr.width); b = EditorGUI.TextArea(fr, b, st); });
  At("imgui", gui, 464, 64, 236); gui.style.height = 200; }
else if (WHICH.StartsWith("lap")) {
  var panel = new VisualElement(); panel.style.paddingLeft = panel.style.paddingRight = 4; panel.style.paddingTop = 4;
  var fo = new Foldout { text = "Lap progress", value = true }; panel.Add(fo);
  var pb = new ProgressBar { title = "Lap 1 of 4", lowValue = 0, highValue = 4, value = 0 }; fo.Add(pb);
  bool y = WHICH == "lapY";
  var msg = y ? TF("Checkpoint Message", "Almost there!\n\nYou cleared a checkpoint! (+10 seconds)\n\nPass all checkpoints before timer runs out", true)
              : TF("Checkpoint Message", "Almost there! You cleared a checkpoint! (+10 seconds) Pass all checkpoints before timer runs out");
  msg.AddToClassList("unity-base-field__aligned"); fo.Add(msg); LW(msg, 132); var tg = new Toggle("Pause Game"); tg.AddToClassList("unity-base-field__aligned"); fo.Add(tg);
  var bt = new Button { text = "Close" }; bt.style.marginTop = 6; bt.style.height = 24; panel.Add(bt);
  At("panel", panel, 14, 3, 330, 1.04f); }
else if (WHICH == "overview") { var f = At("f", TF("Label", "Text Field"), 104, 136, 142, 1.3f); LW(f, 52); }
else if (WHICH == "anatomy") { var f = At("f", TF("Label", "Text Field"), 222, 116, 135, 2.4f); LW(f, 38); }
else if (WHICH == "margins") { var f = At("f", TF("Label", "Text Field"), 230, 104, 110, 3f); LW(f, 40); }
else if (WHICH == "hscale") {
  foreach (var (k, x, wd) in new[]{ ("narrow", 74f, 223f), ("wide", 417f, 327f) }) {
    var box = Box(k, x, 62, wd, 180);
    var f1 = TF("Top Speed", "30"); f1.AddToClassList("unity-base-field__aligned"); box.Add(f1);
    var f2 = TF("Top Speed", "30"); f2.AddToClassList("unity-base-field__aligned"); box.Add(new VisualElement { style = { height = 21 } });   // row spacing as its own element, so f2 keeps its real margins
     f2.SetEnabled(false); box.Add(f2); named[k + "_f2"] = f2; } }
else if (WHICH == "vscale") {
  foreach (var (k, x, y, h, multi) in new[]{ ("short", 85f, 65f, 126f, false), ("tall", 481f, 32f, 192f, true) }) {
    var box = Box(k, x, y, 223, h);
    var f1 = TF("Top Speed", multi ? "30\n\n" : "30", multi); f1.AddToClassList("unity-base-field__aligned"); box.Add(f1);
    var f2 = TF("Top Speed", multi ? "30\n\n" : "30", multi); f2.AddToClassList("unity-base-field__aligned"); box.Add(new VisualElement { style = { height = 21 } });   // row spacing as its own element, so f2 keeps its real margins
     f2.SetEnabled(false); box.Add(f2); named[k + "_f2"] = f2; } }
else if (WHICH == "states") {
  At("default", TF(null, "Default"), 150, 50, 94); var h = At("hover", TF(null, "Hover"), 150, 110, 94); HoverAt(h);
  var f = At("focus", TF(null, "Focused"), 150, 170, 94); RealFocus(f); var d = At("disabled", TF(null, "Disabled"), 150, 230, 94); d.SetEnabled(false); }
else { var f = At("f", TF(null, "Text Field"), 42, 128, 112, 2f); }
root.schedule.Execute(() => {
  var r = new Dictionary<string, Rect>();
  foreach (var kv in named) { var e = kv.Value; r[kv.Key] = e.worldBound; var s = e.resolvedStyle; float k = e.worldTransform.lossyScale.x;
    r[kv.Key + ":margin"] = new Rect(s.marginLeft * k, s.marginTop * k, s.marginRight * k, s.marginBottom * k);
    var l = e.Q(className: "unity-base-field__label"); if (l != null) { r[kv.Key + ":label"] = l.worldBound; r[kv.Key + ":labelmargin"] = new Rect(l.resolvedStyle.marginLeft * k, 0, l.resolvedStyle.marginRight * k, 0); }
    var i = e.Q(className: "unity-base-text-field__input"); if (i != null) { var t = i.resolvedStyle; r[kv.Key + ":input"] = i.worldBound;
      r[kv.Key + ":inputmargin"] = new Rect(t.marginLeft * k, t.marginTop * k, t.marginRight * k, t.marginBottom * k);
      r[kv.Key + ":inputpad"] = new Rect((t.paddingLeft + t.borderLeftWidth) * k, (t.paddingTop + t.borderTopWidth) * k, (t.paddingRight + t.borderRightWidth) * k, (t.paddingBottom + t.borderBottomWidth) * k);
      r[kv.Key + ":inputborder"] = new Rect(t.borderLeftWidth * k, t.borderTopWidth * k, t.borderRightWidth * k, t.borderBottomWidth * k); }
    var te = e.Q(className: "unity-text-element--inner-input-field-component"); if (te != null) r[kv.Key + ":text"] = te.worldBound; }
  System.IO.File.WriteAllText("C:/tmp/tf_spec.json", "{" + string.Join(",", r.Select(kv => "\"" + kv.Key + "\":[" + kv.Value.x + "," + kv.Value.y + "," + kv.Value.width + "," + kv.Value.height + "]")) + "}");
}).StartingIn(700);
w.Repaint(); return WHICH;
