using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection; using System.Collections;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var sv = Resources.FindObjectsOfTypeAll<SceneView>().First(w => !w.docked);
Selection.activeGameObject = null;
var archTool = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("UnityEditor.ProBuilder.CreateArchTool")).First(x => x != null);
UnityEditor.EditorTools.ToolManager.SetActiveTool(archTool);
var root = sv.overlayCanvas.overlays.First(o => o.displayName == "Tools").rootVisualElement;
var b = root.Query<VisualElement>().ToList().First(e => e.GetType().GetField("m_Variants", BF) != null && ((IList)e.GetType().GetField("m_Variants", BF).GetValue(e)).Cast<object>().Any(v => v.GetType() == archTool));
var t = b.GetType();
var list = (IList)t.GetField("m_Variants", BF).GetValue(b);
int idx = -1; for (int i = 0; i < list.Count; i++) if (list[i].GetType() == archTool) idx = i;
t.GetField("m_CurrentVariantIndex", BF).SetValue(b, idx);
var dd = (VisualElement)t.GetField("m_ToolVariantDropdown", BF).GetValue(b); dd?.RemoveFromHierarchy(); t.GetField("m_ToolVariantDropdown", BF).SetValue(b, null);
t.GetMethod("UpdateContent", BF)?.Invoke(b, null);
foreach (var o in sv.overlayCanvas.overlays) if (o.displayName.Contains("Shape Settings") || o.id.Contains("Shape Settings")) { o.collapsed = false; o.rootVisualElement.style.display = DisplayStyle.None; }
sv.LookAtDirect(new Vector3(0.9f, 2.6f, 0), Quaternion.Euler(28f, -22f, 0f), 8.6f);
t.GetMethod("OpenVariantsDropdown", BF).Invoke(b, null);
sv.Repaint();
return "button=" + b.name + " archIndex=" + idx + " active=" + UnityEditor.EditorTools.ToolManager.activeToolType?.Name;
