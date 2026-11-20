// Color palette > Window borders: floating two-column Project browser showing Assets/Scenes, original's 647x273 pt.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
float W = 647, H = 273; string FOLDER = "Assets/Scenes";
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var t = typeof(Editor).Assembly.GetType("UnityEditor.ProjectBrowser");
var w = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().FirstOrDefault(x => !x.docked);
if (w == null) { w = (EditorWindow)ScriptableObject.CreateInstance(t); w.Show(); }
w.position = new Rect(200, 100, W, H);
var vmT = t.GetNestedType("ViewMode", BF);
var sv = t.GetMethod("SetViewMode", BF);
if (sv != null) sv.Invoke(w, new object[]{ System.Enum.Parse(vmT, "TwoColumns") });
var f = AssetDatabase.LoadMainAssetAtPath(FOLDER);
var m = t.GetMethods(BF).Where(x => x.Name == "ShowFolderContents").OrderBy(x => x.GetParameters().Length).ToList();
var mm = m.Last(); var ps = mm.GetParameters();
object id = ps[0].ParameterType == typeof(int) ? (object)(int)typeof(Object).GetMethod("GetInstanceID").Invoke(f, null) : (object)f.GetEntityId();
mm.Invoke(w, ps.Length == 2 ? new object[]{ id, true } : new object[]{ id });
Selection.activeObject = null;
var la = t.GetField("m_ListArea", BF)?.GetValue(w);
string gs = "";
if (la != null) { var p = la.GetType().GetProperty("gridSize", BF); if (p != null) { p.SetValue(la, 64); gs = "grid=" + p.GetValue(la); } }
var dw = t.GetField("m_DirectoriesAreaWidth", BF); if (dw != null) dw.SetValue(w, 195f);
w.Repaint(); return "vm=" + (sv != null) + " " + gs + " dw=" + (dw != null);
