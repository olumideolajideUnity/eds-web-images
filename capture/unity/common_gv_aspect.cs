// Set the (main) Game view to a fixed resolution W x H (adds a custom 'EFDS' size if needed), or RESET=true to go back to
// Free Aspect. The Scene view's Cameras overlay preview takes its aspect from the Game view.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
int W = 1984, H = 1000; bool RESET = false;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var asm = typeof(Editor).Assembly;
var sizesT = asm.GetType("UnityEditor.GameViewSizes"); var sizeT = asm.GetType("UnityEditor.GameViewSize"); var typeT = asm.GetType("UnityEditor.GameViewSizeType");
var inst = sizesT.BaseType.GetProperty("instance", BF).GetValue(null);
var grpType = sizesT.GetProperty("currentGroupType", BF).GetValue(inst);
var grp = sizesT.GetMethod("GetGroup", BF).Invoke(inst, new[]{ grpType });
var gt = grp.GetType(); int total = (int)gt.GetMethod("GetTotalCount", BF).Invoke(grp, null);
int idx = -1;
for (int i = 0; i < total; i++) { var s = gt.GetMethod("GetGameViewSize", BF).Invoke(grp, new object[]{ i }); if ((string)sizeT.GetProperty("baseText", BF).GetValue(s) == "EFDS") idx = i; }
var gv = Resources.FindObjectsOfTypeAll(asm.GetType("UnityEditor.GameView")).Cast<EditorWindow>().First();
if (RESET) { gv.GetType().GetProperty("selectedSizeIndex", BF).SetValue(gv, 0); gv.Repaint(); return "free aspect"; }
if (idx < 0) {
  var ns = System.Activator.CreateInstance(sizeT, new object[]{ System.Enum.Parse(typeT, "FixedResolution"), W, H, "EFDS" });
  gt.GetMethod("AddCustomSize", BF).Invoke(grp, new[]{ ns }); idx = total;
} else { var s = gt.GetMethod("GetGameViewSize", BF).Invoke(grp, new object[]{ idx }); sizeT.GetProperty("width", BF).SetValue(s, W); sizeT.GetProperty("height", BF).SetValue(s, H); }
gv.GetType().GetProperty("selectedSizeIndex", BF).SetValue(gv, idx); gv.Repaint();
return "EFDS size index " + idx;
