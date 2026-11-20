using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var a = System.AppDomain.CurrentDomain.GetAssemblies().First(x => x.GetName().Name == "Unity.ProBuilder.Editor");
var act = System.Activator.CreateInstance(a.GetType("UnityEditor.ProBuilder.Actions.BevelEdges"));
act.GetType().GetMethod("PerformAltAction", BF).Invoke(act, null);
foreach (var w in Resources.FindObjectsOfTypeAll<EditorWindow>().Where(w => w.GetType().Name == "MenuOption").ToList()) w.Close();
SceneView.RepaintAll();
return "primed";
