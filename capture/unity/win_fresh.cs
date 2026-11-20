// Replace the floating Scene view with a fresh one (default overlay layout), floating at W x H content.
using UnityEditor; using UnityEditor.Overlays; using UnityEngine; using System.Linq; using System.Reflection;
float W = 709, H = 300;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
foreach (var s in Resources.FindObjectsOfTypeAll<SceneView>().Where(w => !w.docked).ToList()) s.Close();
var sv = ScriptableObject.CreateInstance<SceneView>(); sv.Show();
var parent = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(sv);
var container = parent.GetType().GetProperty("window", BF).GetValue(parent);
container.GetType().GetProperty("position", BF).SetValue(container, new Rect(100, 100, W, H + 24 + 26));
var am = typeof(OverlayCanvas).GetMethods(BF).Where(x => x.Name == "ApplyPreset").Select(x => string.Join(",", x.GetParameters().Select(p => p.ParameterType.Name)));
return "presetSig=" + string.Join("|", am) + " last=" + typeof(OverlayCanvas).GetProperty("lastAppliedPresetName", BF)?.GetValue(sv.overlayCanvas) + " shown=" + string.Join(",", sv.overlayCanvas.overlays.Where(o => o.displayed).Select(o => o.displayName));
