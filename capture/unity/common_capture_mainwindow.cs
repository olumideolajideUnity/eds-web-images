// Composite the main window from its own GUIViews (toolbar, dock areas, status bar), each grabbed from its buffer and placed
// at its screen position; writes OUT (PNG) and OUT.json (window rect, pt). No screen read.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection; using System.Collections.Generic;
string OUT = "C:/tmp/main.png";
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var asm = typeof(Editor).Assembly; var gv = asm.GetType("UnityEditor.GUIView"); var cwT = asm.GetType("UnityEditor.ContainerWindow");
var main = Resources.FindObjectsOfTypeAll(cwT).First(c => (int)cwT.GetProperty("showMode", BF).GetValue(c) == 4);
var mpos = (Rect)cwT.GetProperty("position", BF).GetValue(main);
var views = Resources.FindObjectsOfTypeAll(gv).Where(v => gv.GetProperty("window", BF).GetValue(v) == main).ToList();
float ppp = EditorGUIUtility.pixelsPerPoint;
int frames = 0; EditorApplication.CallbackFunction cb = null;
cb = () => {
  foreach (var v in views) gv.GetMethod("RepaintImmediately", BF)?.Invoke(v, null);
  if (++frames < 4) return;
  EditorApplication.update -= cb;
  int W = Mathf.RoundToInt(mpos.width * ppp), H = Mathf.RoundToInt(mpos.height * ppp);
  var outTex = new Texture2D(W, H, TextureFormat.RGBA32, false); var clear = new Color32[W * H]; outTex.SetPixels32(clear);
  var log = new List<string>();
  foreach (var v in views) {
    var sp = (Rect)gv.GetProperty("screenPosition", BF).GetValue(v);
    int w = Mathf.RoundToInt(sp.width * ppp), h = Mathf.RoundToInt(sp.height * ppp); if (w <= 0 || h <= 0) continue;
    var rt = new RenderTexture(w, h, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
    gv.GetMethod("GrabPixels", BF).Invoke(v, new object[]{ rt, new Rect(0, 0, w, h) });
    var prev = RenderTexture.active; RenderTexture.active = rt;
    var t = new Texture2D(w, h, TextureFormat.RGBA32, false); t.ReadPixels(new Rect(0, 0, w, h), 0, 0); t.Apply(); RenderTexture.active = prev; rt.Release();
    int ox = Mathf.RoundToInt((sp.x - mpos.x) * ppp), oyTop = Mathf.RoundToInt((sp.y - mpos.y) * ppp);
    var px = t.GetPixels32();
    for (int y = 0; y < h; y++) { int dy = H - 1 - (oyTop + y); if (dy < 0 || dy >= H) continue;
      for (int x = 0; x < w; x++) { int dx = ox + x; if (dx < 0 || dx >= W) continue; var c = px[y * w + x]; c.a = 255; outTex.SetPixel(dx, dy, c); } }
    log.Add(v.GetType().Name + " " + sp);
  }
  outTex.Apply(); System.IO.File.WriteAllBytes(OUT, outTex.EncodeToPNG());
  System.IO.File.WriteAllText(OUT + ".json", "{\"window\":[" + mpos.x + "," + mpos.y + "," + mpos.width + "," + mpos.height + "]}");
};
EditorApplication.update += cb;
return "main " + mpos + " views=" + views.Count;
