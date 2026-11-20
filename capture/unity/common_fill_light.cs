// Temporary, never-saved fill light aimed from the Scene view camera. Edit INTENSITY; 0 removes it.
using UnityEditor; using UnityEngine; using System.Linq;
float INTENSITY = 0.7f;
var go = GameObject.Find("EFDS_TempFill");
if (INTENSITY <= 0) { if (go) Object.DestroyImmediate(go); return "removed"; }
if (go == null) { go = new GameObject("EFDS_TempFill"); go.hideFlags = HideFlags.DontSave | HideFlags.HideInHierarchy; }
var l = go.GetComponent<Light>(); if (l == null) l = go.AddComponent<Light>();
l.type = LightType.Directional; l.shadows = LightShadows.None; l.intensity = INTENSITY; l.color = new Color(1f, 0.97f, 0.92f);
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
go.transform.rotation = Quaternion.Euler(35f, sv.rotation.eulerAngles.y + 25f, 0f);
foreach (var v in Resources.FindObjectsOfTypeAll<SceneView>()) v.Repaint();
return "fill=" + INTENSITY + " sceneDirty=" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().isDirty;
