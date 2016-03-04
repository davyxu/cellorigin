# cellorigin
基于cellnet的游戏服务器框架


本框架定位为可以为项目启动的快速框架

整体设计为高效,便捷, 复用的原则

Protobuf为核心的大量自动代码

多年积累的导表工具为基础构建的数据配置框架


项目施工中, 完善后将开放游览, 敬请期待



目录结构
proto	协议,配置定义
server	服务器代码, GOPATH
	bin	服务器二进制
	cfg 服务器配置
	src
		gamesvc	游戏服务器及组件代码
		gatesvc	网关服务器及组件代码
		proto	生成的协议,结构代码
		share	服务共享的代码
		table	表格读取及接口代码
table	表格,数据, 配置
tool	工具及代码, GOPATH