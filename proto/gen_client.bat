..\tool\protoc.exe --plugin=protoc-gen-sharpnet=..\tool\protoc-gen-sharpnet.exe ^
--sharpnet_out use_hasfield:..\client\Assets\Script\Proto ^
--proto_path "." %*

..\tool\protoc.exe --descriptor_set_out=..\client\Assets\game.pb --proto_path "." %*