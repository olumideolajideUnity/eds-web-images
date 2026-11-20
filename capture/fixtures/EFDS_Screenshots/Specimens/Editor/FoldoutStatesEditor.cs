using UnityEditor;
using UnityEngine;

namespace EFDS
{
    // Draws foldouts in forced states (idle / hover / focused / disabled) using Unity's own foldout style.
    [CustomEditor(typeof(FoldoutStates))]
    class FoldoutStatesEditor : Editor
    {
        const float Row = 88f, Col2 = 121f, X0 = 29f;
        public override void OnInspectorGUI()
        {
            var area = GUILayoutUtility.GetRect(390, Row * 4);
            Rects.Mark("area", area);
            var style = EditorStyles.foldout;
            var content = new GUIContent("Foldout");
            for (int i = 0; i < 4; i++)
            {
                bool hover = i == 1, focus = i == 2, disabled = i == 3;
                float cy = area.y + Row * i + Row / 2 - 9;
                Rects.Mark("row" + i, new Rect(area.x, area.y + Row * i, area.width, Row));
                using (new EditorGUI.DisabledScope(disabled))
                {
                    for (int c = 0; c < 2; c++)
                    {
                        var r = new Rect(area.x + X0 + c * Col2, cy, 80, 18);
                        if (Event.current.type == EventType.Repaint)
                            style.Draw(r, content, hover, false, c == 0, focus);
                    }
                }
            }
        }
    }
}
