set outdir=..\server\src\proto\gamedef
set plugindir=.
mkdir %outdir%
protoc.exe --plugin=protoc-gen-go=%plugindir%\protoc-gen-go.exe --go_out %outdir% --proto_path "." %*
protoc.exe --plugin=protoc-gen-msg=%plugindir%\protoc-gen-msg.exe --msg_out %outdir% --proto_path "." %*