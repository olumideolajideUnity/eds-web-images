// Capture any GUIView (by type name, e.g. UnityEditor.Toolbar / UnityEditor.AppStatusBar) from its own buffer to OUT.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
string VIEW = "UnityEditor.Toolbar"; string TYPE = ""; string MSG = ""; string OUT = "C:/tmp/sb.png";
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
if (TYPE == "Log") Debug.Log(MSG); else if (TYPE == "Warning") Debug.LogWarning(MSG); else if (TYPE == "Error") Debug.LogError(MSG);
var asm = typeof(Editor).Assembly;
var sbT = asm.GetType(VIEW);
var host = Resources.FindObjectsOfTypeAll(sbT).First();
var gv = asm.GetType("UnityEditor.GUIView");
int frames = 0; EditorApplication.CallbackFunction cb = null;
cb = () => {
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
  tex.SetPixels32(fl); tex.Apply(); System.IO.File.WriteAllBytes(OUT, tex.EncodeToPNG());
};
EditorApplication.update += cb;
return "statusbar " + gv.GetProperty("position", BF).GetValue(host);
