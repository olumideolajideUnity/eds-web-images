// Errors & messaging: floating UI Builder (W x H pt incl. tab) on a project UXML with a ScrollView; MODE 1 then shows
// UI Builder's real "not in project" notification by asking it to open an unsaved (in-memory) UI Document.
using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection;
int MODE = 0; float W = 800, H = 374;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var t = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetTypes().FirstOrDefault(x => x.Name == "UIBuilderWindow")).First(x => x != null);
var w = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().FirstOrDefault(x => !x.docked);
if (w == null) { w = (EditorWindow)ScriptableObject.CreateInstance(t); w.Show(); }
w.position = new Rect(120, 120, W, H);
var open = t.GetMethod("TryOpenProjectDocument", BF);
if (MODE == 0) {
  AssetDatabase.ImportAsset("Assets/EFDS_Screenshots/UI/EFDSBanner.uxml");
  var vta = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/EFDS_Screenshots/UI/EFDSBanner.uxml");
  var r = open.Invoke(w, new object[]{ vta });
  w.Repaint(); return "opened=" + r + " type=" + t.FullName;
}
var r2 = open.Invoke(w, new object[]{ ScriptableObject.CreateInstance<VisualTreeAsset>() });
w.Repaint(); return "notification shown, open=" + r2;
