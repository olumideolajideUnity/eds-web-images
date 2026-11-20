using UnityEditor; using UnityEngine; using UnityEngine.Playables; using UnityEngine.Timeline; using System.Linq;
var dir = Object.FindObjectsByType<PlayableDirector>(FindObjectsSortMode.None).First(d => d.playableAsset != null && d.playableAsset.name == "GameplaySequence");
var tl = (TimelineAsset)dir.playableAsset;
return string.Join("\n", tl.GetOutputTracks().Where(t => t.name.Contains("Character2")).Select(t => {
  var b = dir.GetGenericBinding(t);
  string info = b == null ? "UNBOUND" : (b.GetType().Name + " '" + b.name + "'");
  if (b is Animator a) info += " avatar=" + (a.avatar ? a.avatar.name : "none") + " valid=" + (a.avatar && a.avatar.isValid) + " ctrl=" + (a.runtimeAnimatorController ? "yes" : "no");
  if (b is AudioSource s) info += " clip=" + (s.clip ? s.clip.name : "none") + " enabled=" + s.enabled;
  return t.GetType().Name + " " + t.name + " -> " + info;
}));
