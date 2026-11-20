// Color palette > Window Backgrounds: new empty scene with the original's four lights, Point Light selected,
// and a floating Light Explorer window (W x H pt incl. tab).
using UnityEditor; using UnityEngine; using UnityEditor.SceneManagement; using System.Linq; using System.Reflection;
float W = 780, H = 300;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
if (SceneManagerSetup()) {}
bool SceneManagerSetup() {
    var s = EditorSceneManager.GetActiveScene();
    if (s.name != "" || s.rootCount != 4) {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        Light Mk(string n, LightType t, LightmapBakeType b, Color c) {
            var g = new GameObject(n); var l = g.AddComponent<Light>(); l.type = t; l.lightmapBakeType = b; l.color = c; l.intensity = 1; return l;
        }
        Mk("Spot Light", LightType.Spot, LightmapBakeType.Realtime, Color.white);
        Mk("Area Light", LightType.Rectangle, LightmapBakeType.Baked, Color.white);
        Mk("Point Light", LightType.Point, LightmapBakeType.Realtime, Color.white);
        Mk("Directional Light", LightType.Directional, LightmapBakeType.Mixed, new Color32(255, 244, 214, 255));
    }
    return true;
}
Selection.activeGameObject = GameObject.Find("Point Light");
var t = typeof(Editor).Assembly.GetType("UnityEditor.LightingExplorerWindow");
var w = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().FirstOrDefault();
if (w == null) { w = (EditorWindow)ScriptableObject.CreateInstance(t); w.Show(); }
var host = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(w);
var cw = host.GetType().GetProperty("window", BF).GetValue(host);
cw.GetType().GetProperty("position", BF).SetValue(cw, new Rect(150, 120, W, H));
w.Repaint();
return "ok " + w.titleContent.text + " " + w.position;
