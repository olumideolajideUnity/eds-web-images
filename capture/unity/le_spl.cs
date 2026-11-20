using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var cwT = typeof(Editor).Assembly.GetType("UnityEditor.ConsoleWindow");
var w = Resources.FindObjectsOfTypeAll(cwT).Cast<EditorWindow>().First(x => !x.docked);
var spl = cwT.GetField("spl", BF).GetValue(w);
var s = string.Join("; ", spl.GetType().GetFields(BF).Select(f => { var v = f.GetValue(spl); return f.Name + "=" + (v is System.Array a ? string.Join(",", a.Cast<object>()) : v); }));
System.IO.File.WriteAllText("C:/tmp/spl.txt", s + "\nMETHODS: " + string.Join(",", spl.GetType().GetMethods(BF).Where(m => m.DeclaringType == spl.GetType()).Select(m => m.Name)));
return "ok";
