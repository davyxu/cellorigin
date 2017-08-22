# cellorigin
基于cellnet和Unity3D的游戏框架


# 第三方依赖

    https://github.com/davyxu/cellnet
    https://github.com/davyxu/golexer
    https://github.com/davyxu/golog
    https://github.com/davyxu/goobjfmt
    https://github.com/davyxu/gosproto

# 文件夹功能
```
client                  // Unity客户端代码
proto                   // 协议
    tool                // 协议工具及shell
server                  // 完整服务器代码
    src
        svc             // 服务共享代码
        svcmod          // 所有服务器
            login       // 登录服务器
```




# 开发环境

* Unity3D 5.5+

* Golang 1.8+

# 协议编译

使用sproto作为协议描述及通信格式

sproto协议格式简单, 方便解析, 对lua开发较为友好


- 编译sproto协议生成器

    执行proto/tool/Install.bat

- 根据sproto协议(*.sp)文件生成代码

    执行proto/GenerateProto.bat

- 生成文件位置

    Golang: server/src/proto/msg_sp.go

    C#: client/Assets/Script/Proto/sp.cs


# 服务器开发指南

开发工具推荐使用Gogland(https://www.jetbrains.com/go/)

## 设置GOPATH

    设置项目GOPATH为server下

## 运行

    找到server/src/svcmod/login/main.go
    在main前的绿箭头点击运行


# 客户端开发指南

## 找到入口

    client/Assets/Scene/Launch.unity

    运行后, 点击hello, 将连接服务器127.0.0.1:8001端口发送LoginREQ消息并接收LoginACK消息



# 备注

感觉不错请star, 谢谢!

博客: http://www.cppblog.com/sunicdavy

知乎: http://www.zhihu.com/people/sunicdavy


