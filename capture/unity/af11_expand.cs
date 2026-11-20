using UnityEditor; using UnityEngine; using UnityEngine.Playables; using UnityEngine.Timeline; using System.Linq; using System.Reflection;
var BF = BindingFlags.Static|BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var asm = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "Unity.Timeline.Editor");
var ext = asm.GetType("UnityEditor.Timeline.TrackExtensions");
var setCollapsed = ext.GetMethod("SetCollapsed", BF);
var dir = Object.FindObjectsByType<PlayableDirector>(FindObjectsSortMode.None).First(d => d.playableAsset != null && d.playableAsset.name == "GameplaySequence");
var tl = (TimelineAsset)dir.playableAsset;
var expand = new[]{ "Cameras", "Characters", "Audio" };
foreach (var t in tl.GetRootTracks()) setCollapsed.Invoke(null, new object[]{ t, !expand.Contains(t.name) });
UnityEditor.Timeline.TimelineEditor.Refresh(UnityEditor.Timeline.RefreshReason.ContentsAddedOrRemoved);
// scroll the track list so Cameras is the first visible group
var w = Resources.FindObjectsOfTypeAll(asm.GetType("UnityEditor.Timeline.TimelineWindow")).Cast<EditorWindow>().First();
var tv = w.GetType().GetProperty("treeView", BF)?.GetValue(w) ?? w.GetType().GetField("m_TreeView", BF)?.GetValue(w);
string scroll = "no treeView";
if (tv != null) {
  var sp = tv.GetType().GetProperty("scrollPosition", BF);
  if (sp != null) { sp.SetValue(tv, new Vector2(0, 52)); scroll = "scrolled to " + sp.GetValue(tv); }
  else scroll = "treeView props: " + string.Join(",", tv.GetType().GetProperties(BF).Select(p => p.Name).Where(n => n.ToLower().Contains("scroll")));
}
w.Repaint();
return "tracks: " + string.Join(",", tl.GetRootTracks().Select(t => t.name)) + " | " + scroll;
