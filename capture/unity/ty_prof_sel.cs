// Profiler: select frame (last - SEL) and refresh the charts.
using UnityEditor; using UnityEditorInternal; using UnityEngine; using System.Linq; using System.Reflection;
int SEL = 150;
var w = Resources.FindObjectsOfTypeAll<ProfilerWindow>().First(x => !x.docked);
w.selectedFrameIndex = ProfilerDriver.lastFrameIndex - SEL; w.Repaint();
return "sel=" + w.selectedFrameIndex;
