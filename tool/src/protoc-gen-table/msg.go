package main

import (
	"strings"

	"github.com/davyxu/pbmeta"
	pbprotos "github.com/davyxu/pbmeta/proto"
)

// 取出一行的字段描述符
func getRowFD(msg *pbmeta.Descriptor) *pbmeta.FieldDescriptor {
	for i := 0; i < msg.FieldCount(); i++ {
		fd := msg.Field(i)

		if fd.IsRepeated() && fd.IsMessageType() {
			return fd
		}
	}

	return nil
}

func protoType2GoType(fd *pbmeta.FieldDescriptor) string {

	switch fd.Type() {

	case pbprotos.FieldDescriptorProto_TYPE_STRING:
		return "string"
	case pbprotos.FieldDescriptorProto_TYPE_UINT32:
		return "uint32"
	case pbprotos.FieldDescriptorProto_TYPE_INT32:
		return "int32"
	case pbprotos.FieldDescriptorProto_TYPE_FLOAT:
		return "float32"
	case pbprotos.FieldDescriptorProto_TYPE_INT64:
		return "int64"
	case pbprotos.FieldDescriptorProto_TYPE_BOOL:
		return "bool"
	}

	return "unknown"

}

// 遍历要生成key的所有字段
func iterateMapKey(rowMsgType *pbmeta.Descriptor, callback func(string, string)) {

	for i := 0; i < rowMsgType.FieldCount(); i++ {
		mapKeyFD := rowMsgType.Field(i)

		// 重复的字段和消息类型不能作为key
		if mapKeyFD.IsRepeated() || mapKeyFD.IsMessageType() {
			continue
		}

		if getOption(mapKeyFD.ParseTaggedComment()).GenMapKey {

			callback(mapKeyFD.Name(), protoType2GoType(mapKeyFD))
		}
	}

}

func printMessage(gen *Generator, msg *pbmeta.Descriptor, file *pbmeta.FileDescriptor) {

	// 一行的字段描述符
	rowMsgFD := getRowFD(msg)

	if rowMsgFD == nil {
		gen.Fail("row field not found in ", msg.Name())
		return
	}

	// 行类型
	rowMsgType := rowMsgFD.MessageDesc()

	if rowMsgType == nil {
		gen.Fail("row message type not found in ", msg.Name())
		return
	}

	// 检查命名规范

	// 表的行类型, 必须带有XXDefine
	if rowMsgFD.Name()+"Define" != rowMsgType.Define.GetName() {
		gen.Fail("row message type must named as:  xxDefine ")
		return
	}

	// 导出文件的消息名必须为XXFile
	if rowMsgFD.Name()+"File" != msg.Name() {
		gen.Fail("table file message must named as:  xxFile ")
		return
	}

	name := rowMsgFD.Name()
	lowerName := strings.ToLower(name)

	gen.Println()
	gen.Println("import (")
	gen.In()
	gen.Println("\"proto/gamedef\"")
	gen.Println("\"share\"")
	gen.Out()
	gen.Println(")")
	gen.Println()

	var keyCount int

	// 声明索引
	iterateMapKey(rowMsgType, func(keyName string, keyType string) {

		// 声明映射类型
		gen.Println("var ", name, keyName, "Map = make(map[", keyType, "]*gamedef.", name, "Define)")
		gen.Println()
		keyCount++

	})

	gen.Println("var ", lowerName, "File gamedef.", name, "File")
	gen.Println()

	gen.Println("func Load", name, "Table() {")
	gen.Println()
	gen.In()

	gen.Println("log.Infoln(\"load ", lowerName, " table...\")")
	gen.Println()

	gen.Println("if share.LoadTable(\"", name, "\", &", lowerName, "File) != nil {")
	gen.In()
	gen.Println("return")
	gen.Out()
	gen.Println("}")
	gen.Println()

	if keyCount > 0 {
		gen.Println("for _, def := range ", lowerName, "File.", name, " {")
		gen.In()

		gen.Println()

		// 建立索引
		iterateMapKey(rowMsgType, func(keyName string, keyType string) {
			gen.Println(name, keyName, "Map[def.", keyName, "] = def")
			gen.Println()
		})

		gen.Out()
		gen.Println("}")
	}

	gen.Out()
	gen.Println("}")
}
