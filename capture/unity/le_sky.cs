using UnityEditor; using UnityEngine;
var s = RenderSettings.skybox;
return "sky=" + (s ? s.name + "/" + s.shader.name : "null") + " sun=" + (RenderSettings.sun ? RenderSettings.sun.name : "null") + " roots=" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().rootCount + " amb=" + RenderSettings.ambientMode;
