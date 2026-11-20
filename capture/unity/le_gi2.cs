using UnityEditor; using UnityEngine; using UnityEngine.Rendering; using UnityEditor.SceneManagement;

string s = "scene=" + EditorSceneManager.GetActiveScene().path;
try { s += " rtGI=" + Lightmapping.lightingSettings.realtimeGI + " baked=" + Lightmapping.lightingSettings.bakedGI; } catch (System.Exception e) { s += " ls=" + e.GetType().Name; }
s += " lda=" + (Lightmapping.lightingDataAsset ? Lightmapping.lightingDataAsset.name : "null");
var f = SupportedRenderingFeatures.active; s += " enlighten=" + f.enlighten + " lightmapBakeTypes=" + f.lightmapBakeTypes;
s += " pipeline=" + (GraphicsSettings.currentRenderPipeline ? GraphicsSettings.currentRenderPipeline.GetType().Name : "builtin");
return s;
