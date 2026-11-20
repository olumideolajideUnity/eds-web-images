using UnityEditor; using UnityEngine; using System.Linq;
var t = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSMessageSpecimens")).First(x => x != null);
var r = (Rect[])t.GetField("rects").GetValue(null);
return "[" + string.Join(",", r.Select(x => $"[{x.x},{x.y},{x.width},{x.height}]")) + "]";
