// Empty states #6 (error variant): exactly what Unity 7's offline "Retry connection" button publishes, minus RefreshProject()
// (which, while offline, leaves the page in this state; online it would leave the offline state).
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var asm = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "UnityEditor.UnityConnectModule");
var nmT = asm.GetTypes().First(t => t.Name == "NotificationManager");
var nm = nmT.GetProperty("instance", BF).GetValue(null);
var pub = nmT.GetMethods(BF).First(x => x.Name == "Publish" && x.GetParameters().Length == 4);
var p = pub.GetParameters();
pub.Invoke(nm, new object[]{ System.Enum.ToObject(p[0].ParameterType, 1), System.Enum.ToObject(p[1].ParameterType, 0), L10n.Tr("Connection refresh attempted"), true });
return "published topic=" + System.Enum.ToObject(p[0].ParameterType, 1) + " severity=" + System.Enum.ToObject(p[1].ParameterType, 0);
