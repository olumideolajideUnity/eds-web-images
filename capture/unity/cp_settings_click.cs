// Click (x, y) in the Project Settings window through its own GUI event path; returns the async shader compilation value.
using UnityEditor; using UnityEngine; using System.Linq;
float X = 300, Y = 111; int N = 1;
var t = typeof(Editor).Assembly.GetType("UnityEditor.ProjectSettingsWindow");
var w = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().First(x => !x.docked);
w.Focus();
var before = EditorSettings.asyncShaderCompilation;
for (int i = 0; i < N; i++) {
  w.SendEvent(new Event { type = EventType.MouseDown, mousePosition = new Vector2(X, Y), button = 0, clickCount = 1 });
  w.SendEvent(new Event { type = EventType.MouseUp, mousePosition = new Vector2(X, Y), button = 0, clickCount = 1 });
}
w.Repaint();
return "async " + before + " -> " + EditorSettings.asyncShaderCompilation;
