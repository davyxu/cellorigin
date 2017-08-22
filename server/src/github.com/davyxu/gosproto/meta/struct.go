package meta

import (
	"bytes"
	"errors"
)

type DescriptorType int

const (
	DescriptorType_None DescriptorType = iota
	DescriptorType_Enum
	DescriptorType_Struct
)

type Descriptor struct {
	*CommentGroup
	Name    string
	SrcName string
	Type    DescriptorType

	Fields      []*FieldDescriptor
	FieldByName map[string]*FieldDescriptor
	FieldByTag  map[int]*FieldDescriptor

	File *FileDescriptor

	TagBase int

	EnumValueIgnoreType bool
}

func (self *Descriptor) MaxTag() (ret int) {

	for _, fd := range self.Fields {
		if fd.TagNumber() > ret {
			ret = fd.TagNumber()
		}

	}

	return
}

func (self *Descriptor) TypeName() string {
	switch self.Type {
	case DescriptorType_Enum:
		return "enum"
	case DescriptorType_Struct:
		return "struct"
	}

	return "none"
}

// c# 要使用的fieldcount
func (self *Descriptor) MaxFieldCount() int {
	maxn := len(self.Fields)
	lastTag := -1

	for _, fd := range self.Fields {
		if fd.TagNumber() < lastTag {
			panic(errors.New("tag must in ascending order"))
		}

		if fd.TagNumber() > lastTag+1 {
			maxn++
		}

		lastTag = fd.TagNumber()
	}

	return maxn
}

func (self *Descriptor) String() string {

	var bf bytes.Buffer

	bf.WriteString(self.Name)

	bf.WriteString(":")
	bf.WriteString("\n")

	for _, fd := range self.Fields {
		bf.WriteString("	")
		bf.WriteString(fd.String())
		bf.WriteString("\n")
	}

	bf.WriteString("\n")

	return bf.String()
}

func (self *Descriptor) addField(fd *FieldDescriptor) {
	self.Fields = append(self.Fields, fd)
	self.FieldByName[fd.Name] = fd
	self.FieldByTag[fd.Tag] = fd
}

func newDescriptor(f *FileDescriptor) *Descriptor {
	return &Descriptor{
		CommentGroup: newCommentGroup(),
		File:         f,
		FieldByName:  make(map[string]*FieldDescriptor),
		FieldByTag:   make(map[int]*FieldDescriptor),
	}
}
