// Tooltip page: show a real Editor tooltip (UnityEditor.TooltipView, the view the Editor uses on hover) for TEXT, anchored under the
// screen rect (X, Y, W, H) pt, so it can be grabbed from its own view buffer (capview UnityEditor.TooltipView). Returns its window rect.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
string TEXT = System.IO.File.ReadAllText("C:/tmp/tt_text.txt"); float X = 600, Y = 400, W = 60, H = 18;   // text from a file (newlines, quotes)
var BF = BindingFlags.Static|BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var t = typeof(Editor).Assembly.GetType("UnityEditor.TooltipView");
var show = t.GetMethods(BF).Where(m => m.Name == "Show").OrderBy(m => m.GetParameters().Length).ToList();
var sig = string.Join(" ; ", show.Select(m => string.Join(",", m.GetParameters().Select(p => p.ParameterType.Name + " " + p.Name))));
var m0 = show.First(m => m.GetParameters().Length >= 2 && m.GetParameters()[0].ParameterType == typeof(string) && m.GetParameters()[1].ParameterType == typeof(Rect));
var ps = m0.GetParameters(); var a = new object[ps.Length]; a[0] = TEXT; a[1] = new Rect(X, Y, W, H);
for (int i = 2; i < ps.Length; i++) a[i] = ps[i].HasDefaultValue ? ps[i].DefaultValue : null;
m0.Invoke(null, a);
var tv = Resources.FindObjectsOfTypeAll(t).FirstOrDefault();
var win = tv == null ? null : t.GetProperty("window", BF)?.GetValue(tv);
var pos = win?.GetType().GetProperty("position", BF)?.GetValue(win);
var r = (Rect)pos; System.IO.File.WriteAllText("C:/tmp/tt_rect.json", "[" + (r.x - X - W / 2) + "," + (r.y - Y - H) + "," + r.width + "," + r.height + "]");   // offset from anchor centre / bottom, size
return "win " + pos;
