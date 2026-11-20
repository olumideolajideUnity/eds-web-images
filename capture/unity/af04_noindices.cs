// Splines visuals: hide knot indices (off in the dark af04 capture).
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var t = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("UnityEditor.Splines.SplineHandleSettings")).First(x => x != null);
t.GetProperty("ShowKnotIndices", BF).SetValue(null, false); SceneView.RepaintAll();
return "indices=" + t.GetProperty("ShowKnotIndices", BF).GetValue(null);
