// While in Play mode, rotate the scene's Directional Light and move the Point Light so realtime GI keeps updating.
// ON=false removes the callback.
using UnityEditor; using UnityEngine; using System;
bool ON = true;
var key = "EFDS_RotLight";
if (AppDomain.CurrentDomain.GetData(key) is EditorApplication.CallbackFunction old) { EditorApplication.update -= old; AppDomain.CurrentDomain.SetData(key, null); }
if (!ON) return "off";
EditorApplication.CallbackFunction cb = () => {
  if (!EditorApplication.isPlaying) return;
  var d = GameObject.Find("Directional Light"); if (d) d.transform.rotation = Quaternion.Euler(40, (float)(EditorApplication.timeSinceStartup * 40 % 360), 0);
  var p = GameObject.Find("Point Light"); if (p) p.transform.position = new Vector3(Mathf.Sin((float)EditorApplication.timeSinceStartup * 2) * 3, 2.5f, -1);
};
EditorApplication.update += cb; AppDomain.CurrentDomain.SetData(key, cb);
return "on";
