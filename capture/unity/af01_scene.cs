using UnityEditor; using UnityEditor.SceneManagement; using UnityEngine;
if (!AssetDatabase.IsValidFolder("Assets/EFDS_Screenshots")) AssetDatabase.CreateFolder("Assets", "EFDS_Screenshots");
var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
cube.name = "Cube";
cube.transform.position = new Vector3(8.789518f, 0.829852f, -4.06888f);
EditorSceneManager.SaveScene(scene, "Assets/EFDS_Screenshots/GOAuthoring.unity");

// Floating Scene view sized like the original (838 x 523 pt content)
var sv = ScriptableObject.CreateInstance<SceneView>();
sv.titleContent = new GUIContent("Scene");
sv.Show();
sv.position = new Rect(60, 120, 838, 523);
sv.in2DMode = false;
sv.LookAtDirect(cube.transform.position, Quaternion.Euler(24f, -28f, 0f), 1.15f);

// Floating Inspector, 500 pt wide like the original
var inspType = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
var insp = (EditorWindow)ScriptableObject.CreateInstance(inspType);
insp.Show();
insp.position = new Rect(918, 120, 500, 760);

Selection.activeGameObject = cube;
Tools.current = Tool.Move; Tools.pivotMode = PivotMode.Pivot; Tools.pivotRotation = PivotRotation.Global;
sv.Focus(); sv.Repaint(); insp.Repaint();
return "ok sv=" + sv.position + " insp=" + insp.position + " tool=" + Tools.current;
