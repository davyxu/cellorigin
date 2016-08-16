package model

import (
	"reflect"
)

// 原则: 缓存源, 比对源于目标差异, 构建差异结构
type ModelSync struct {
	valueByPath map[string]reflect.Value
	modelType   reflect.Type
}

func (self *ModelSync) SetModel(name string, value interface{}) {

	if v, ok := self.valueByPath[name]; ok {

		v.Set(reflect.ValueOf(value))
	}

}

func (self *ModelSync) GetModel(name string) (interface{}, bool) {

	if v, ok := self.valueByPath[name]; ok {

		return v.Interface(), true
	}

	return nil, false

}

func (self *ModelSync) walkMemStruct(data interface{}) {

	dataType := reflect.TypeOf(data)
	dataValue := reflect.ValueOf(data)

	if dataType.Kind() == reflect.Ptr {
		dataType = dataType.Elem()
		dataValue = dataValue.Elem()
	}

	//log.Debugln("iterate", dataType.Name(), dataType.Name())

	for i := 0; i < dataType.NumField(); i++ {

		field := dataType.Field(i)

		modelPath := field.Tag.Get("model")

		// 不需要遍历的部分跳过, 提高效率
		if modelPath == "-" {
			continue
		}

		switch field.Type.Kind() {
		case reflect.Ptr, reflect.Struct:

			// 指针和结构体都可以遍历, 且值不为空
			if dataValue.Kind() == reflect.Struct {

				self.walkMemStruct(dataValue.Field(i).Interface())
			}

		default:

			// 只有写了path的才可以导出
			if modelPath != "" {

				//log.Debugln(field.Name, field.Type, field.Type.Kind(), modelPath, dataValue.Field(i).Interface())

				self.valueByPath[modelPath] = dataValue.Field(i)
			}

		}

	}
}

func NewModelSync(v interface{}, modelType reflect.Type) *ModelSync {
	self := &ModelSync{
		valueByPath: make(map[string]reflect.Value),
		modelType:   modelType,
	}

	self.walkMemStruct(v)

	return self
}
