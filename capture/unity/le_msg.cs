using UnityEditor; using UnityEngine; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var asm = typeof(Editor).Assembly; var le = asm.GetType("UnityEditor.LogEntries");
var entT = asm.GetType("UnityEditor.LogEntry"); var ent = System.Activator.CreateInstance(entT);
le.GetMethod("StartGettingEntries", BF)?.Invoke(null, null);
le.GetMethod("GetEntryInternal", BF).Invoke(null, new object[]{ 0, ent });
le.GetMethod("EndGettingEntries", BF)?.Invoke(null, null);
System.IO.File.WriteAllText("C:/tmp/msg.txt", entT.GetField("message").GetValue(ent).ToString()); return "ST=" + PlayerSettings.GetStackTraceLogType(LogType.Warning) + "/" + Application.GetStackTraceLogType(LogType.Warning) + " MSG=" + entT.GetField("message").GetValue(ent).ToString().Replace("\n", " || ").Replace("\"", "'");
