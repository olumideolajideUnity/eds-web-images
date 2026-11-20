// Temporary, never-saved exposure boost for dark sample scenes. Edit EXPOSURE per shot; set 0 to remove.
using UnityEditor; using UnityEngine; using UnityEngine.Rendering; using UnityEngine.Rendering.Universal; using System.Linq;
float EXPOSURE = 0f;
var go = GameObject.Find("EFDS_TempBrighten");
if (go == null) { go = new GameObject("EFDS_TempBrighten"); go.hideFlags = HideFlags.DontSave | HideFlags.HideInHierarchy; }
var vol = go.GetComponent<Volume>(); if (vol == null) vol = go.AddComponent<Volume>();
vol.isGlobal = true; vol.priority = 1000; vol.weight = EXPOSURE > 0 ? 1 : 0;
if (vol.sharedProfile == null) { var prof = ScriptableObject.CreateInstance<VolumeProfile>(); prof.hideFlags = HideFlags.DontSave; vol.sharedProfile = prof; }
var ca = vol.sharedProfile.components.OfType<ColorAdjustments>().FirstOrDefault() ?? vol.sharedProfile.Add<ColorAdjustments>(true);
ca.postExposure.Override(EXPOSURE);
foreach (var sv in Resources.FindObjectsOfTypeAll<SceneView>()) { sv.sceneViewState.alwaysRefresh = true; sv.sceneViewState.showImageEffects = true; sv.Repaint(); }
return "exposure=" + EXPOSURE + " sceneDirty=" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().isDirty;
