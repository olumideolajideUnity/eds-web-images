# Source after efds.sh: two <which> [extra sed]  -> captures/two_<which>[_l].png + $EFDS_TMP/two_<which>[_l].json (suffix by live skin)
two(){ local suf; suf=$(ucode 'return UnityEditor.EditorGUIUtility.isProSkin ? "D" : "L";' | grep -o '"result": "[DL]' | tail -c 2)
  [ "$suf" = "L" ] && suf="_l" || suf=""
  ueval "$AUD/unity/two_spec.cs" "s/string WHICH = \"btn_overview\";/string WHICH = \"$1\";/${2:+;$2}" | grep -o '"result": "[^"]*\|"message": "[^"]\{0,300\}'
  sleep 2; capwin EFDSPopupHost "two_$1$suf" >/dev/null; cp $EFDS_TMP/two_spec.json "$EFDS_TMP/two_$1$suf.json"; echo "captured two_$1$suf"; }
