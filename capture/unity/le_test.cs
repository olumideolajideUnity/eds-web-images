using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var le = typeof(Editor).Assembly.GetType("UnityEditor.LogEntries");
var cwT = typeof(Editor).Assembly.GetType("UnityEditor.ConsoleWindow");
var f = (int)le.GetProperty("consoleFlags", BF).GetValue(null);
var t = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.Example")).First(x => x != null);
var dc = typeof(EditorApplication).GetField("delayCall", BF)?.GetValue(null) as System.Delegate;
return "count=" + le.GetMethod("GetCount", BF).Invoke(null, null) + " flags=" + f + " delay=" + (dc == null ? 0 : dc.GetInvocationList().Length) + " asm=" + t.Assembly.GetName().Name;
