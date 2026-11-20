# Components > Search field 'search results': the floating Project browser (grid view) searched for "c", "ca" ... "candle" through its
# own ProjectBrowser.SetSearch (Unity's real search runs on each), then a real click on a result.
source efds.sh; source scripts/gif_cap.sh
PB=ProjectBrowser
pbs(){ ucode "var BF=System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic; var t=typeof(UnityEditor.Editor).Assembly.GetType(\"UnityEditor.ProjectBrowser\"); var w=UnityEngine.Resources.FindObjectsOfTypeAll(t).Cast<UnityEditor.EditorWindow>().First(x=>!x.docked); UnityEditor.Selection.activeObject=null; var st=t.GetField(\"m_ListAreaState\",BF).GetValue(w); var ids=st.GetType().GetField(\"m_SelectedInstanceIDs\",BF)?.GetValue(st) as System.Collections.IList; ids?.Clear(); t.GetMethod(\"SetSearch\",BF,null,new[]{typeof(string)},null).Invoke(w,new object[]{\"$1\"}); var la=t.GetField(\"m_ListArea\",BF).GetValue(w); la.GetType().GetProperty(\"gridSize\",BF).SetValue(la,56); w.Repaint(); return \"s\";" >/dev/null; }
ueval unity/gif_proj.cs >/dev/null; sleep 1; pbs ""; ueval unity/gif_proj.cs >/dev/null; sleep 2; pbs ""; sleep 1; gm leave 0 0 $PB; gf sr_0 $PB
i=1; for s in c ca can cand candl candle; do pbs $s; sleep 2; gf sr_$i $PB; i=$((i+1)); done
gm click ${CX:-317} ${CY:-110} $PB; sleep 1.5; gf sr_7 $PB
