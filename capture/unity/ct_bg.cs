// Set (or restore) the Scene view background preference color. Edit R,G,B (0-1).
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
float R = 0.255f, G = 0.212f, B = 0.29f;
var BF = BindingFlags.Static|BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var f = typeof(SceneView).GetFields(BF).FirstOrDefault(x => x.FieldType.Name == "PrefColor" && x.Name.ToLower().Contains("background"));
var pc = f.GetValue(null);
var t = pc.GetType();
var col = new Color(R, G, B, 1);
var setColor = t.GetField("m_Color", BF); setColor?.SetValue(pc, col);
var setOpt = t.GetField("m_OptionalOverride", BF);
t.GetMethod("Save", BF, null, System.Type.EmptyTypes, null)?.Invoke(pc, null);
foreach (var sv in Resources.FindObjectsOfTypeAll<SceneView>()) { sv.sceneViewState.showSkybox = false; sv.Repaint(); }
return "field=" + f.Name + " now=" + t.GetProperty("Color", BF)?.GetValue(pc) + " pref=" + EditorPrefs.GetString("Scene/Background", "?");
