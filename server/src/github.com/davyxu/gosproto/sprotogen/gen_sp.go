package main

import (
	"bufio"
	"bytes"
	"fmt"
	"strconv"
	"strings"

	"github.com/davyxu/gosproto/meta"
)

const spCodeTemplate = `
{{range $a, $obj := .Objects}}
{{.SpLeadingComment}}
{{.TypeName}} {{.Name}} {
	{{range .StFields}}{{.SpLeadingComment}}{{if $obj.IsStruct}}
	{{.SpFieldString}}	
	{{else}}
	{{.Name}} = {{.TagNumber}}{{.SpTrailingComment}}
	{{end}}	{{end}}
}
{{end}}

`

func addCommentSignAtEachLine(sign, comment string) string {

	if comment == "" {
		return ""
	}
	var out bytes.Buffer

	scanner := bufio.NewScanner(strings.NewReader(comment))

	var index int
	for scanner.Scan() {

		if index > 0 {
			out.WriteString("\n")
		}

		out.WriteString(sign)
		out.WriteString(scanner.Text())

		index++
	}

	return out.String()
}

func (self *fieldModel) SpFieldString() string {
	if self.st.f.forceAutoTag {
		return fmt.Sprintf("%s %s%s", self.Name, self.TypeString(), self.SpTrailingComment())
	} else {
		return fmt.Sprintf("%s %s %s%s", self.Name, self.TagString(), self.TypeString(), self.SpTrailingComment())
	}
}

func (self *fieldModel) SpTrailingComment() string {

	return addCommentSignAtEachLine("//", self.Trailing)
}

func (self *fieldModel) TagString() string {
	if self.AutoTag == -1 {
		return strconv.Itoa(self.Tag)
	}

	return ""
}

func (self *fieldModel) SpLeadingComment() string {

	return addCommentSignAtEachLine("//", self.Leading)
}

func (self *structModel) SpLeadingComment() string {

	return addCommentSignAtEachLine("//", self.Leading)
}

func (self *structModel) TypeName() string {
	switch self.Type {
	case meta.DescriptorType_Enum:
		return "enum"
	case meta.DescriptorType_Struct:
		return "message"
	}

	return "none"
}

func gen_sp(fileset *meta.FileDescriptorSet, forceAutoTag bool) {

	//for srcName := range fileD.ObjectsBySrcName {
	//	fm := &fileModel{
	//		FileDescriptorSet: fileset,
	//		forceAutoTag:      forceAutoTag,
	//	}
	//
	//	addStruct(fm, fileD, srcName)
	//
	//	generateCode("sp->sp", spCodeTemplate, srcName, fm, nil)
	//}

}
