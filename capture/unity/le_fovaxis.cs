using UnityEditor; using UnityEngine; using System.Linq;
var cam = Selection.activeGameObject.GetComponent<Camera>(); var so = new SerializedObject(cam);
return "fov=" + cam.fieldOfView + " axis=" + so.FindProperty("m_FOVAxisMode")?.intValue + " popup=" + Resources.FindObjectsOfTypeAll<EditorWindow>().Where(w => w.GetType().Name.Contains("Popup") || w.GetType().Name.Contains("Dropdown")).Select(w => w.GetType().Name).FirstOrDefault();
