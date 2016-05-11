: 服务器协议
call gen_go.bat ^
service.proto ^
game.proto ^
login.proto ^
tool.proto

: 客户端协议
..\tool\protoc.exe --plugin=protoc-gen-sharpnet=..\tool\protoc-gen-sharpnet.exe ^
--sharpnet_out ..\client\Assets\Script\Proto ^
--proto_path "." ^
network.proto ^
game.proto ^
client.proto ^
framework.proto ^
login.proto