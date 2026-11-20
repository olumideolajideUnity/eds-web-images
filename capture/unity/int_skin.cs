// Switch the Editor skin to PRO (true = dark, false = light) if it isn't already; returns the resulting skin.
using UnityEditor; using UnityEditorInternal;
bool PRO = false;
if (EditorGUIUtility.isProSkin != PRO) InternalEditorUtility.SwitchSkinAndRepaintAllViews();
return "pro=" + EditorGUIUtility.isProSkin;
