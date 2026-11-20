// Export Unity 7's own logo textures from the Editor resource bundle as PNGs (readable copies).
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
string DIR = "C:/tmp";
var b = (AssetBundle)typeof(EditorGUIUtility).GetMethod("GetEditorAssetBundle", BindingFlags.Static|BindingFlags.NonPublic|BindingFlags.Public).Invoke(null, null);
string r = "";
foreach (var n in new[]{ "icons/branding/d_unityproduct@2x.png", "icons/branding/unityproduct@2x.png", "icons/d_unitylogo.png", "icons/unitylogo.png", "icons/branding/unityaboutlogo@2x.png" }) {
  var t = b.LoadAsset<Texture2D>(n); if (t == null) { r += n + "=null; "; continue; }
  var rt = RenderTexture.GetTemporary(t.width, t.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
  Graphics.Blit(t, rt); var prev = RenderTexture.active; RenderTexture.active = rt;
  var c = new Texture2D(t.width, t.height, TextureFormat.RGBA32, false); c.ReadPixels(new Rect(0, 0, t.width, t.height), 0, 0); c.Apply();
  RenderTexture.active = prev; RenderTexture.ReleaseTemporary(rt);
  System.IO.File.WriteAllBytes(DIR + "/logo_" + System.IO.Path.GetFileNameWithoutExtension(n).Replace("@", "_") + ".png", c.EncodeToPNG()); r += n + " " + t.width + "x" + t.height + "; ";
}
return r;
