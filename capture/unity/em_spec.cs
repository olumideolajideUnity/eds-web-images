// Errors & messaging specimens: open EFDS.EFDSMessageSpecimens (MODE 0 help boxes, 1 buttons) at W x H, report control rects.
using UnityEditor; using UnityEngine; using System.Linq;
int MODE = 0; float W = 380, H = 160;
var t = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSMessageSpecimens")).First(x => x != null);
var w = (EditorWindow)t.GetMethod("Open").Invoke(null, new object[]{ MODE, W, H });
w.Repaint();
return "opened " + w.position;
