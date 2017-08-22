package meta

import (
	"fmt"
)

type FieldType int

const (
	FieldType_None FieldType = iota
	FieldType_Integer
	FieldType_Int32
	FieldType_Int64
	FieldType_UInt32
	FieldType_UInt64
	FieldType_Bool
	FieldType_String
	FieldType_Struct
	FieldType_Enum
	FieldType_Float32
	FieldType_Float64
	FieldType_Bytes
)

var fieldtypeByStr = map[string]FieldType{
	"boolean": FieldType_Bool,
	"integer": FieldType_Integer,
	"int32":   FieldType_Int32,
	"int64":   FieldType_Int64,
	"uint32":  FieldType_UInt32,
	"uint64":  FieldType_UInt64,
	"string":  FieldType_String,
	"struct":  FieldType_Struct,
	"enum":    FieldType_Enum,
	"float32": FieldType_Float32,
	"float64": FieldType_Float64,
	"bytes":   FieldType_Bytes,
}

var strByFieldtype = map[FieldType]string{}

func init() {
	for k, v := range fieldtypeByStr {
		strByFieldtype[v] = k
	}
}

func ParseFieldType(str string) FieldType {

	if t, ok := fieldtypeByStr[str]; ok {
		return t
	}

	if str == "bool" {
		return FieldType_Bool
	}

	return FieldType_None
}

func (self FieldType) String() string {

	if v, ok := strByFieldtype[self]; ok {
		return v
	}

	return fmt.Sprintf("none(%d)", self)
}
