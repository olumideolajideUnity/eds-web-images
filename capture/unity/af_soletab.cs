// Close every other tab in the dock area that holds the window whose type name is TYPE, so it stands alone.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection; using System.Collections.Generic;
string TYPE = "MaterialGraphEditWindow";
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var w = Resources.FindObjectsOfTypeAll<EditorWindow>().First(x => x.GetType().Name == TYPE);
var parent = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(w);
var panes = ((IEnumerable<EditorWindow>)parent.GetType().GetField("m_Panes", BF).GetValue(parent)).ToList();
var closed = new List<string>();
foreach (var p in panes) if (p != w) { closed.Add(p.GetType().Name); p.Close(); }
w.Focus(); return "closed " + string.Join(",", closed) + " pos=" + w.position;
