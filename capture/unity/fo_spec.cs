// Components > Foldout specimens in a plain host window (EFDS.EFDSPopupHost), 800 x 300pt, dark. Real UI Toolkit Foldouts / ListView /
// TreeView and fields, plus IMGUI foldout APIs (EditorGUI.Foldout, EditorGUI.BeginFoldoutHeaderGroup) for the variations image.
// Rects (pt) of interesting parts go to C:/tmp/fo_spec.json.  WHICH: variations | flvtv | content | anatomy | indentation
using UnityEditor; using UnityEditor.UIElements; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection;
using System.Collections.Generic;
string WHICH = "variations";
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var hostT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSPopupHost")).First(x => x != null);
foreach (var o in Resources.FindObjectsOfTypeAll(hostT).Cast<EditorWindow>().ToList()) o.Close();
var w = (EditorWindow)ScriptableObject.CreateInstance(hostT); w.titleContent = new GUIContent("EFDSPopupHost"); w.ShowUtility(); w.position = new Rect(200, 150, 800, 300);
var root = w.rootVisualElement; root.Clear();
var named = new Dictionary<string, VisualElement>(); var rects = new Dictionary<string, Rect>();
var folder = EditorGUIUtility.IconContent("Folder Icon").image as Texture2D; var folderOpen = EditorGUIUtility.IconContent("FolderOpened Icon").image as Texture2D;
T At<T>(string key, T e, float x, float y, float width = -1, float height = -1) where T : VisualElement {
  e.style.position = Position.Absolute; e.style.left = x; e.style.top = y; if (width > 0) e.style.width = width; if (height > 0) e.style.height = height; root.Add(e); if (key != null) named[key] = e; return e; }
void LW(VisualElement f, float px) { root.schedule.Execute(() => { foreach (var l in f.Query(className: "unity-base-field__label").ToList()) { l.style.minWidth = px; l.style.width = px; } }); }
VisualElement Box(float x, float y, float wd, float h) { var b = new VisualElement(); b.style.borderTopWidth = b.style.borderBottomWidth = b.style.borderLeftWidth = b.style.borderRightWidth = 1;
  b.style.borderTopColor = b.style.borderBottomColor = b.style.borderLeftColor = b.style.borderRightColor = (Color)new Color32(25, 25, 25, 255); return At(null, b, x, y, wd, h); }
void Icon(Foldout f, Texture2D t) { var input = f.Q(className: "unity-toggle__input"); var im = new Image { image = t }; im.style.width = 16; im.style.height = 16; im.style.marginRight = 3;
  var txt = f.Q(className: "unity-foldout__text"); input.Insert(input.IndexOf(txt), im); }
if (WHICH == "variations") {
  var imc = new IMGUIContainer(() => {
    EditorGUI.Foldout(new Rect(158, 112, 200, 18), false, "Foldout", true);
    EditorGUI.Foldout(new Rect(438, 112, 260, 18), false, new GUIContent("Foldout with an icon", folder), true);
    EditorGUI.BeginFoldoutHeaderGroup(new Rect(158, 170, 230, 20), false, "Foldout Header"); EditorGUI.EndFoldoutHeaderGroup();
    EditorGUI.BeginFoldoutHeaderGroup(new Rect(438, 170, 260, 20), false, new GUIContent("Foldout Header with an icon", folder)); EditorGUI.EndFoldoutHeaderGroup();
  });
  imc.style.position = Position.Absolute; imc.style.left = 0; imc.style.top = 0; imc.style.width = 800; imc.style.height = 300; root.Add(imc);
} else if (WHICH == "flvtv") {
  Box(40, 118, 226, 190); Box(285, 118, 226, 190); Box(530, 118, 226, 190);
  // tree view of real project folders
  var items = new List<TreeViewItemData<string>>(); int id = 0;
  TreeViewItemData<string> N(string n, params TreeViewItemData<string>[] kids) => new TreeViewItemData<string>(id++, n, kids.ToList());
  items.Add(N("Assets", N("_Recovery"), N("Atrio"), N("EFDS_Screenshots", N("GameplaySequenceDemo", N("Animation"), N("Audio"), N("Character", N("Materials"), N("Models"), N("Textures")), N("Info", N("Icons"), N("Scripts")))), N("ExampleAssets"), N("Scenes")));
  var tv = new TreeView { fixedItemHeight = 16 }; tv.SetRootItems(items);
  tv.makeItem = () => { var r = new VisualElement { style = { flexDirection = FlexDirection.Row, alignItems = Align.Center } }; var im = new Image(); im.style.width = 16; im.style.height = 16; im.style.marginRight = 3; r.Add(im); r.Add(new Label()); return r; };
  tv.bindItem = (e, i) => { var hasKids = tv.GetChildrenIdsForIndex(i).Any(); e.Q<Image>().image = hasKids && tv.IsExpanded(tv.GetIdForIndex(i)) ? folderOpen : folder; e.Q<Label>().text = tv.GetItemDataForIndex<string>(i); };
  At("tree", tv, 41, 120, 224, 188); tv.Rebuild(); root.schedule.Execute(() => { tv.ExpandAll(); tv.Rebuild(); });
  var li = Enumerable.Repeat("List View Item", 8).ToList();
  At("list", new ListView(li, 16, () => new Label(), (e, i) => ((Label)e).text = li[i]), 286, 120, 224, 188);
  var f = new Foldout { text = "Lighting", value = true }; f.Add(new DropdownField("Cast Shadows", new List<string>{ "On" }, 0)); f.Add(new Toggle("Receive Shadows") { value = true });
  At("foldout", f, 534, 121, 218); LW(f, 120);
} else if (WHICH == "content") {
  var f = new Foldout { text = "Probes", value = true };
  f.Add(new DropdownField("Light Probes", new List<string>{ "Blend Probes" }, 0)); f.Add(new DropdownField("Reflection Probes", new List<string>{ "Blend Probes" }, 0));
  f.Add(new ObjectField("Anchor Override") { objectType = typeof(Transform) });
  At("foldout", f, 212, 112, 380); LW(f, 128);
} else if (WHICH == "anatomy") {
  var f = new Foldout { text = "Foldout Label", value = true }; Icon(f, folderOpen);
  f.Add(new IntegerField("Content row") { value = 1 }); f.Add(new Toggle("Content row") { value = true }); f.Add(new DropdownField("Content row", new List<string>{ "On" }, 0));
  var b = At("foldout", f, 160, 95, 260); LW(f, 145);
  root.schedule.Execute(() => { foreach (var x in f.Query(className: "unity-base-field__input").ToList()) x.style.width = 76; });
  b.style.scale = new Scale(new Vector3(1.4f, 1.4f, 1)); b.style.transformOrigin = new TransformOrigin(0, 0);
} else if (WHICH == "indentation") {
  Box(215, 90, 376, 220);
  var f1 = new Foldout { text = "Foldout Level 1", value = true };
  f1.Add(new DropdownField("Light Probes", new List<string>{ "Blend Probes" }, 0)); f1.Add(new DropdownField("Reflection Probes", new List<string>{ "Blend Probes" }, 0));
  f1.Add(new ObjectField("Anchor Override") { objectType = typeof(Transform) });
  var f2 = new Foldout { text = "Foldout Level 2", value = true }; f2.Add(new Toggle("Dynamic Occlusion") { value = true }); f1.Add(f2);
  At("f1", f1, 219, 104, 368); LW(f1, 140); named["f2"] = f2;
}
root.schedule.Execute(() => {
  foreach (var kv in named) {
    rects[kv.Key] = kv.Value.worldBound;
    foreach (var fo in kv.Value.Query<Foldout>().ToList()) {
      var n = kv.Key + ":" + fo.text; var ck = fo.Q(className: "unity-foldout__checkmark"); var tx = fo.Q(className: "unity-foldout__text"); var ct = fo.Q(className: "unity-foldout__content");
      if (ck != null) rects[n + ":arrow"] = ck.worldBound; if (tx != null) rects[n + ":text"] = tx.worldBound; if (ct != null) rects[n + ":content"] = ct.worldBound;
      var im = fo.Q<Image>(); if (im != null) rects[n + ":icon"] = im.worldBound;
      int i = 0; foreach (var l in ct.Children().Select(c => c.Q(className: "unity-base-field__label")).Where(l => l != null)) rects[n + ":label" + (i++)] = l.worldBound;
      int j = 0; foreach (var c in ct.Children()) rects[n + ":row" + (j++)] = c.worldBound;
    } }
  System.IO.File.WriteAllText("C:/tmp/fo_spec.json", "{" + string.Join(",", rects.Select(kv => "\"" + kv.Key + "\":[" + kv.Value.x + "," + kv.Value.y + "," + kv.Value.width + "," + kv.Value.height + "]")) + "}");
}).StartingIn(600);
w.Repaint();
return WHICH;
