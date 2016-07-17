: 服务器协议
call gen_server.bat ^
service.proto ^
game.proto ^
login.proto ^
tool.proto

: 客户端协议
call gen_client.bat ^
network.proto ^
game.proto ^
client.proto ^
framework.proto ^
descriptor.proto ^
addressbook.proto ^
login.proto