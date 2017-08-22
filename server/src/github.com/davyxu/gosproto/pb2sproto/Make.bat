set CURR_DIR=%cd%

: Build generator
cd ..\..\..\..\..
set GOPATH=%cd%
go build -o %CURR_DIR%\protoc-gen-meta.exe github.com/davyxu/pbmeta/protoc-gen-meta
cd %CURR_DIR%

protoc.exe --plugin=protoc-gen-meta=protoc-gen-meta.exe --meta_out=meta.pb:. --proto_path "." pb.proto
:@IF %ERRORLEVEL% NEQ 0 pause

go build -o pb2sproto.exe gen_proto.go main.go

pb2sproto --pbmeta=meta.pb --outdir=.