// Typography (a11y-11): floating Project Settings (W x H pt incl. tab) searched for SEARCH, on the Editor page.
using UnityEditor; using UnityEngine; using System.Linq; using System.Reflection;
float W = 630, H = 260; string SEARCH = "Editor"; float SCROLL = 0;
var BF = BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
var t = typeof(Editor).Assembly.GetType("UnityEditor.ProjectSettingsWindow"); var st = t.BaseType;
var w = Resources.FindObjectsOfTypeAll(t).Cast<EditorWindow>().FirstOrDefault(x => !x.docked);
if (w == null) { w = (EditorWindow)ScriptableObject.CreateInstance(t); w.Show(); }
var host = typeof(EditorWindow).GetField("m_Parent", BF).GetValue(w);
var cw = host.GetType().GetProperty("window", BF).GetValue(host);
cw.GetType().GetProperty("position", BF).SetValue(cw, new Rect(250, 120, W, H));
st.GetField("m_SearchText", BF).SetValue(w, SEARCH);
st.GetMethod("HandleSearchFiltering", BF).Invoke(w, null);
st.GetMethod("SelectProviderByName", BF).Invoke(w, new object[]{ "Project/Editor", true });
st.GetField("m_PosRight", BF).SetValue(w, new Vector2(0, SCROLL));
w.Repaint();
return "providers=" + st.GetMethod("GetVisibleProviderCount", BF).Invoke(w, null);
