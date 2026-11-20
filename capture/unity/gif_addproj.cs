// adding_project.gif setup (user screen recording): temporary folders Assets/Art assets/{Environment art, Textures, UI Icons/Icon set 2021}
// as in the original, then (after unity/gif_proj.cs with SHOW "Assets/Art assets") Assets, Art assets and UI Icons expanded in the
// floating two-column Project browser. The user drags "New icon set" from the desktop onto UI Icons. MODE CLEAN deletes Art assets.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
string MODE = "FOLDERS";
const string R = "Assets/Art assets";
if (MODE == "CLEAN") return "deleted " + AssetDatabase.DeleteAsset(R);
if (MODE == "FOLDERS") {
  if (!AssetDatabase.IsValidFolder(R)) AssetDatabase.CreateFolder("Assets", "Art assets");
  foreach (var f in new[]{ "Environment art", "Textures", "UI Icons" }) if (!AssetDatabase.IsValidFolder(R + "/" + f)) AssetDatabase.CreateFolder(R, f);
  if (!AssetDatabase.IsValidFolder(R + "/UI Icons/Icon set 2021")) AssetDatabase.CreateFolder(R + "/UI Icons", "Icon set 2021");
  AssetDatabase.Refresh(); return "folders"; }
// MODE EXPAND
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var t = typeof(Editor).Assembly.GetType("UnityEditor.ProjectBrowser");
var w = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().First(x => !x.docked);
var tree = t.GetField("m_FolderTree", BF).GetValue(w);
var data = tree.GetType().GetProperty("data", BF).GetValue(tree);
MethodInfo[] All(string n) { var l = new System.Collections.Generic.List<MethodInfo>(); for (var ty = data.GetType(); ty != null; ty = ty.BaseType) l.AddRange(ty.GetMethods(BF|BindingFlags.DeclaredOnly).Where(x => x.Name == n)); return l.ToArray(); }
var se = All("SetExpanded").First(m => m.GetParameters()[0].ParameterType == typeof(EntityId));
foreach (var p in new[]{ "Assets", R, R + "/UI Icons" }) se.Invoke(data, new object[]{ AssetDatabase.LoadAssetAtPath<Object>(p).GetEntityId(), true });
All("ReloadData").FirstOrDefault()?.Invoke(data, null);
t.GetMethod("ShowFolderContents", BF, null, new[]{ typeof(EntityId), typeof(bool) }, null)?.Invoke(w, new object[]{ AssetDatabase.LoadAssetAtPath<Object>(R).GetEntityId(), false });
Selection.activeObject = null;
var state = tree.GetType().GetProperty("state", BF).GetValue(tree);
var selP = state.GetType().GetProperty("selectedIDs", BF); if (selP != null) selP.SetValue(state, System.Activator.CreateInstance(selP.PropertyType));
w.Repaint(); return "expanded " + w.position;
