# cellorigin
基于cellnet的游戏服务器框架


本框架定位为可以为项目启动的快速框架

整体设计为高效,便捷, 复用的原则

Protobuf为核心的大量自动代码

多年积累的导表工具为基础构建的数据配置框架


**项目施工中, 完善后将开放游览, 敬请期待**



# 目录结构

proto	协议,配置定义

server	服务器代码, GOPATH

	bin	服务器二进制
	
	cfg 服务器配置
	
	src
	
		agentsvc	网关服务器及组件代码
		
		gamesvc	游戏服务器及组件代码
		
		loginsvc 登陆服务器组件代码
		
		robotsvc 机器人组件代码
		
		proto	生成的协议,结构代码
		
		share	服务共享的代码
		
		table	表格读取及接口代码
		
table	表格,数据, 配置

tool	工具及代码, GOPATH

# 服务器互联关系

## 平台验证
client -> loginsvc

## 逻辑
client -> agentsvc

## 游戏与网关上下行通信
gamesvc -> agentsvc



# 与以往设计区别
* game和agent不再与login连接, 而是通过GM服务器维护登陆地址和状态, 降低服务器耦合度

