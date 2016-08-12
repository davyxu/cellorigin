
# 目录结构

* proto
	协议,配置定义

* server
	服务器代码, GOPATH

	* bin
		服务器二进制
	
	* cfg
		服务器配置
	
	* src
	
		* agentsvc
			网关服务器及组件代码
		
		* gamesvc
			游戏服务器及组件代码
		
		* loginsvc
			登陆服务器组件代码
		
		* robotsvc
			机器人组件代码
			
		* backend
			后台与网关通信模块
			
		* behavior
			行为系统, 整个服务器的核心框架
		
		* proto
			生成的协议,结构代码
		
		* share
			服务共享的代码
		
		* table
			表格读取及接口代码
		
* table
	表格,数据, 配置

* tool
	工具及代码, GOPATH