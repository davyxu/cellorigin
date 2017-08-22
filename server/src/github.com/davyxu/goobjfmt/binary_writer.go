package goobjfmt

import (
	"encoding/binary"
	"errors"
	"reflect"
)

var (
	ErrInvalidType = errors.New("invalid type")
	ErrOutOfData = errors.New("out of data")
)

func BinaryWrite(obj interface{}) ([]byte, error) {

	// Fallback to reflect-based encoding.
	v := reflect.Indirect(reflect.ValueOf(obj))
	size := dataSize(v, nil)
	if size < 0 {
		return nil, ErrInvalidType
	}

	buf := make([]byte, size)

	e := &encoder{order: binary.LittleEndian, buf: buf}
	e.value(v)

	return buf, nil
}
