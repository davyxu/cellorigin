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

    Model -> Presenter -> View

* Command调用

    View -> Presenter

* 可见性

    Presenter可见Model

    View只可调用定义好的Command及取属性

    Model不可见任何信息


### Model

* Model的数据格式
    
    可以是Protobuf类型或者C#原生类型

* Model的功能封装
    
    Model不建议添加函数进行功能封装, 相关封装应放在Presenter中

* Model的通知
    和WPF的MVVM相比, 出于性能和复杂度考虑, 框架的Model不应具有修改后通知功能
    
### Presenter

* 大部分逻辑应该写在Present

    包括网络消息处理. 提供给View使用的功能应封装为Command暴露
    
* Presenter需要做出良好的单元测试准备

    网络消息处理都是基于函数的, 对测试系统都是友好的
    
### View

* View可以,也应该由非程序人员来设计并生成    

* View的大部分代码应该被生成

    只有需要程序控制的动画和纯界面类逻辑才能被写到View中
    
