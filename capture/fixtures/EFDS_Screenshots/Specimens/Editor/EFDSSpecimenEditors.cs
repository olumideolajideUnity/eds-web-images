using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EFDS
{
    // Records screen-space rects of named rows so annotations can be drawn precisely over captures.
    public static class Rects
    {
        public static Dictionary<string, Rect> All = new Dictionary<string, Rect>();
        public static bool Calibrate;
        public static void Mark(string key, Rect r)
        {
            if (Event.current.type != EventType.Repaint) return;
            All[key] = GUIUtility.GUIToScreenRect(r);
            if (Calibrate && key == "blend") EditorGUI.DrawRect(new Rect(r.x, r.y, 6, 6), Color.magenta);
        }
        public static void Last(string key) { Mark(key, GUILayoutUtility.GetLastRect()); }
        public static string Dump()
        {
            var sb = new System.Text.StringBuilder("{");
            foreach (var kv in All)
                sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "\"{0}\":[{1},{2},{3},{4}],", kv.Key, kv.Value.x, kv.Value.y, kv.Value.width, kv.Value.height);
            return sb.ToString().TrimEnd(',') + "}";
        }
    }

    static class G
    {
        public static GUIStyle BoldFoldout => new GUIStyle(EditorStyles.foldout) { fontStyle = FontStyle.Bold };
        public static void Popup(string key, string label, string value) { EditorGUILayout.Popup(label, 0, new[] { value }); Rects.Last(key); }
    }

    [CustomEditor(typeof(ComponentHeader))]
    class ComponentHeaderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var top = EditorGUILayout.BeginVertical();
            EditorGUILayout.Foldout(true, "Foldout 1", true, G.BoldFoldout); Rects.Last("f1");
            EditorGUI.indentLevel++;
            EditorGUILayout.Toggle("Label", false); Rects.Last("l1");
            var r = EditorGUILayout.GetControlRect(); Rects.Mark("f2", r);
            EditorGUI.Foldout(new Rect(r.x, r.y, EditorGUIUtility.labelWidth, r.height), true, "Foldout 2", true);
            EditorGUI.Popup(new Rect(r.x + EditorGUIUtility.labelWidth + 2, r.y, r.width - EditorGUIUtility.labelWidth - 2, r.height), 0, new[] { "Dropdown" });
            EditorGUI.indentLevel++;
            EditorGUILayout.Vector2Field("Label", Vector2.zero); Rects.Last("l2");
            EditorGUILayout.IntField("Label", 0); Rects.Last("l3");
            EditorGUI.indentLevel -= 2;
            EditorGUILayout.Space(6);
            G.Popup("l4", "Label", "Dropdown");
            EditorGUILayout.IntField("Label", 0); Rects.Last("l5");
            EditorGUILayout.EndVertical(); Rects.Mark("body", top);
        }
    }

    [CustomEditor(typeof(HeightNoiseLayer))]
    class HeightNoiseLayerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var top = EditorGUILayout.BeginVertical();
            G.Popup("blend", "Blend", "Add"); G.Popup("res", "Resolution", "Full"); G.Popup("type", "Type", "Voronoi");
            EditorGUILayout.Space(8);
            EditorGUILayout.Foldout(true, "Global Controls", true); Rects.Last("global");
            EditorGUI.indentLevel++;
            EditorGUILayout.IntField("Seed", 0); Rects.Last("seed");
            G.Popup("voronoi", "Voronoi Noise", "Default");
            EditorGUILayout.IntField("Frequency", 10); Rects.Last("freq");
            EditorGUILayout.Slider("Height", 0.1f, 0f, 1f); Rects.Last("height");
            EditorGUI.indentLevel--;
            EditorGUILayout.Space(8);
            EditorGUILayout.Foldout(false, "Complexity Controls", true); Rects.Last("complexity");
            EditorGUILayout.Space(8);
            EditorGUILayout.Foldout(false, "Warm Controls", true); Rects.Last("warm");
            EditorGUILayout.EndVertical(); Rects.Mark("body", top);
        }
    }

    [CustomEditor(typeof(MeshRenderer))]
    class MeshRendererEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var el = serializedObject.FindProperty("elements"); el.isExpanded = true;
            EditorGUILayout.PropertyField(el); Rects.Last("elements");
            EditorGUILayout.Space(4);
            G.Popup("blend", "Blend", "Add"); G.Popup("res", "Resolution", "Full"); G.Popup("type", "Type", "Voronoi");
            EditorGUILayout.Foldout(true, "Lighting", true); Rects.Last("lighting");
            EditorGUI.indentLevel++;
            G.Popup("cast", "Cast Shadows", "On");
            EditorGUILayout.Toggle("Receive Shadows", false); Rects.Last("receive");
            EditorGUILayout.Toggle("Contribute Global Illumination", true); Rects.Last("contribute");
            EditorGUI.indentLevel--;
            serializedObject.ApplyModifiedProperties();
        }
    }

    [CustomEditor(typeof(RectTransform))]
    class RectTransformEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Some values are driven by Canvas.", MessageType.None); Rects.Last("driven");
            EditorGUILayout.Space(6);
            using (new EditorGUI.DisabledScope(true))
            {
                Row(new[] { "Pos X", "Pos Y", "Pos Z" }, new[] { "547.75", "296", "0" }, "pos");
                Row(new[] { "Width", "Height" }, new[] { "1095.5", "592" }, "size");
            }
            EditorGUILayout.Space(10);
            EditorGUILayout.HelpBox("Multiple materials will be applied to the same submesh, which costs performance. Consider using multiple shader passes.", MessageType.Warning); Rects.Last("warning");
        }

        static void Row(string[] labels, string[] values, string key)
        {
            var r = EditorGUILayout.GetControlRect(false, 36); Rects.Mark(key, r);
            float x0 = r.x + EditorGUIUtility.labelWidth, w = (r.width - EditorGUIUtility.labelWidth) / 3f;
            for (int i = 0; i < labels.Length; i++)
            {
                GUI.Label(new Rect(x0 + i * w, r.y, w - 4, 18), labels[i], EditorStyles.label);
                EditorGUI.TextField(new Rect(x0 + i * w, r.y + 18, w - 4, 18), values[i]);
            }
            if (labels.Length == 2)
            {
                var b = new Rect(x0 + 2 * w, r.y + 18, (w - 4) / 2f, 18);
                GUI.Button(b, EditorGUIUtility.IconContent("d_RectTransformBlueprint"), EditorStyles.miniButtonLeft);
                GUI.Button(new Rect(b.xMax, b.y, b.width, 18), "R", EditorStyles.miniButtonRight);
            }
        }
    }

    [CustomEditor(typeof(MaterialLayer))]
    class MaterialLayerEditor : Editor
    {
        static Texture2D s_Arrow;

        static Texture2D Arrow()
        {
            if (s_Arrow) return s_Arrow;
            s_Arrow = new Texture2D(32, 32, TextureFormat.RGBA32, false) { hideFlags = HideFlags.DontSave };
            var px = new Color32[32 * 32];
            for (int y = 0; y < 32; y++)
                for (int x = 0; x < 32; x++)
                {
                    bool head = y >= 16 && Mathf.Abs(x - 15.5f) <= (31 - y) * 0.75f;
                    bool shaft = y < 18 && y >= 3 && Mathf.Abs(x - 15.5f) <= 3;
                    px[y * 32 + x] = (head || shaft) ? new Color32(230, 30, 30, 255) : new Color32(20, 20, 20, 255);
                }
            s_Arrow.SetPixels32(px); s_Arrow.Apply();
            return s_Arrow;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Mask Stack (4)", EditorStyles.label); Rects.Mark("mainheader", EditorGUI.IndentedRect(GUILayoutUtility.GetLastRect()));
            Layer("Paint", "Subtract", false, false, "paint");
            Layer("Blur", "Add", false, false, "blur");
            Layer("Stamp", "Add", true, true, "stamp");
            EditorGUILayout.Space(4);
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField("Properties"); Rects.Mark("subheader", EditorGUI.IndentedRect(GUILayoutUtility.GetLastRect()));
            EditorGUI.indentLevel++;
            G.Popup("mode", "Stamp Mode", "Texture");
            var r = EditorGUILayout.GetControlRect(); Rects.Mark("texture", r);
            EditorGUI.LabelField(r, "Texture");
            var fr = new Rect(r.x + EditorGUIUtility.labelWidth + 2, r.y, r.width - EditorGUIUtility.labelWidth - 2, r.height);
            int ind = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
            EditorGUI.LabelField(fr, new GUIContent("Arrow", Arrow()), EditorStyles.objectField);
            EditorGUI.indentLevel = ind;
            EditorGUI.indentLevel--;
            EditorGUILayout.Foldout(true, "UW Options", true); Rects.Mark("subfoldout", EditorGUI.IndentedRect(GUILayoutUtility.GetLastRect()));
            EditorGUI.indentLevel++;
            G.Popup("space", "Space", "Layer");
            EditorGUILayout.Vector2Field("Layer Tiling", Vector2.zero); Rects.Last("tiling");
            EditorGUILayout.Vector2Field("Layer Offset", Vector2.zero); Rects.Last("offset");
            EditorGUI.indentLevel -= 2;
        }

        void Layer(string name, string blend, bool expanded, bool selected, string key)
        {
            var r = EditorGUILayout.GetControlRect(false, 22); Rects.Mark(key, r);
            float x = r.x + 4, cy = r.y + 2;
            if (Event.current.type == EventType.Repaint) ((GUIStyle)"RL DragHandle").Draw(new Rect(x, cy + 7, 10, 6), false, false, false, false);
            x += 30;
            EditorGUI.Foldout(new Rect(x, cy, 14, 18), expanded, GUIContent.none); x += 18;
            EditorGUI.Toggle(new Rect(x, cy, 18, 18), true); x += 22;
            GUI.DrawTexture(new Rect(x, cy + 1, 16, 16), Arrow()); x += 20;
            var st = new GUIStyle(EditorStyles.label);
            if (selected) st.normal.textColor = new Color(0.47f, 0.69f, 1f);
            GUI.Label(new Rect(x, cy, 120, 18), name, st);
            float rx = r.xMax - 220;
            GUI.Label(new Rect(rx, cy, 20, 18), EditorGUIUtility.IconContent("d_scenevis_visible_hover"));
            EditorGUI.Popup(new Rect(rx + 22, cy, 90, 18), 0, new[] { blend });
            GUI.HorizontalSlider(new Rect(rx + 118, cy + 1, 96, 16), 0.65f, 0f, 1f);
        }
    }
}
