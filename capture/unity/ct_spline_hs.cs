// Spline handle settings for the CT spline shots: knot indices / flow arrows off, all tangents on (set IDX/FLOW true to restore).
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
bool IDX = false, FLOW = false, ALLT = true;
var BF = BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var t = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("UnityEditor.Splines.SplineHandleSettings")).First(x => x != null);
void Set(string n, bool v) { var p = t.GetProperty(n, BF); if (p != null && p.CanWrite) p.SetValue(null, v); }
Set("ShowKnotIndices", IDX); Set("FlowDirectionEnabled", FLOW); Set("ShowAllTangents", ALLT);
SceneView.RepaintAll();
return string.Join(",", t.GetProperties(BF).Select(p => p.Name + "=" + p.GetValue(null)));
