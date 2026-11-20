// Read IMGUI button style metrics (height, padding, margin, font) for the Button page's size and layout callouts.
using UnityEditor; using UnityEngine; using System.Linq;
string S(string n) { GUIStyle s = null; try { s = new GUIStyle(n); } catch { } if (s == null || s.name == "") return n + "=missing"; var h = s.CalcHeight(new GUIContent("Mini Button"), 200); return n + ": fixedH=" + s.fixedHeight + " calcH=" + h + " pad=" + s.padding + " margin=" + s.margin + " font=" + s.fontSize + " border=" + s.border; }
System.IO.File.WriteAllText("C:/tmp/btn.txt", string.Join("\n", new[]{ "miniButton", "button", "LargeButton", "AC Button", "toolbarbutton", "ButtonLeft", "ButtonMid", "ButtonRight" }.Select(S)));
return "ok";
