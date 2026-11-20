using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var sv = SceneView.lastActiveSceneView; var st = sv.sceneViewState;
return "fx=" + st.fxEnabled + " sky=" + st.showSkybox + " all=" + st.alwaysRefreshEnabled + " fog=" + st.showFog + " sun=" + (RenderSettings.sun ? RenderSettings.sun.name : "null") + " skymat=" + (RenderSettings.skybox ? RenderSettings.skybox.name : "null") + " light=" + (GameObject.Find("Directional Light") ? GameObject.Find("Directional Light").transform.eulerAngles.ToString() : "none");
