// Empty states #6: put Project Settings > Services into Unity 7's real offline state (no network needed) and frame it like the original (tree + search hidden).
using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
float W = 560, H = 360;
var ps = Resources.FindObjectsOfTypeAll<EditorWindow>().First(x => x.GetType().Name == "ProjectSettingsWindow");
var cur = ps.GetType().GetMethod("GetCurrentProvider", BF).Invoke(ps, null);
var spsT = cur.GetType().BaseType;
var sm = spsT.GetField("m_StateMachine", BF).GetValue(cur);
var evT = spsT.Assembly.GetType("UnityEditor.Connect.ServicesProjectSettings+Event");
var nmT = spsT.Assembly.GetTypes().First(t => t.Name == "NotificationManager");
var nm = nmT.GetProperty("instance", BF).GetValue(null);
foreach (var n in ((System.Collections.IEnumerable)nmT.GetMethod("GetAllNotifications", BF).Invoke(nm, null)).Cast<object>().ToList())
  nmT.GetMethod("Dismiss", BF).Invoke(nm, new object[]{ n.GetType().GetProperty("id", BF)?.GetValue(n) ?? n.GetType().GetField("id", BF)?.GetValue(n) });
var off = spsT.GetField("m_OfflineState", BF).GetValue(cur);
sm.GetType().GetProperty("currentState", BF).SetValue(sm, off);
off.GetType().GetMethod("EnterState", BF).Invoke(off, null);
var state = sm.GetType().GetProperty("currentState", BF).GetValue(sm);
ps.position = new Rect(150, 150, W, H);
var root = ps.rootVisualElement;
string tree = string.Join(" | ", root.Query<VisualElement>().ToList().Take(40).Select(e => e.GetType().Name + "#" + e.name + "." + string.Join(".", e.GetClasses())));
ps.Repaint();
return "state=" + state.GetType().Name + "\n" + tree;
