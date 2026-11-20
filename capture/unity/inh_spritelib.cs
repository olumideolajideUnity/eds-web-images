// Main Sprite Library (idle x6, attack x2: Black Cat) and a variant overriding idle/idle01 with the Tiger Cat, as .spriteLib
// source assets via SpriteLibrarySourceAssetFactory; then open Unity's Sprite Library Editor on the variant (floating, W x H).
using UnityEditor; using UnityEngine; using UnityEngine.U2D.Animation; using UnityEditor.U2D.Animation; using System.Linq; using System.Reflection;
string D = "Assets/EFDS_Screenshots/Sprites"; float W = 440, H = 270;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
Sprite S(string n) => AssetDatabase.LoadAssetAtPath<Sprite>(D + "/" + n + ".png");
string M = D + "/CatMaster_SpriteLib.spriteLib", V = D + "/CatVar_SpriteLib.spriteLib";
if (AssetDatabase.LoadMainAssetAtPath(M) == null) {
  var m = ScriptableObject.CreateInstance<SpriteLibraryAsset>();
  for (int i = 1; i <= 6; i++) m.AddCategoryLabel(S("CatBlack_idle0" + i), "idle", "idle0" + i);
  m.AddCategoryLabel(S("CatBlack_idle03"), "attack", "attack01"); m.AddCategoryLabel(S("CatBlack_idle05"), "attack", "attack02");
  m.SaveAsSourceAsset(M); AssetDatabase.ImportAsset(M);
}
if (AssetDatabase.LoadMainAssetAtPath(V) == null) {
  var v = ScriptableObject.CreateInstance<SpriteLibraryAsset>();
  v.AddCategoryLabel(S("CatTiger_idle01"), "idle", "idle01");
  v.SaveAsSourceAsset(V, M); AssetDatabase.ImportAsset(V);
}
var va = AssetDatabase.LoadAssetAtPath<SpriteLibraryAsset>(V);
var wt = typeof(SpriteLibrarySourceAssetFactory).Assembly.GetType("UnityEditor.U2D.Animation.SpriteLibraryEditor.SpriteLibraryEditorWindow");
var w = (EditorWindow)wt.GetMethod("OpenWindowForAsset", BF).Invoke(null, new object[]{ va });
if (w.docked) { var f = Resources.FindObjectsOfTypeAll(wt).Cast<EditorWindow>().First(); }
w.position = new Rect(200, 150, W, H);
return "docked=" + w.docked + " main=" + va.name + " cats=" + string.Join(",", va.GetCategoryNames());
