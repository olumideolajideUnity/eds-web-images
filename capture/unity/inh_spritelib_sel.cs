// Drive the Sprite Library Editor through its own view events (as clicks do): grid view at SIZE, select idle + idle01.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection; using System.Collections.Generic;
float SIZE = 0.5f; float W = 442, H = 290;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var asm = typeof(UnityEditor.U2D.Animation.SpriteLibrarySourceAssetFactory).Assembly;
var wt = asm.GetType("UnityEditor.U2D.Animation.SpriteLibraryEditor.SpriteLibraryEditorWindow");
var w = Resources.FindObjectsOfTypeAll(wt).Cast<EditorWindow>().First();
w.position = new Rect(200, 150, W, H);
var ve = wt.GetField("m_ViewEvents", BF).GetValue(w); var vt = ve.GetType();
object Ev(string n) => vt.GetProperty(n, BF).GetValue(ve);
void Fire(string n, object arg) { var e = Ev(n); e.GetType().GetMethod("Invoke").Invoke(e, new[]{ arg }); }
var viewType = asm.GetType("UnityEditor.U2D.Animation.SpriteLibraryEditor.ViewType");
Fire("onViewTypeUpdate", System.Enum.Parse(viewType, "Grid"));
Fire("onViewSizeUpdate", SIZE);
Fire("onSelectCategories", (IList<string>)new List<string>{ "idle" });
Fire("onSelectLabels", (IList<string>)new List<string>{ "idle01" });
w.Repaint();
return "viewTypes=" + string.Join(",", System.Enum.GetNames(viewType));
