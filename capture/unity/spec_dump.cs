// Write recorded rects + inspector geometry next to the capture. Edit OUT.
using UnityEditor; using UnityEngine; using System.Linq;
string OUT = "C:/tmp/out.json";
var it = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
var insp = Resources.FindObjectsOfTypeAll(it).Cast<EditorWindow>().First(w => !w.docked);
var p = insp.position;
var json = "{\"ppp\":" + EditorGUIUtility.pixelsPerPoint.ToString(System.Globalization.CultureInfo.InvariantCulture)
  + ",\"win\":[" + string.Join(",", new[]{ p.x, p.y, p.width, p.height }.Select(v => v.ToString(System.Globalization.CultureInfo.InvariantCulture))) + "]"
  + ",\"rects\":" + EFDS.Rects.Dump() + "}";
System.IO.File.WriteAllText(OUT, json);
return json.Length + " chars";
