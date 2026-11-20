using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection; using System.Collections.Generic;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
VisualElement d = null; var st = new Stack<VisualElement>(); st.Push(sv.rootVisualElement.panel.visualTree);
while (st.Count > 0) { var e = st.Pop(); if (e.GetType().Name == "SplineHandleSettingsDropdown") { d = e; break; } foreach (var c in e.Children()) st.Push(c); }
foreach (var w in Resources.FindObjectsOfTypeAll<EditorWindow>().Where(w => w.GetType().Name == "SplineHandleSettingsWindow").ToList()) w.Close();
d.GetType().GetMethod("OnClick", BF).Invoke(d, null);
var pw = Resources.FindObjectsOfTypeAll<EditorWindow>().First(w => w.GetType().Name == "SplineHandleSettingsWindow");
var b = d.worldBound;
var json = "{\"btn\":[" + b.x + "," + b.y + "," + b.width + "," + b.height + "],\"popup\":[" + pw.position.width + "," + pw.position.height + "],\"root\":[" + sv.rootVisualElement.worldBound.y + "]}";
System.IO.File.WriteAllText("OUT", json.Replace(",", ",").Replace(" ", ""));
return json;
