using UnityEditor; using UnityEngine;
namespace EFDS {
  [CustomEditor(typeof(EFDS.Camera))]
  class CameraSpecimenEditor : Editor {
    public override void OnInspectorGUI() {
      var c = (EFDS.Camera)target;
      c.fieldOfView = EditorGUILayout.Slider("Field of View", c.fieldOfView, 1e-05f, 179f); Rects.Last("fov");
    }
  }
  [CustomEditor(typeof(EFDS.Transform))]
  class TransformSpecimenEditor : Editor {
    public override void OnInspectorGUI() {
      var t = (EFDS.Transform)target; var p = t.position; string[] ax = { "X", "Y", "Z" };
      for (int i = 0; i < 3; i++) {
        var r = EditorGUILayout.GetControlRect(); Rects.Mark("row" + i, r);
        if (i == 0) EditorGUI.LabelField(new Rect(r.x, r.y, EditorGUIUtility.labelWidth, r.height), "Position");
        var f = new Rect(r.x + EditorGUIUtility.labelWidth + 2, r.y, r.width - EditorGUIUtility.labelWidth - 2, r.height);
        EditorGUI.LabelField(new Rect(f.x, f.y, 14, f.height), ax[i]);
        p[i] = EditorGUI.Slider(new Rect(f.x + 14, f.y, f.width - 14, f.height), p[i], 0f, 10f);
      }
      t.position = p;
    }
  }
}
namespace EFDS {
  [UnityEditor.CustomEditor(typeof(EFDS.DepthOfField))]
  class DepthOfFieldSpecimenEditor : UnityEditor.Editor {
    public override UnityEngine.UIElements.VisualElement CreateInspectorGUI() {
      var d = (EFDS.DepthOfField)target; var root = new UnityEngine.UIElements.VisualElement();
      void Row(string label, float v, float max) {
        root.Add(new UnityEngine.UIElements.Slider(label, 0, max) { value = v, showInputField = true });
        var p = new UnityEngine.UIElements.ProgressBar { title = label, lowValue = 0, highValue = 100, value = v / max * 100 }; p.style.marginLeft = 3; p.style.marginRight = 3; root.Add(p);
      }
      Row("Focus Distance", d.focusDistance, 43); Row("Focal Length", d.focalLength, 140);
      return root;
    }
  }
}
