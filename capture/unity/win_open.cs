// Open a floating EditorWindow by full type name at W x H pt.
using UnityEditor; using UnityEngine; using System.Linq;
string TYPE = "UnityEditor.ShortcutManagement.ShortcutManagerWindow"; float W = 700, H = 260;
var t = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType(TYPE)).First(x => x != null);
var w = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().FirstOrDefault(x => !x.docked);
if (w == null) { w = (EditorWindow)ScriptableObject.CreateInstance(t); w.Show(); }
w.minSize = new Vector2(100, 100); w.position = new Rect(150, 120, W, H); w.Repaint();
return w.titleContent.text + " " + w.position;
