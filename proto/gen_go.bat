set outdir=..\server\src\proto\gamedef
mkdir %outdir%
..\tool\protoc.exe --plugin=protoc-gen-go=..\tool\protoc-gen-go.exe --go_out %outdir% --proto_path "." %*
..\tool\protoc.exe --plugin=protoc-gen-msg=..\tool\protoc-gen-msg.exe --msg_out %outdir% --proto_path "." %*