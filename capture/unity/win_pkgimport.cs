// Windowing modal-800: size the open "Import Unity Package" window (opened by unity/win_import.cs) to 350 x 140 pt.
using UnityEditor; using UnityEngine; using System.Linq;
var w = Resources.FindObjectsOfTypeAll<EditorWindow>().FirstOrDefault(x => x.GetType().Name == "PackageImport");
if (w == null) return "none";
w.minSize = new Vector2(100, 100); w.position = new Rect(300, 200, 350, 140); w.Repaint();   // dark capture: 350 x 140 pt
return "pi " + w.titleContent.text + " " + w.position + " docked=" + w.docked;
