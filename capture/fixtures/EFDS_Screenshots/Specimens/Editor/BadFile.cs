using UnityEditor;
using UnityEngine;

namespace EFDS
{
    // Color palette > Text Colors Base 1: the original's Console example, logged from compiled code on the editor update loop
    // so each stack trace reads like a real tool's, with a clickable file link.
    public static class Example
    {
        public static void Log() { EditorApplication.update += ExampleError; }

        static void ExampleError()
        {
            EditorApplication.update -= ExampleError;
            Debug.LogWarning("Caution message, something is wrong but it's not that bad");
            Debug.LogWarning("Caution message, something is wrong but it's not that bad");
            Debug.LogError("Warning message, something has gone critically wrong!");
        }
    }
}
