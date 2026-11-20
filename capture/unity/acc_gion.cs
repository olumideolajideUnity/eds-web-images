// Turn on Enlighten realtime GI on the RealtimeGI scene's lighting settings asset (serialized field), save, re-run the precompute.
using UnityEditor; using UnityEditor.SceneManagement; using UnityEngine;
var ls = Lightmapping.lightingSettings;
var so = new SerializedObject(ls); var s = "";
var it = so.GetIterator(); while (it.NextVisible(true)) if (it.name.ToLower().Contains("realtime") || it.name.ToLower().Contains("enlighten")) s += it.name + "=" + (it.propertyType == SerializedPropertyType.Boolean ? it.boolValue.ToString() : it.propertyType.ToString()) + " ";
var p = so.FindProperty("m_EnableRealtimeLightmaps") ?? so.FindProperty("m_GISettings.m_EnableRealtimeLightmaps");
if (p != null) { p.boolValue = true; so.ApplyModifiedPropertiesWithoutUndo(); }
EditorUtility.SetDirty(ls); AssetDatabase.SaveAssets(); EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene()); EditorSceneManager.SaveOpenScenes();
var ok = Lightmapping.BakeAsync();
return "fields: " + s + " | set=" + (p != null) + " bake=" + ok;
