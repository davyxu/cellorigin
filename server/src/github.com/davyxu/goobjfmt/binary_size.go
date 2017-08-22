package goobjfmt

import (
	"reflect"
)

func dataSize(v reflect.Value, sf *reflect.StructField) int {

	switch v.Kind() {
	case reflect.Array:
		if s := dataSize(v.Elem(), nil); s >= 0 {
			return s*v.Type().Len() + 4
		}
	case reflect.Slice:
		l := v.Len()
		elemSize := int(v.Type().Elem().Size())
		return l*elemSize + 4

	case reflect.String:
		t := v.Len()
		return t + 4
	case reflect.Bool,
		reflect.Uint8, reflect.Uint16, reflect.Uint32, reflect.Uint64,
		reflect.Int8, reflect.Int16, reflect.Int32, reflect.Int64,
		reflect.Float32, reflect.Float64:
		return int(v.Type().Size())
	case reflect.Struct:
		sum := 0

		st := v.Type()

		for i := 0; i < v.NumField(); i++ {

			fv := v.Field(i)

			sf := st.Field(i)

			s := dataSize(fv, &sf)
			if s < 0 {
				return -1
			}
			sum += s
		}
		return sum

	case reflect.Int:
		panic("do not support int, use int32/int64 instead")
		//case reflect.Ptr:
		//	ev := v.Elem()
		//
		//	return dataSize(ev, sf)
		//case reflect.Invalid:
		//	return 0
		//case reflect.Interface:
		return 0
	default:

		if sf != nil && sf.Tag.Get("binary") == "-" {
			return 0
		} else {
			panic("size: unsupport kind: " + v.Kind().String())
		}

	}

	return -1
}

func BinarySize(obj interface{}) int {
	v := reflect.Indirect(reflect.ValueOf(obj))
	return dataSize(v, nil)
}
