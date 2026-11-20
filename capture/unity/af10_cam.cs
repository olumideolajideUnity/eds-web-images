using UnityEditor; using UnityEngine; using System.Linq;
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
var rot = Quaternion.Euler(28f, 35f, 0f);
var c = new Vector3(8.789518f, 0.829852f, -4.06888f);
sv.LookAtDirect(c + rot * Vector3.right * 1.6f, rot, 2.2f);   // push the cube toward the left third
sv.drawGizmos = false; sv.Focus(); sv.Repaint();
var pe = Resources.FindObjectsOfTypeAll<EditorWindow>().First(w => w.GetType().Name == "PropertyEditor");
pe.position = new Rect(pe.position.x, pe.position.y, 403, 700);
return "ok";
