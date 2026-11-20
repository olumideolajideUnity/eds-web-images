// Capture a window's whole dock area (any window not in the main window, tabbed or not) (tab strip + content) from Unity's own view buffer (no screen read).
// Edit MATCH (window title or type name) and OUT.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
string MATCH = "Inspector"; string OUT = "C:/tmp/out.png";
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var cwT = typeof(Editor).Assembly.GetType("UnityEditor.ContainerWindow");
bool NotMain(EditorWindow x) { var p = typeof(EditorWindow).GetField("m_Parent", BindingFlags.Instance|BindingFlags.NonPublic).GetValue(x); var c = p?.GetType().GetProperty("window", BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic)?.GetValue(p); return c != null && (int)cwT.GetProperty("showMode", BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic).GetValue(c) != 4; }
var w = Resources.FindObjectsOfTypeAll<EditorWindow>().Where(x => NotMain(x) && (x.titleContent.text == MATCH || x.GetType().Name == MATCH)).OrderByDescending(x => x.position.width).First();
var host = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(w);
var gv = typeof(Editor).Assembly.GetType("UnityEditor.GUIView");
var pos = (Rect)gv.GetProperty("position", BF).GetValue(host);          // in container coordinates (points)
float ppp = EditorGUIUtility.pixelsPerPoint;
w.Focus(); w.Repaint();
int W = Mathf.RoundToInt(pos.width * ppp), H = Mathf.RoundToInt(pos.height * ppp);
int frames = 0;
EditorApplication.CallbackFunction cb = null;
cb = () => {
  if (++frames < 3) { gv.GetMethod("RepaintImmediately", BF)?.Invoke(host, null); return; }
  EditorApplication.update -= cb;
  gv.GetMethod("RepaintImmediately", BF)?.Invoke(host, null);
  var rt = new RenderTexture(W, H, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
  gv.GetMethod("GrabPixels", BF).Invoke(host, new object[]{ rt, new Rect(0, 0, W, H) });
  var prev = RenderTexture.active; RenderTexture.active = rt;
  var tex = new Texture2D(W, H, TextureFormat.RGBA32, false); tex.ReadPixels(new Rect(0, 0, W, H), 0, 0); tex.Apply();
  RenderTexture.active = prev; rt.Release();
  var px = tex.GetPixels32(); var fl = new Color32[px.Length];          // render textures are bottom-up: flip rows
  for (int y = 0; y < H; y++) System.Array.Copy(px, y * W, fl, (H - 1 - y) * W, W);
  tex.SetPixels32(fl); tex.Apply();
  System.IO.File.WriteAllBytes(OUT, tex.EncodeToPNG());
};
EditorApplication.update += cb;
return "host=" + host.GetType().Name + " pos=" + pos + " px=" + W + "x" + H + " title=" + w.titleContent.text;
