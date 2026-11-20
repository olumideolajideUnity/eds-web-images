using UnityEditor; using UnityEngine; using System.Linq;
string[] names = { "d_UnityLogo", "UnityLogo", "d_SceneAsset Icon", "BuildSettings.Editor" };
foreach (var n in names) {
  var t = EditorGUIUtility.IconContent(n)?.image as Texture2D; if (t == null) continue;
  var rt = RenderTexture.GetTemporary(t.width, t.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
  Graphics.Blit(t, rt); var prev = RenderTexture.active; RenderTexture.active = rt;
  var o = new Texture2D(t.width, t.height, TextureFormat.RGBA32, false); o.ReadPixels(new Rect(0, 0, t.width, t.height), 0, 0); o.Apply();
  RenderTexture.active = prev; RenderTexture.ReleaseTemporary(rt);
  System.IO.File.WriteAllBytes("OUTDIR/unity_logo.png", o.EncodeToPNG());
  return n + " " + t.width + "x" + t.height;
}
return "none";
