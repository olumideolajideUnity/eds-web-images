using UnityEditor; using UnityEngine; using System.Reflection;
#pragma warning disable 618, 619
var BF = BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
object G(System.Type t, string n) { try { return t.GetProperty(n, BF)?.GetValue(null); } catch (System.Exception e) { return e.InnerException?.GetType().Name ?? e.GetType().Name; } }
return "EditorSettings.enableEnlightenBakedGI=" + G(typeof(EditorSettings), "enableEnlightenBakedGI") + " Lightmapping.realtimeGI=" + G(typeof(Lightmapping), "realtimeGI") + " realtimeGICPUUsage=" + G(typeof(QualitySettings), "realtimeGICPUUsage");
