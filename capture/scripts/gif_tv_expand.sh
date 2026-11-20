# Components > Tree view 'tv-expand-collapse': real clicks in the floating two-column Project browser's folder tree (rows 16pt).
source efds.sh; source scripts/gif_cap.sh
PB=ProjectBrowser
ueval unity/gif_proj.cs >/dev/null; sleep 3; gm leave 0 0 $PB; gf tvx_0 $PB
gm click 10 88 $PB;  sleep 1; gf tvx_1 $PB     # expand Assets
gm click 70 120 $PB; sleep 1; gf tvx_2 $PB     # select Atrio
gm click 24 120 $PB; sleep 1; gf tvx_3 $PB     # expand Atrio
gm click 80 216 $PB; sleep 1; gf tvx_4 $PB     # select Character
gm click 38 216 $PB; sleep 1; gf tvx_5 $PB     # expand Character
gm click 95 232 $PB; sleep 1; gf tvx_6 $PB     # select Bird
gm click 24 120 $PB; sleep 1; gf tvx_7 $PB     # collapse Atrio
gm click 10 88 $PB;  sleep 1; gf tvx_8 $PB     # collapse Assets
