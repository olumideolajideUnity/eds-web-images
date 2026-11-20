using UnityEditor;
using UnityEngine;

namespace EFDS
{
    // Real Editor controls for the Errors & messaging page: Unity's own help boxes and buttons, drawn exactly as any
    // Editor tool would draw them. Mode: 0 = help boxes (error, warning, info), 1 = dialog button order rows.
    public class EFDSMessageSpecimens : EditorWindow
    {
        public int mode;
        public static Rect[] rects = new Rect[0];

        public static EFDSMessageSpecimens Open(int mode, float w, float h)
        {
            foreach (var x in Resources.FindObjectsOfTypeAll<EFDSMessageSpecimens>()) x.Close();
            var win = CreateInstance<EFDSMessageSpecimens>();
            win.titleContent = new GUIContent("EFDS Messages");
            win.mode = mode;
            win.Show();
            win.position = new Rect(300, 200, w, h);
            return win;
        }

        // The Errors & messaging Console example, logged from compiled Editor code like a real tool would.
        public static void LogConsoleExample()
        {
            Debug.LogWarning("Caution message, something is wrong but it's not that bad");
            Debug.LogWarning("Caution message, something is wrong but it's not that bad");
            Debug.LogError("UI Builder: Cannot load .uxml files outside the Project.");
        }

        void OnGUI()
        {
            if (mode == 0)
            {
                rects = new Rect[3];
                GUILayout.Space(10);
                EditorGUILayout.HelpBox("This is a critical error message displayed with a help box.", MessageType.Error);
                rects[0] = GUILayoutUtility.GetLastRect();
                GUILayout.Space(10);
                EditorGUILayout.HelpBox("This is a warning message displayed with a help box.", MessageType.Warning);
                rects[1] = GUILayoutUtility.GetLastRect();
                GUILayout.Space(10);
                EditorGUILayout.HelpBox("This is a log / information message displayed with a help box.", MessageType.Info);
                rects[2] = GUILayoutUtility.GetLastRect();
            }
            else
            {
                // dialog button rows: buttons sized to their labels, aligned to the dialog's 12pt content margins
                float L = 24, R = position.width - 24, y0 = 30, dy = 66, gap = 4;
                Vector2 Sz(string s) { var v = GUI.skin.button.CalcSize(new GUIContent(s)); return new Vector2(Mathf.Max(v.x + 16, 58), 20); }
                var d = Sz("Destructive"); var s2 = Sz("Secondary"); var p = Sz("Primary");
                var list = new System.Collections.Generic.List<Rect>();
                Rect B(string label, float x, float y, Vector2 sz) { var r = new Rect(x, y, sz.x, sz.y); GUI.Button(r, label); list.Add(r); return r; }
                B("Destructive", L, y0, d); B("Primary", R - p.x, y0, p);
                B("Secondary", R - p.x - gap - s2.x, y0 + dy, s2); B("Primary", R - p.x, y0 + dy, p);
                B("Destructive", L, y0 + 2 * dy, d); B("Secondary", R - p.x - gap - s2.x, y0 + 2 * dy, s2); B("Primary", R - p.x, y0 + 2 * dy, p);
                rects = list.ToArray();
            }
        }
    }
}
