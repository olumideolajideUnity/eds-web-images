using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var t = System.AppDomain.CurrentDomain.GetAssemblies().Select(x => x.GetType("EFDS.EFDSMessageSpecimens")).First(x => x != null);
var a = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().First();
var dock = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(a);
var panes = (System.Collections.IList)dock.GetType().GetField("m_Panes", BF).GetValue(dock);
dock.GetType().GetProperty("selected", BF).SetValue(dock, panes.IndexOf(a));
a.Focus(); a.Repaint();
return "sel=" + dock.GetType().GetProperty("selected", BF).GetValue(dock) + " idx=" + panes.IndexOf(a) + " focused=" + (EditorWindow.focusedWindow == a);
