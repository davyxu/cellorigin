
: 服务器使用的所有GOPATH
set CURR=%cd%
cd ../../server
set GOPATH=%cd%
cd %CURR%

: windows版本
set GOARCH=amd64
set GOOS=windows

go build -v -o sprotogen.exe github.com/davyxu/gosproto/sprotogen