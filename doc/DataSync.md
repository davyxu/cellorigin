# 客户端服务器数据同步

每个模块在框架下的客户端与服务器同步的内容有一定的规则

```protobuf
message ModuleDataToClient
{
	DataA
	
	DataB
	
	DataC
	
	repeated DataX
	
	repeated DataY
	
	repeated DataZ
}
```

单个字段的同步很方便

数组字段的同步稍显复杂, 主要操作有增删改


```protobuf
message ModuleSetToClient
{
	repeated DataToAdd
	repeated DataToDeleteID
	repeated DataToModify
}

```