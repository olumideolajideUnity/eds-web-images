// Make the floating Scene view the last active one (so overlays/tools target it), focus and repaint it.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var f = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
var fld = typeof(SceneView).GetFields(BindingFlags.Static|BindingFlags.NonPublic|BindingFlags.Public).First(x => x.Name.Contains("LastActiveSceneView") || x.Name.Contains("lastActiveSceneView"));
fld.SetValue(null, f); f.Focus(); f.Repaint();
return fld.Name + " last=" + (SceneView.lastActiveSceneView == f);
