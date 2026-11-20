// Unity's own Prefab override comparison popup (PrefabOverridesTreeView.ComparisonViewPopup) for the Cube instance's
// Transform override, hosted in EFDS.EFDSPopupHost (real popups close when Unity loses focus). Size W x H pt.
using UnityEditor; using UnityEditor.SceneManagement; using UnityEngine; using System.Linq; using System.Reflection;
float W = 0, H = 0;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
if (EditorSceneManager.GetActiveScene().path != "Assets/EFDS_Screenshots/Inheritance.unity") EditorSceneManager.OpenScene("Assets/EFDS_Screenshots/Inheritance.unity");
var go = GameObject.Find("Cube instance") ?? GameObject.Find("Cube");
var tr = go.transform; var src = PrefabUtility.GetCorrespondingObjectFromSource(tr);
var ov = PrefabUtility.GetObjectOverrides(go).First(o => o.instanceObject == tr);
var asm = typeof(Editor).Assembly;
var popT = asm.GetType("UnityEditor.PrefabOverridesTreeView+ComparisonViewPopup");
var pop = (PopupWindowContent)System.Activator.CreateInstance(popT, BF, null, new object[]{ src, tr, ov, null }, null);
var hostT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSPopupHost")).First(x => x != null);
var host = (EditorWindow)hostT.GetMethod("Open").Invoke(null, new object[]{ pop });
var sz = pop.GetWindowSize();
host.position = new Rect(400, 300, W > 0 ? W : sz.x, H > 0 ? H : sz.y);
host.Repaint();
return "size=" + sz + " pos=" + host.position;
