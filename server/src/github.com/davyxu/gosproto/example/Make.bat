set CURR_DIR=%cd%

: Build generator
cd ..\..\..\..\..
set GOPATH=%cd%
go build -o %CURR_DIR%\sprotogen.exe github.com/davyxu/gosproto/sprotogen
@IF %ERRORLEVEL% NEQ 0 pause
cd %CURR_DIR%

: Generate go source file by sproto
sprotogen --go_out=addressbook_gen.go --package=example --cellnet_reg=true addressbook.sp
@IF %ERRORLEVEL% NEQ 0 pause

: Convert to standard sproto file
: sprotogen --type=sproto --out=addressbook_gen.sproto addressbook.sp
@IF %ERRORLEVEL% NEQ 0 pause

: Format sp file
: sprotogen --type=sp --out=addressbook_gen.sp addressbook.sp
: @IF %ERRORLEVEL% NEQ 0 pause

: Generate c# source file by sproto
: sprotogen --type=cs --out=addressbook_gen.cs --package=example addressbook.sp
@IF %ERRORLEVEL% NEQ 0 pause

: Generate lua source file by sproto
: sprotogen --type=lua --out=addressbook_gen.lua --package=example addressbook.sp
@IF %ERRORLEVEL% NEQ 0 pause