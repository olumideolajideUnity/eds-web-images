// Toolbar page: floating Timeline window (W x H pt) with Scene and Game tabs docked beside it, showing an empty in-memory TimelineAsset
// (so both Timeline toolbar rows show and the track area is empty, like the original). Nothing is saved.
using UnityEditor; using UnityEditor.Timeline; using UnityEngine; using UnityEngine.Timeline; using System.Linq; using System.Reflection;
float W = 520, H = 300;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var tType = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("UnityEditor.Timeline.TimelineWindow")).First(t => t != null);
foreach (var old in Resources.FindObjectsOfTypeAll(tType).Cast<EditorWindow>().ToList()) old.Close();
var tw = (EditorWindow)ScriptableObject.CreateInstance(tType); tw.Show();
var dock = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(tw);
var add = dock.GetType().GetMethods(BF).First(m => m.Name == "AddTab" && m.GetParameters().Length >= 1 && m.GetParameters()[0].ParameterType == typeof(EditorWindow));
foreach (var t in new[]{ typeof(SceneView), typeof(Editor).Assembly.GetType("UnityEditor.GameView") }) {
  var e = (EditorWindow)ScriptableObject.CreateInstance(t); var ps = add.GetParameters(); var a = new object[ps.Length]; a[0] = e;
  for (int i = 1; i < ps.Length; i++) a[i] = ps[i].HasDefaultValue ? ps[i].DefaultValue : (ps[i].ParameterType == typeof(bool) ? (object)true : null); add.Invoke(dock, a); }
var cw = dock.GetType().GetProperty("window", BF).GetValue(dock); cw.GetType().GetProperty("position", BF).SetValue(cw, new Rect(120, 140, W, H));
var asset = ScriptableObject.CreateInstance<TimelineAsset>(); asset.name = "Timeline"; asset.hideFlags = HideFlags.DontSave;
((TimelineEditorWindow)tw).SetTimeline(asset);
tw.Focus(); tw.Repaint();
return "tabs ok " + tw.position;
