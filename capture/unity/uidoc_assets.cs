// Temporary UI Toolkit document assets for the UI Viewport / UI element Inspector captures (deleted again afterwards):
// Assets/New UXML.uxml (one Button), Assets/UI Toolkit/PanelSettings.asset with Assets/UI Toolkit/UnityThemes/UnityDefaultRuntimeTheme.tss.
using UnityEditor; using UnityEngine; using UnityEngine.UIElements;
System.IO.File.WriteAllText("Assets/New UXML.uxml", "<ui:UXML xmlns:ui=\"UnityEngine.UIElements\" xmlns:uie=\"UnityEditor.UIElements\">\n    <ui:Button text=\"Button\"/>\n</ui:UXML>\n");
if (!AssetDatabase.IsValidFolder("Assets/UI Toolkit")) AssetDatabase.CreateFolder("Assets", "UI Toolkit");
if (!AssetDatabase.IsValidFolder("Assets/UI Toolkit/UnityThemes")) AssetDatabase.CreateFolder("Assets/UI Toolkit", "UnityThemes");
System.IO.File.WriteAllText("Assets/UI Toolkit/UnityThemes/UnityDefaultRuntimeTheme.tss", "@import url(\"unity-theme://default\");\n");
AssetDatabase.Refresh();
var ps = ScriptableObject.CreateInstance<PanelSettings>(); ps.themeStyleSheet = AssetDatabase.LoadAssetAtPath<ThemeStyleSheet>("Assets/UI Toolkit/UnityThemes/UnityDefaultRuntimeTheme.tss");
AssetDatabase.CreateAsset(ps, "Assets/UI Toolkit/PanelSettings.asset"); AssetDatabase.SaveAssets();
return "theme=" + (ps.themeStyleSheet != null) + " uxml=" + (AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/New UXML.uxml") != null);
