package meta

import (
	"fmt"
	"strconv"
)

type FieldDescriptor struct {
	*CommentGroup
	Name      string
	Type      FieldType
	Tag       int
	AutoTag   int
	Repeatd   bool
	MainIndex *FieldDescriptor
	Complex   *Descriptor

	Struct *Descriptor
}

func (self *FieldDescriptor) IsExtendType() bool {

	switch self.Type {
	case FieldType_Float32,
		FieldType_Float64:
		return true
	}

	return false
}

func (self *FieldDescriptor) ExtendTypePrecision() int {

	var precision int = 1000
	if precisionStr, ok := self.CommentGroup.MatchTag("ExtendPrecision"); ok {
		if pcs, err := strconv.ParseInt(precisionStr, 10, 64); err == nil {
			precision = int(pcs)
		}
	}

	switch self.Type {
	case FieldType_Float32,
		FieldType_Float64:
		return precision
	}

	return 0
}

func (self *FieldDescriptor) TagNumber() int {

	var tag int

	if self.AutoTag == -1 {
		tag = self.Tag
	} else {
		tag = self.AutoTag
	}

	if tag != 0 {
		return self.Struct.TagBase + tag
	}

	return tag
}

func (self *FieldDescriptor) TypeString() string {
	return self.typeStr(false)
}

func (self *FieldDescriptor) CompatibleTypeString() string {
	return self.typeStr(true)
}

func (self *FieldDescriptor) typeStr(compatible bool) (ret string) {

	if self.Repeatd {
		if compatible {
			ret = "*"
		} else {
			ret = "[]"
		}

	}

	if compatible {
		ret += self.CompatibleTypeName()
	} else {
		ret += self.TypeName()
	}

	if self.MainIndex != nil {
		ret += fmt.Sprintf("(%s)", self.MainIndex.Name)
	}

	return
}

func (self *FieldDescriptor) String() string {

	return fmt.Sprintf("%s %d : %s", self.Name, self.TagNumber(), self.TypeString())
}

func (self *FieldDescriptor) Kind() string {

	return self.Type.String()
}

func (self *FieldDescriptor) CompatibleTypeName() string {

	switch self.Type {

	case FieldType_Struct:
		return self.Complex.Name
	case FieldType_Int32,
		FieldType_Int64,
		FieldType_UInt32,
		FieldType_UInt64,
		FieldType_Float32,
		FieldType_Float64,
		FieldType_Enum:
		return FieldType_Integer.String()
	case FieldType_Bytes:
		return FieldType_String.String()
	default:
		return self.Type.String()
	}

}

func (self *FieldDescriptor) TypeName() string {

	switch self.Type {
	case FieldType_Bool:
		return "bool"

	case FieldType_Struct, FieldType_Enum:
		return self.Complex.Name
	default:
		return self.Type.String()
	}

}

func (self *FieldDescriptor) parseType(name string) (ft FieldType, structType *Descriptor) {

	ft = ParseFieldType(name)

	if ft != FieldType_None {
		return ft, nil
	}

	if ft, structType = self.Struct.File.FileSet.parseType(name); ft != FieldType_None {
		return ft, structType
	}

	return FieldType_None, nil

}

func newFieldDescriptor(d *Descriptor) *FieldDescriptor {
	return &FieldDescriptor{
		CommentGroup: newCommentGroup(),
		Struct:       d,
		AutoTag:      -1,
	}
}
