using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Collections.Generic;
UnityEditor.PackageManager.UI.Window.Open("");
var pmt = Resources.FindObjectsOfTypeAll<EditorWindow>().First(x => x.GetType().Name == "PackageManagerWindow").GetType();
pmt.GetMethod("OpenAndSelectPage", System.Reflection.BindingFlags.Static|System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic)?.Invoke(null, new object[]{ "UnityRegistry", "" });
var w = Resources.FindObjectsOfTypeAll<EditorWindow>().First(x => x.GetType().Name == "PackageManagerWindow");
if (w.docked) { var c = ScriptableObject.CreateInstance(w.GetType()); }
w.position = new Rect(150, 150, 1000, 300);
// type the search text into the toolbar search field
var fields = new List<TextField>(); var st = new Stack<VisualElement>(); st.Push(w.rootVisualElement);
while (st.Count > 0) { var e = st.Pop(); if (e is TextField tf) fields.Add(tf); foreach (var ch in e.Children()) st.Push(ch); }
var sf = fields.FirstOrDefault(f => f.parent != null && f.parent.GetType().Name.Contains("Search")) ?? fields.FirstOrDefault();
if (sf != null) sf.value = "Package name";
w.Repaint();
return "docked=" + w.docked + " pos=" + w.position + " fields=" + fields.Count + " set=" + (sf != null);
