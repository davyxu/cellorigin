# MVVM 框架

## 命名规则

* 名字单一性

    每个界面, 功能都应该有一个英文名, 且唯一. 以后简称"功能名"

* 名字可以被组合

    框架会将功能名与不同的前缀, 后缀组成新的名字, 例如: 功能名+View

* ViewModel改名叫Presenter
    
    为了便于搜索和查看, 将MVVM中的ViewModel在本框架内称为Presenter, 但并不代表使用MVP设计思想

## 关系

* 变化后的事件通知

    Presenter -> View

* Command调用

    View -> Presenter

* 可见性

    Presenter可见Model

    View只可调用定义好的Command
	View只从Presenter里取属性数据

    Model不可见任何信息


### Model

* Mode的功能
	
	提供数据

* Model的数据格式
    
    可以是Protobuf类型或者C#原生类型

* Model的数据封装
    
    Model不建议添加函数进行功能封装, 相关封装应放在Presenter中

* Model的通知

    和WPF的MVVM相比, 出于性能和复杂度考虑, 框架的Model不应具有修改后通知功能
    
### Presenter

* Presenter的功能

		为View的显示数据提供封装
	
		网络处理
	
		为View提供Command
    
* Presenter需要做出良好的单元测试准备

    网络消息处理都是基于函数的, 对测试系统都是友好的
    
### View

* View的功能

		将显示控件与Presenter提供的属性进行绑定
	
		将用户交互与Presenter对应的Command进行绑定

* View可以,也应该由非程序人员来设计并生成  
  
* View不能用来编写业务逻辑

    只有需要程序控制的动画和纯界面类逻辑才能被写到View中
    
