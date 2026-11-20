using UnityEditor; using UnityEditor.EditorTools; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
Selection.activeGameObject = GameObject.Find("Main Block");
var ctxType = typeof(Editor).Assembly.GetType("UnityEditor.EditorTools.GameObjectToolContext");
var ctx = Resources.FindObjectsOfTypeAll(ctxType).FirstOrDefault() ?? ScriptableObject.CreateInstance(ctxType);
var menu = new DropdownMenu();
ctxType.GetMethod("PopulateMenu", BF).Invoke(ctx, new object[]{ menu });
// also let the active tool contribute (as the Scene view does)
var sb = new System.Text.StringBuilder();
foreach (var it in menu.MenuItems()) {
  if (it is DropdownMenuSeparator s) { sb.Append("---|" + s.subMenuPath + "\n"); continue; }
  if (it is DropdownMenuAction a) { a.UpdateActionStatus(null); sb.Append(a.name + "|" + a.status + "\n"); }
}
System.IO.File.WriteAllText("OUT", sb.ToString());
return sb.ToString();
