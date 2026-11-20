using UnityEditor; using UnityEngine; using System.Linq;
string PAGE = "Project/Services";
var w = SettingsService.OpenProjectSettings(PAGE);
var ps = Resources.FindObjectsOfTypeAll<EditorWindow>().First(x => x.GetType().Name == "ProjectSettingsWindow");
ps.position = new Rect(150, 150, 372, 195);
ps.Repaint();
return "docked=" + ps.docked + " " + ps.position;
