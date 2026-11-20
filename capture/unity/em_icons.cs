// Export Unity's own console icons (largest variant available) as PNGs.
using UnityEditor; using UnityEngine; using System.Linq;
string DIR = "C:/tmp";
string r = "";
foreach (var n in new[]{ "console.erroricon", "console.warnicon", "console.infoicon" }) {
  Texture2D best = null;
  foreach (var name in new[]{ "d_" + n + "@2x", n + "@2x", "d_" + n, n }) { var t = EditorGUIUtility.Load("icons/" + name + ".png") as Texture2D ?? EditorGUIUtility.IconContent(name).image as Texture2D; if (t != null && (best == null || t.width > best.width)) best = t; }
  var rt = RenderTexture.GetTemporary(best.width, best.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
  Graphics.Blit(best, rt); var prev = RenderTexture.active; RenderTexture.active = rt;
  var c = new Texture2D(best.width, best.height, TextureFormat.RGBA32, false); c.ReadPixels(new Rect(0, 0, best.width, best.height), 0, 0); c.Apply();
  RenderTexture.active = prev; RenderTexture.ReleaseTemporary(rt);
  System.IO.File.WriteAllBytes(DIR + "/" + n + ".png", c.EncodeToPNG()); r += best.name + " " + best.width + "; ";
}
return r;
