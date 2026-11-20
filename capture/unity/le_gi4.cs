using UnityEditor; using UnityEngine; using System.Reflection;
var BF = BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var p = typeof(Lightmapping).GetProperty("realtimeGI", BF);
string e = ""; try { p.SetValue(null, true); } catch (System.Exception x) { e = (x.InnerException ?? x).Message; }
var ls = Lightmapping.lightingSettings; var lp = typeof(LightingSettings).GetProperty("realtimeGI");
string e2 = ""; try { lp.SetValue(ls, true); } catch (System.Exception x) { e2 = (x.InnerException ?? x).Message; }
return "after: Lightmapping.realtimeGI=" + p.GetValue(null) + " ls.realtimeGI=" + lp.GetValue(ls) + " err=" + e + " | " + e2;
