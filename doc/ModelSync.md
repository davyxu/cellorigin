# 客户端服务器数据同步

```protobuf
message LoginModelBundle
{
	LoginModel Login = 1;
	
	repeated ServerInfo ServerList = 2;
	
	repeated SimpleCharInfo CharList = 3;
}



```

* 客户端与服务器同步基于protobuf的message结构

* 不同模块的ModelBundle可以是分离的, 但是ModelName必须全局唯一, 例如代码中的: Login, ServerList和CharList

* 客户端会将不同个ModelBundle合并到全局中, 通过ModelName访问