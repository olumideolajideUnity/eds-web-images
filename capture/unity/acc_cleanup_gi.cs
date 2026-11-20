// Remove the temporary realtime-GI scene (Enlighten realtime GI can't be enabled in Unity 7) and open Atrio.
using UnityEditor; using UnityEditor.SceneManagement;
EditorSceneManager.OpenScene("Assets/Atrio/AtrioScenes/Atrio.unity");
var r = AssetDatabase.DeleteAsset("Assets/EFDS_Screenshots/RealtimeGI.unity") + "/" + AssetDatabase.DeleteAsset("Assets/EFDS_Screenshots/RealtimeGI.lighting") + "/" + AssetDatabase.DeleteAsset("Assets/EFDS_Screenshots/RealtimeGI");
return "deleted " + r + " active=" + EditorSceneManager.GetActiveScene().name;
