using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var a = System.AppDomain.CurrentDomain.GetAssemblies().First(x => x.GetName().Name == "Unity.ProBuilder.Editor");
a.GetType("UnityEditor.ProBuilder.MenuActionSettings").GetMethod("ResetMenuActionSettingsStatics", BF)?.Invoke(null, null);
var act = System.Activator.CreateInstance(a.GetType("UnityEditor.ProBuilder.Actions.BevelEdges"));
var mas = System.Activator.CreateInstance(a.GetType("UnityEditor.ProBuilder.MenuActionSettings"), BF, null, new object[]{ act, true }, null);
SceneView.RepaintAll();
var ea = System.AppDomain.CurrentDomain.GetAssemblies().Select(x => x.GetType("UnityEditor.Actions.EditorAction")).First(x => x != null);
var st = ea.GetMethods(BF).FirstOrDefault(m => m.Name == "Start" && m.IsStatic && !m.ContainsGenericParameters && m.GetParameters().Length == 1);
st?.Invoke(null, new[]{ mas });
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
var ov = sv.overlayCanvas.overlays.FirstOrDefault(o => o.displayName == "Bevel Edges");
if (ov != null) { ov.Undock(); ov.floatingPosition = new Vector2(820, 480); }
return "overlay=" + (ov != null) + " tool=" + UnityEditor.EditorTools.ToolManager.activeToolType?.Name;
