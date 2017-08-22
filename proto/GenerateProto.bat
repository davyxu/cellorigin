set CURR=%cd%
cd tool
call gen_sproto ^
..\login.sp ^
..\clientnet.sp

cd %CURR%