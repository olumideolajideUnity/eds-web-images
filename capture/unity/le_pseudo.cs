using UnityEditor; using UnityEngine; using UnityEngine.UIElements; using System.Linq; using System.Reflection;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var p = typeof(VisualElement).GetProperty("pseudoStates", BF);
var t = p?.PropertyType;
return "prop=" + (p != null) + " type=" + t + " names=" + (t == null ? "" : string.Join(",", System.Enum.GetNames(t).Zip(System.Enum.GetValues(t).Cast<object>(), (n, v) => n + "=" + System.Convert.ToInt32(v)))) + " pro=" + EditorGUIUtility.isProSkin;
