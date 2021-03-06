package main

const codeTemplate = `// Generated by github.com/davyxu/cellnet/objprotogen
// DO NOT EDIT!
package {{.PackageName}}

{{if gt (.Structs|len) 0}}
import (
	"github.com/davyxu/cellnet"
	"github.com/davyxu/goobjfmt"
	"reflect"
)
{{end}}

{{range .Structs}}
func (self *{{.Name}}) String() string { return goobjfmt.CompactTextString(self) } {{end}}

func init() {
	{{range .Structs}}
	cellnet.RegisterMessageMeta("binary","{{$.PackageName}}.{{.Name}}", reflect.TypeOf((*{{.Name}})(nil)).Elem(), {{.MsgID}})	{{end}}
}

`

func genCode(output string, f *Package) {

	generateCode("objprotogen", codeTemplate, output, f, &generateOption{formatGoCode: true})
}
