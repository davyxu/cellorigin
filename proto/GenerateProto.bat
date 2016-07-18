: 服务器协议
call gen_server.bat ^
service.proto ^
game.proto ^
login.proto ^
model.proto ^
tool.proto

: 客户端协议
call gen_lua.bat ^
network.proto ^
game.proto ^
client.proto ^
descriptor.proto ^
addressbook.proto ^
model.proto ^
login.proto

call gen_csharp.bat ^
network.proto