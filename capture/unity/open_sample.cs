using UnityEditor; using UnityEditor.SceneManagement; using UnityEngine;
var s = EditorSceneManager.OpenScene("Assets/Scenes/SampleScene.unity");
var sv = SceneView.lastActiveSceneView; if (sv) { sv.drawGizmos = true; sv.LookAtDirect(new Vector3(0, 1, 0), Quaternion.Euler(8, 30, 0), 12f); sv.Repaint(); }
return "open " + s.name + " roots=" + s.rootCount;
