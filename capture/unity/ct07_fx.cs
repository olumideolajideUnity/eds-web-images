using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
var fx = typeof(Editor).Assembly.GetType("UnityEditor.SceneFXWindow");
var c = (PopupWindowContent)fx.GetConstructors(BF)[0].Invoke(new object[]{ sv });
var host = EFDS.EFDSPopupHost.Open(c);
var vo = sv.overlayCanvas.overlays.First(o => o.displayName == "View Options");
return "host=" + host.position + " vo=" + vo.rootVisualElement.worldBound;
