using UnityEditor; using UnityEngine; using System.Linq;
var cands = new[]{ "align_horizontally_left","align_horizontally_center","align_horizontally_right","align_horizontally_left_active","d_align_horizontally_left","GUISystem/align_horizontally_left","TextAlignLeft","d_TextAlignLeft","Text Left","d_AlignLeft","AlignLeft" };
var s = string.Join(",", cands.Select(n => n + "=" + (EditorGUIUtility.FindTexture(n) != null) + "/" + (EditorGUIUtility.Load(n) != null)));

return s;
