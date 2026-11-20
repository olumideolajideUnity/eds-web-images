using UnityEditor; using UnityEditor.SceneManagement; using UnityEngine; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
sv.position = new Rect(300, 120, 679, 430);
sv.drawGizmos = false;
sv.Focus();
var stage = PrefabStageUtility.OpenPrefab("Assets/ExampleAssets/Prefabs/PaintSupplies.prefab");
var root = stage.prefabContentsRoot;
Selection.activeGameObject = root;
// Floating Hierarchy
var hType = typeof(Editor).Assembly.GetType("UnityEditor.SceneHierarchyWindow");
var h = Resources.FindObjectsOfTypeAll(hType).Cast<EditorWindow>().FirstOrDefault(w => !w.docked);
if (h == null) { h = (EditorWindow)ScriptableObject.CreateInstance(hType); h.Show(); }
h.position = new Rect(60, 120, 214, 430);
var m = hType.GetMethod("SetExpandedRecursive", BF);
if (m != null) {
    var pt = m.GetParameters()[0].ParameterType;
    object id = pt == typeof(int) ? (object)typeof(Object).GetMethod("GetInstanceID").Invoke(root, null) : root.GetEntityId();
    m.Invoke(h, new object[]{ id, true });
}
h.Repaint(); sv.Repaint();
return "stage=" + stage.assetPath + " root=" + root.name + " expandMethod=" + (m != null) + " sv=" + sv.position + " h=" + h.position;
