package goobjfmt

import (
	"encoding/binary"
	"reflect"
)

func BinaryRead(data []byte, obj interface{}) error {

	if len(data) == 0 {
		return nil
	}

	v := reflect.ValueOf(obj)

	switch v.Kind() {
	case reflect.Ptr:
		v = v.Elem()
	}

	size := dataSize(v, nil)
	if size < 0 {
		return ErrInvalidType
	}

	if len(data) < size {
		return ErrOutOfData
	}

	d := &decoder{order: binary.LittleEndian, buf: data}
	d.value(v)

	return nil
}
