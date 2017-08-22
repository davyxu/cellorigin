package main

import (
	"strings"

	"github.com/davyxu/gosproto/meta"
)

type fieldModel struct {
	*meta.FieldDescriptor
	FieldIndex int

	st *structModel
}

func (self *fieldModel) UpperName() string {
	return strings.ToUpper(string(self.Name[0])) + self.Name[1:]
}

type structModel struct {
	*meta.Descriptor

	StFields []fieldModel

	f *fileModel
}


func (self *structModel) IsResultEnum() bool {
	return self.IsEnum() && strings.HasSuffix(self.Name, "Result")
}

func (self *structModel) IsEnum() bool {
	return self.Type == meta.DescriptorType_Enum
}

func (self *structModel) IsStruct() bool {
	return self.Type == meta.DescriptorType_Struct
}

func (self *structModel) MsgID() uint32 {
	return StringHash(self.MsgFullName())
}

func (self *structModel) MsgFullName() string {
	return self.f.PackageName + "." + self.Name
}
func (self *structModel) FieldCount() int {
	return len(self.StFields)
}

type fileModel struct {
	*meta.FileDescriptorSet

	Structs []*structModel

	Enums []*structModel

	Objects []*structModel

	PackageName string

	CellnetReg bool

	forceAutoTag bool

	CSClassAttr string

	CSFieldAttr string

	EnumValueGroup bool
}

func (self *fileModel) Len() int {
	return len(self.Structs)
}

func (self *fileModel) Swap(i, j int) {
	self.Structs[i], self.Structs[j] = self.Structs[j], self.Structs[i]
}

func (self *fileModel) Less(i, j int) bool {

	a := self.Structs[i]
	b := self.Structs[j]

	return a.Name < b.Name
}

func addStruct(fm *fileModel, fileD *meta.FileDescriptor, srcName string) {

	for _, st := range fileD.Objects {

		// 过滤, 只输出某个来源
		if srcName != "" && st.SrcName != srcName {
			continue
		}

		stModel := &structModel{
			Descriptor: st,
		}

		for index, fd := range st.Fields {

			fdModel := fieldModel{
				FieldDescriptor: fd,
				FieldIndex:      index,
				st:              stModel,
			}

			stModel.StFields = append(stModel.StFields, fdModel)

		}

		stModel.f = fm

		fm.Objects = append(fm.Objects, stModel)

		switch stModel.Type {
		case meta.DescriptorType_Enum:
			fm.Enums = append(fm.Enums, stModel)
		case meta.DescriptorType_Struct:
			fm.Structs = append(fm.Structs, stModel)
		}

	}
}

func addData(fm *fileModel, matchTag string) {

	for _, file := range fm.FileDescriptorSet.Files {

		if file.MatchTag(matchTag) {
			addStruct(fm, file, "")
		}

	}

}
