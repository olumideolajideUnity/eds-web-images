using UnityEditor; using UnityEngine; using System.Linq;
var s = string.Join("\n", Resources.FindObjectsOfTypeAll<GameObject>().Where(g => !EditorUtility.IsPersistent(g) && (g.hideFlags & HideFlags.HideAndDontSave) != HideFlags.HideAndDontSave)
  .Select(g => g.name + " [" + g.hideFlags + "] scene=" + g.scene.name + " pos=" + g.transform.position + " : " + string.Join(",", g.GetComponents<Component>().Select(c => c ? c.GetType().Name : "null"))));
System.IO.File.WriteAllText("C:/tmp/objs.txt", s); return "ok";
