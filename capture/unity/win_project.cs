// Floating Project browser (W x H pt incl. tab), two-column, showing FOLDER's contents with it selected in the folder tree.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
float W = 620, H = 440; string FOLDER = "Assets/Atrio/Environment";
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var t = typeof(Editor).Assembly.GetType("UnityEditor.ProjectBrowser");
var w = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().FirstOrDefault(x => !x.docked);
if (w == null) { w = (EditorWindow)ScriptableObject.CreateInstance(t); w.Show(); }
w.position = new Rect(200, 100, W, H);
var f = AssetDatabase.LoadMainAssetAtPath(FOLDER);
var m = t.GetMethods(BF).Where(x => x.Name == "ShowFolderContents").OrderBy(x => x.GetParameters().Length).ToList();
var sig = string.Join("|", m.Select(x => string.Join(",", x.GetParameters().Select(p => p.ParameterType.Name))));
var mm = m.Last(); var ps = mm.GetParameters();
object id = ps[0].ParameterType == typeof(int) ? (object)(int)typeof(Object).GetMethod("GetInstanceID").Invoke(f, null) : (object)f.GetEntityId();
mm.Invoke(w, ps.Length == 2 ? new object[]{ id, true } : new object[]{ id });
w.Repaint(); return "sig=" + sig;
