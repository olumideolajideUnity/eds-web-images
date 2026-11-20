using UnityEditor; using UnityEngine;
void Set(string cls, string icon) { var ms = AssetDatabase.LoadAssetAtPath<MonoScript>("Assets/EFDS_Screenshots/Specimens/" + cls + ".cs"); EditorGUIUtility.SetIconForObject(ms, (Texture2D)EditorGUIUtility.IconContent(icon).image); EditorUtility.SetDirty(ms); }
Set("MeshRenderer", "d_MeshRenderer Icon"); Set("RectTransform", "d_RectTransform Icon"); Set("MaterialLayer", "d_Material Icon");
AssetDatabase.SaveAssets();
return "icons set";
