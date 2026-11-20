using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var t = typeof(Editor).Assembly.GetType("UnityEditor.AnnotationWindow");
var ws = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().ToList();
return "n=" + ws.Count + " " + string.Join(";", ws.Select(w => w.position + " title='" + w.titleContent.text + "' parent=" + typeof(EditorWindow).GetField("m_Parent", BF).GetValue(w)?.GetType().Name));
