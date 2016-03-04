call gen_go.bat ^
channel.proto ^
service.proto ^
game.proto ^
tool.proto


..\tool\protoc.exe --plugin=protoc-gen-sharpnet=..\tool\protoc-gen-sharpnet.exe --sharpnet_out ..\client\Assets\Script\Proto --proto_path "." ^
game.proto