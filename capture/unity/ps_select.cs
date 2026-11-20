// Select a settings page in an open Settings window (Preferences or Project Settings) by provider path.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
string WIN = "PreferenceSettingsWindow"; string PAGE = "Preferences/General";
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var w = Resources.FindObjectsOfTypeAll<EditorWindow>().First(x => x.GetType().Name == WIN);
var m = w.GetType().GetMethods(BF).Where(x => x.Name == "SelectProviderByName").OrderBy(x => x.GetParameters().Length).First();
var args = m.GetParameters().Select((p, i) => i == 0 ? (object)PAGE : (p.HasDefaultValue ? p.DefaultValue : (p.ParameterType == typeof(bool) ? (object)false : null))).ToArray();
m.Invoke(w, args); w.Repaint();
return "selected " + PAGE + " params=" + m.GetParameters().Length;
