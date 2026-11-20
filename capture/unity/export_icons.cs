// Export Editor icons (largest variant) by IconContent name to DIR/<name>.png.
using UnityEditor; using UnityEngine; using System.Linq;
string DIR = "C:/tmp"; string[] NAMES = { "d_Prefab Icon", "d_Material Icon" };
string r = "";
foreach (var n in NAMES) {
  Texture2D best = null;
  foreach (var v in new[]{ n + "@2x", n }) { var t = EditorGUIUtility.IconContent(v)?.image as Texture2D; if (t != null && (best == null || t.width > best.width)) best = t; }
  if (best == null) { r += n + "=null; "; continue; }
  var rt = RenderTexture.GetTemporary(best.width, best.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
  Graphics.Blit(best, rt); var prev = RenderTexture.active; RenderTexture.active = rt;
  var c = new Texture2D(best.width, best.height, TextureFormat.RGBA32, false); c.ReadPixels(new Rect(0, 0, best.width, best.height), 0, 0); c.Apply();
  RenderTexture.active = prev; RenderTexture.ReleaseTemporary(rt);
  System.IO.File.WriteAllBytes(DIR + "/icon_" + n.Replace(" ", "_") + ".png", c.EncodeToPNG()); r += n + " " + best.width + "; ";
}
return r;
