// Show UI Builder's real "not in project" notification (asks it to open an unsaved UI Document) and grab the window a few frames later.
using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection;
string MATCH = "UIBuilderWindow"; string OUT = "C:/tmp/out.png";
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var bt = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetTypes().FirstOrDefault(x => x.Name == "UIBuilderWindow")).First(x => x != null);
var bw = Resources.FindObjectsOfTypeAll(bt).Cast<EditorWindow>().First(x => !x.docked);
bw.Focus();
bt.GetMethod("TryOpenProjectDocument", BF).Invoke(bw, new object[]{ ScriptableObject.CreateInstance<VisualTreeAsset>() });
var w = Resources.FindObjectsOfTypeAll<EditorWindow>().Where(x => !x.docked && (x.titleContent.text == MATCH || x.GetType().Name == MATCH)).OrderByDescending(x => x.position.width).First();
var host = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(w);
var gv = typeof(Editor).Assembly.GetType("UnityEditor.GUIView");
var pos = (Rect)gv.GetProperty("position", BF).GetValue(host);          // in container coordinates (points)
float ppp = EditorGUIUtility.pixelsPerPoint;
w.Repaint();
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
