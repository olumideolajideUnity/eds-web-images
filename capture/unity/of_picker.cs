// Components > Object field: Unity 7's real object picker (ObjectSelector.Show routes to UnityEditor.Search.SearchPickerWindow), opened as the field's picker button opens it,
// for TYPE, at W x H pt. It's an aux window; it's captured right after opening (EFDS capture helpers don't take focus).
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
string TYPE = "Mesh"; float W = 438, H = 378; string OUT = "C:/tmp/of_picker.png";
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var ost = typeof(Editor).Assembly.GetType("UnityEditor.ObjectSelector");
var os = (EditorWindow)ost.GetProperty("get", BF).GetValue(null);
var t = TYPE == "Mesh" ? typeof(Mesh) : typeof(Material);
var m = ost.GetMethods(BF).First(x => x.Name == "Show" && x.GetParameters().Length == 10 && x.GetParameters()[1].ParameterType == typeof(System.Type));
m.Invoke(os, new object[]{ null, t, null, false, null, null, null, true, true, false });
var spT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("UnityEditor.Search.SearchPickerWindow")).First(x => x != null);
var sp = Resources.FindObjectsOfTypeAll(spT).Cast<EditorWindow>().First();   // Unity 7's object picker is a Unity Search picker window
sp.position = new Rect(600, 200, W, H); sp.Repaint(); os = sp;
var gv = typeof(Editor).Assembly.GetType("UnityEditor.GUIView");
var host = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(os);
for (int i = 0; i < 4; i++) gv.GetMethod("RepaintImmediately", BF)?.Invoke(host, null);   // synchronous: the picker closes itself once it loses focus
{
  var pos = (Rect)gv.GetProperty("position", BF).GetValue(host); float ppp = EditorGUIUtility.pixelsPerPoint;
  int PW = Mathf.RoundToInt(pos.width * ppp), PH = Mathf.RoundToInt(pos.height * ppp);
  var rt = new RenderTexture(PW, PH, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
  gv.GetMethod("GrabPixels", BF).Invoke(host, new object[]{ rt, new Rect(0, 0, PW, PH) });
  var prev = RenderTexture.active; RenderTexture.active = rt;
  var tex = new Texture2D(PW, PH, TextureFormat.RGBA32, false); tex.ReadPixels(new Rect(0, 0, PW, PH), 0, 0); tex.Apply();
  RenderTexture.active = prev; rt.Release();
  var px = tex.GetPixels32(); var fl = new Color32[px.Length];
  for (int y = 0; y < PH; y++) System.Array.Copy(px, y * PW, fl, (PH - 1 - y) * PW, PW);
  tex.SetPixels32(fl); tex.Apply(); System.IO.File.WriteAllBytes(OUT, tex.EncodeToPNG());
}

return "picker " + os.titleContent.text + " " + os.position + " host=" + host?.GetType().Name;
