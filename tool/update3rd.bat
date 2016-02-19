: ×¼±¸MyUpdate3rd.bat, ÌîÈëcall update3rd.bat path\to\githubcode
set GOPATH=%1%
go install github.com/golang/protobuf/protoc-gen-go
copy %GOPATH%\bin\protoc-gen-go.exe .\*.*

go install github.com/davyxu/pbmeta/protoc-gen-meta
copy %GOPATH%\bin\protoc-gen-meta.exe .\*.*

go install github.com/davyxu/cellnet/protoc-gen-msg
copy %GOPATH%\bin\protoc-gen-msg.exe .\*.*

go install github.com/davyxu/protoc-gen-sharpnet
copy %GOPATH%\bin\protoc-gen-sharpnet.exe .\*.*

go install github.com/davyxu/tabtoy
copy %GOPATH%\bin\tabtoy.exe .\*.*