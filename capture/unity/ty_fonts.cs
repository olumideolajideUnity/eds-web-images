// Typography: font sizes of the Editor styles the a11y-11 callouts point at, read live.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
string F(string n, GUIStyle s) => n + "=" + (s == null ? "null" : s.fontSize + (s.fontStyle != FontStyle.Normal ? "/" + s.fontStyle : "") + " font=" + (s.font ? s.font.name : "-"));
var r = new System.Collections.Generic.List<string>{
  F("label", EditorStyles.label), F("boldLabel", EditorStyles.boldLabel), F("helpBox", EditorStyles.helpBox), F("miniLabel", EditorStyles.miniLabel),
  F("largeLabel", EditorStyles.largeLabel), F("toolbarSearchField", EditorStyles.toolbarSearchField), F("textField", EditorStyles.textField),
  F("whiteLargeLabel", EditorStyles.whiteLargeLabel), F("toolbarButton", EditorStyles.toolbarButton), F("miniBoldLabel", EditorStyles.miniBoldLabel) };
var sw = typeof(Editor).Assembly.GetType("UnityEditor.SettingsWindow");
foreach (var nt in sw.GetNestedTypes(BF)) foreach (var f in nt.GetFields(BF).Where(f => f.FieldType == typeof(GUIStyle))) { try { r.Add("SW." + nt.Name + "." + f.Name + ":" + F("", (GUIStyle)f.GetValue(null))); } catch {} }
var sp = typeof(SettingsProvider); foreach (var nt in sp.GetNestedTypes(BF)) foreach (var f in nt.GetFields(BF).Where(f => f.FieldType == typeof(GUIStyle))) { try { r.Add("SP." + nt.Name + "." + f.Name + ":" + F("", (GUIStyle)f.GetValue(null))); } catch {} }
System.IO.File.WriteAllText("C:/tmp/fonts.txt", string.Join("\n", r)); return "ok";
