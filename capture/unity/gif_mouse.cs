// Send a real mouse event to the first floating window whose type name is TYPE: MODE move (hover), click, or leave (move far away).
// X, Y in pt, window coords (as win_click.cs). MODE key types the characters of KEYS as real KeyDown events.
using UnityEditor; using UnityEngine; using System.Linq;
string TYPE = "InspectorWindow"; string MODE = "move"; float X = 0, Y = 0; string KEYS = "";
var w = Resources.FindObjectsOfTypeAll<EditorWindow>().First(x => x.GetType().Name == TYPE && !x.docked);
w.wantsMouseMove = true; w.Focus();
if (MODE == "key") { foreach (var ch in KEYS) { w.SendEvent(new Event { type = EventType.KeyDown, character = ch, keyCode = KeyCode.None }); } w.Repaint(); return "typed " + KEYS; }
var p = MODE == "leave" ? new Vector2(-500, -500) : new Vector2(X, Y);
if (MODE == "click") { w.SendEvent(new Event { type = EventType.MouseDown, mousePosition = p, button = 0, clickCount = 1 });
                       w.SendEvent(new Event { type = EventType.MouseUp, mousePosition = p, button = 0, clickCount = 1 }); }
w.SendEvent(new Event { type = EventType.MouseMove, mousePosition = p });
w.Repaint(); return MODE + " " + p;
