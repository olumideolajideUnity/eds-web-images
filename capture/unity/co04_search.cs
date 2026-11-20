using UnityEditor; using UnityEditor.Search; using UnityEngine; using System.Linq;
foreach (var w in Resources.FindObjectsOfTypeAll<EditorWindow>().Where(w => w.GetType().Name.Contains("SearchWindow")).ToList()) w.Close();
var ctx = SearchService.CreateContext("test");
var view = SearchService.ShowWindow(ctx, "Search", 720, 290, false, false, false, true);
var win = Resources.FindObjectsOfTypeAll<EditorWindow>().First(w => w.GetType().Name.Contains("SearchWindow"));
win.position = new Rect(200, 140, 720, 290);
return win.GetType().FullName + " docked=" + win.docked + " " + win.position;
