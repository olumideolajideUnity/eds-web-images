// Open (floating) Preferences or Project Settings at PAGE, sized W x H pt (incl. tab).
using UnityEditor; using UnityEngine; using System.Linq;
string KIND = "Project"; string PAGE = "Project/Memory Settings"; float W = 800, H = 330;
EditorWindow w;
if (KIND == "Project") { SettingsService.OpenProjectSettings(PAGE); w = Resources.FindObjectsOfTypeAll<EditorWindow>().First(x => x.GetType().Name == "ProjectSettingsWindow"); }
else { SettingsService.OpenUserPreferences(PAGE); w = Resources.FindObjectsOfTypeAll<EditorWindow>().First(x => x.GetType().Name == "PreferenceSettingsWindow"); }
w.position = new Rect(120, 120, W, H); w.Repaint();
return w.GetType().Name + " docked=" + w.docked + " " + w.position;
