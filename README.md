# cellorigin
基于cellnet的服务器框架, 以及配套客户端框架

# 定位
项目的快速启动框架

# 特性

* 基于Protobuf的导表工具及通用数据配置系统

* 配置表索引,关联代码自动生成

* 基于UGUI的UI控件绑定代码自动生成


# 文档请参考
https://github.com/davyxu/cellorigin/tree/master/doc

# 开发环境

* Unity3D 5.x+

* Golang 1.5+

# TODO

* 基于代码生成的自动Model数据更新系统

* 网络底层的支持连接失败(而不是与断开混合)事件

* UI单元模块联网开发工作流


# 信条
编程不仅为了写正确的代码, 更是为了找到聪明解决问题的方案

**项目施工中, 敬请期待**


# 开发日志

* 添加ModelManager

	ModelManager挂接在全局对象上, 并且在所有Model被使用前最先初始化
	
	初始化时, 会自动注册所有的Model
	
* 添加客户端全局事件系统EventEmitter

	可以直接通过数据类型进行调用
	
	
* 添加UI代码自动化代码生成工具

	通过分析当前场景内对象树的结构,再自动生成绑定代码
	没有主代码时, button可以一并生成
	
* 添加基础网络代码

	NetworkPeer通过设定名字和地址可以迅速连接服务器