// Temporarily switch Sprite Atlas mode (EditorSettings.spritePackerMode) to MODE; returns the previous value.
using UnityEditor; using UnityEngine;
string MODE = "Disabled";
var prev = EditorSettings.spritePackerMode;
EditorSettings.spritePackerMode = (SpritePackerMode)System.Enum.Parse(typeof(SpritePackerMode), MODE);
return "prev=" + prev + " now=" + EditorSettings.spritePackerMode + " all=" + string.Join(",", System.Enum.GetNames(typeof(SpritePackerMode)));
