: ×¼±¸MyUpdate3rd.bat, ÌîÈëcall update3rd.bat path\to\githubcode
set GOPATH=%1%
go get -u -v github.com/golang/protobuf/protoc-gen-go
go get -u -v github.com/davyxu/pbmeta/protoc-gen-meta
go get -u -v github.com/davyxu/cellnet/protoc-gen-msg
go get -u -v github.com/davyxu/protoc-gen-sharpnet
go get -u -v github.com/davyxu/tabtoy

copy %GOPATH%\bin\protoc-gen-go.exe .\*.*
copy %GOPATH%\bin\protoc-gen-meta.exe .\*.*
copy %GOPATH%\bin\protoc-gen-msg.exe .\*.*
copy %GOPATH%\bin\protoc-gen-sharpnet.exe .\*.*
copy %GOPATH%\bin\tabtoy.exe .\*.*