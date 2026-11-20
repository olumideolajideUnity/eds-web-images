using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection; using System.Collections.Generic;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
string OUT = "OUTPNG";
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
VisualElement d = null; var st = new Stack<VisualElement>(); st.Push(sv.rootVisualElement.panel.visualTree);
while (st.Count > 0) { var e = st.Pop(); if (e.GetType().Name == "SplineHandleSettingsDropdown") { d = e; break; } foreach (var c in e.Children()) st.Push(c); }
d.GetType().GetMethod("OnClick", BF).Invoke(d, null);
var gv = typeof(Editor).Assembly.GetType("UnityEditor.GUIView");
int frames = 0; EditorApplication.CallbackFunction cb = null;
cb = () => {
  var pw = Resources.FindObjectsOfTypeAll<EditorWindow>().FirstOrDefault(w => w.GetType().Name == "SplineHandleSettingsWindow");
  if (pw == null) { EditorApplication.update -= cb; System.IO.File.WriteAllText(OUT + ".err", "closed at frame " + frames); return; }
  var host = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(pw);
  gv.GetMethod("RepaintImmediately", BF)?.Invoke(host, null);
  if (++frames < 4) return;
  EditorApplication.update -= cb;
  var pos = (Rect)gv.GetProperty("position", BF).GetValue(host); float ppp = EditorGUIUtility.pixelsPerPoint;
  int W = Mathf.RoundToInt(pos.width * ppp), H = Mathf.RoundToInt(pos.height * ppp);
  var rt = new RenderTexture(W, H, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
  gv.GetMethod("GrabPixels", BF).Invoke(host, new object[]{ rt, new Rect(0, 0, W, H) });
  var prev = RenderTexture.active; RenderTexture.active = rt;
  var tex = new Texture2D(W, H, TextureFormat.RGBA32, false); tex.ReadPixels(new Rect(0, 0, W, H), 0, 0); tex.Apply();
  RenderTexture.active = prev; rt.Release();
  var px = tex.GetPixels32(); var fl = new Color32[px.Length];
  for (int y = 0; y < H; y++) System.Array.Copy(px, y * W, fl, (H - 1 - y) * W, W);
  tex.SetPixels32(fl); tex.Apply();
  System.IO.File.WriteAllBytes(OUT, tex.EncodeToPNG());
};
EditorApplication.update += cb;
return "opened";
