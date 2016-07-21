# Windows编译服务器流程

## 准备资源
* golang最新windows安装包
* LiteIDE开发环境

## 推荐目录结构

```txt

\opensource
	src
		github.com
			davyxu
				cellnet
				golog
				pbmeta
				protobuf
				protoc-gen-sharpnet
				tabtoy
			golang
			
\cellorigin\


```


## 如果编译
在LiteIDE的菜单中:查看->管理GOPATH对话框的自定义目录中按如下顺序填写路径(请脑补盘符)

\opensource
\cellorigin\server
\cellorigin\tool

在agentsvc,gamesvc, loginsvc中分别在菜单中选择Go Install

服务器二进制会生成在server/bin下