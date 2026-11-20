// Open the Mesh Renderer's Lighting foldout (EditorPrefs key used by RendererEditorBase: "<type>.ShowLightingSettings").
using UnityEditor; using UnityEngine;
EditorPrefs.SetBool("UnityEngine.MeshRenderer.ShowLightingSettings", true);
ActiveEditorTracker.sharedTracker.ForceRebuild();
return "set " + EditorPrefs.GetBool("UnityEngine.MeshRenderer.ShowLightingSettings");
