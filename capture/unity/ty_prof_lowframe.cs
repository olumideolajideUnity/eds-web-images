// Profiler: among frames (last - HI) .. (last - LO), return the offset from the newest frame of the one with the lowest CPU frame
// time (so its value label sits low on the chart, like the dark capture's 6.53ms). Use the result as ty_prof_sel.cs SEL.
using UnityEditorInternal; using UnityEngine; using System.Linq;
int LO = 80, HI = 160;
int last = ProfilerDriver.lastFrameIndex; int best = -1; float bt = float.MaxValue;
for (int f = last - HI; f <= last - LO; f++) {
  using (var v = ProfilerDriver.GetRawFrameDataView(f, 0)) { if (v == null || !v.valid) continue; if (v.frameTimeMs < bt) { bt = v.frameTimeMs; best = f; } } }
return "SEL=" + (last - best) + " ms=" + bt.ToString("0.00");
