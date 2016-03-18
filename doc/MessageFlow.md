# 消息流

# 平台SDK
* Client 通过SDK -> 第三方账号服务器 验证
	
	客户端取回第三方token/uid等信息

# 登录服务器 验证
* LoginREQ -> LoginACK

	服务器到第三方服务器验证

	客户端获取到服务器列表和私有token


# 进入游戏
* VerifyGameREQ -> VerifyGameACK  

	服务器验证私有token, 并返回

	客户端获取到角色列表

* EnterGameREQ -> EnterGameACK

	服务器根据选定的角色取出完整数据
	
	客户端获取到 角色数据, 初始化所有组件



# 与以往设计区别
* game和agent不再与login连接, 而是通过GM服务器维护登陆地址和状态, 降低服务器耦合度

