using UnityEditor; using UnityEditor.Overlays; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
var t = sv.overlayCanvas.overlays.First(o => o.displayName == "Tools");
t.Undock(); typeof(Overlay).GetProperty("layout", BF).GetSetMethod(true)?.Invoke(t, new object[]{ Layout.VerticalToolbar }); t.floatingPosition = new Vector2(8, 32 + 24);
sv.Repaint(); return "ok";
