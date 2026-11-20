// Tooltip page: in the UI element Inspector (Button from Assets/New UXML.uxml selected in the UI Viewport), type VALUE into the
// 'Tooltip' attribute field and give it real keyboard focus (as the original). VALUE "" clears it again.
using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq;
string VALUE = "Hello World";
var iw = Resources.FindObjectsOfTypeAll<EditorWindow>().First(x => x.GetType().Name == "InspectorWindow" && !x.docked);
var tf = iw.rootVisualElement.Query<TextField>().ToList().FirstOrDefault(t => t.label == "Tooltip");
if (tf == null) return "no tooltip field";
tf.value = VALUE; iw.Focus(); tf.Focus(); tf.schedule.Execute(() => tf.SelectRange(VALUE.Length, VALUE.Length)).StartingIn(100);
iw.Repaint(); return "set " + tf.worldBound;
