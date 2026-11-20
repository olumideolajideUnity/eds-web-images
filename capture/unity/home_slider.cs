// UI Builder on EFDSSlider.uxml (floating W x H) with its Slider selected through UIBuilderWindow.RequestSelection.
using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection; using System.Collections.Generic;
int MODE = 0; float W = 900, H = 480;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var t = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetTypes().FirstOrDefault(x => x.Name == "UIBuilderWindow")).First(x => x != null);
var w = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().FirstOrDefault(x => !x.docked);
if (w == null) { w = (EditorWindow)ScriptableObject.CreateInstance(t); w.Show(); }
w.position = new Rect(100, 100, W, H);
string P = "Assets/EFDS_Screenshots/UI/EFDSSlider.uxml"; AssetDatabase.ImportAsset(P);
var vta = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(P);
if (MODE == 0) { t.GetMethod("TryOpenProjectDocument", BF).Invoke(w, new object[]{ vta }); return "opened"; }
var found = (System.Collections.IEnumerable)typeof(VisualTreeAsset).GetMethod("FindElementsByName", BF).Invoke(vta, new object[]{ "FieldOfView" });
var slider = found.Cast<object>().First();
var idm = (object)slider.GetType().GetProperty("id", BF)?.GetValue(slider) ?? slider.GetType().GetField("m_Id", BF)?.GetValue(slider); int id = System.Convert.ToInt32(idm);
t.GetMethod("RequestSelection", BF).Invoke(w, new object[]{ id, new List<int>(), vta });
var zooms = w.rootVisualElement.Query<VisualElement>().ToList().SelectMany(e => e.GetType().GetMembers(BF).Where(m => m.Name.ToLower().Contains("zoom")).Select(m => e.GetType().Name + "." + m.Name)).Distinct().Take(20);
w.Repaint(); return "selected id=" + id + " zoom=" + string.Join(",", zooms);
