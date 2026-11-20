// Unity's own status-bar code-optimization popup (ManagedDebuggerWindow, a PopupWindowContent) for the current mode,
// hosted in EFDS.EFDSPopupHost (real popups close when Unity loses focus).
using UnityEditor; using UnityEngine; using UnityEditor.Compilation; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
var t = typeof(Editor).Assembly.GetType("UnityEditor.ManagedDebuggerWindow");
var c = (PopupWindowContent)System.Activator.CreateInstance(t, BF, null, new object[]{ CompilationPipeline.codeOptimization }, null);
var hostT = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("EFDS.EFDSPopupHost")).First(x => x != null);
var host = (EditorWindow)hostT.GetMethod("Open").Invoke(null, new object[]{ c });
return "mode=" + CompilationPipeline.codeOptimization + " size=" + c.GetWindowSize() + " pos=" + host.position;
