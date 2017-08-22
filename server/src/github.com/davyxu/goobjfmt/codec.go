package goobjfmt

import (
	"encoding/binary"
	"math"
	"reflect"
)

type coder struct {
	order binary.ByteOrder
	buf   []byte
}

type decoder coder
type encoder coder

func (self *decoder) bool() bool {
	x := self.buf[0]
	self.buf = self.buf[1:]
	return x != 0
}

func (self *encoder) bool(x bool) {
	if x {
		self.buf[0] = 1
	} else {
		self.buf[0] = 0
	}
	self.buf = self.buf[1:]
}

func (self *decoder) uint8() uint8 {
	x := self.buf[0]
	self.buf = self.buf[1:]
	return x
}

func (self *encoder) uint8(x uint8) {
	self.buf[0] = x
	self.buf = self.buf[1:]
}

func (self *decoder) uint16() uint16 {
	x := self.order.Uint16(self.buf[0:2])
	self.buf = self.buf[2:]
	return x
}

func (self *encoder) uint16(x uint16) {
	self.order.PutUint16(self.buf[0:2], x)
	self.buf = self.buf[2:]
}

func (self *decoder) uint32() uint32 {
	x := self.order.Uint32(self.buf[0:4])
	self.buf = self.buf[4:]
	return x
}

func (self *encoder) uint32(x uint32) {
	self.order.PutUint32(self.buf[0:4], x)
	self.buf = self.buf[4:]
}

func (self *decoder) uint64() uint64 {
	x := self.order.Uint64(self.buf[0:8])
	self.buf = self.buf[8:]
	return x
}

func (self *encoder) uint64(x uint64) {
	self.order.PutUint64(self.buf[0:8], x)
	self.buf = self.buf[8:]
}
func (self *decoder) bytes() []byte {
	l := self.int32()
	buf := make([]byte, l)

	copy(buf, self.buf[0:l])
	self.buf = self.buf[l:]
	return buf
}

func (self *encoder) bytes(x []byte) {
	l := len(x)
	self.int32(int32(l))
	copy(self.buf, []byte(x))
	self.buf = self.buf[l:]
}

func (self *decoder) int8() int8 { return int8(self.uint8()) }

func (self *encoder) int8(x int8) { self.uint8(uint8(x)) }

func (self *decoder) int16() int16 { return int16(self.uint16()) }

func (self *encoder) int16(x int16) { self.uint16(uint16(x)) }

func (self *decoder) int32() int32 { return int32(self.uint32()) }

func (self *encoder) int32(x int32) { self.uint32(uint32(x)) }

func (self *decoder) int64() int64 { return int64(self.uint64()) }

func (self *encoder) int64(x int64) { self.uint64(uint64(x)) }

func (self *decoder) value(v reflect.Value) {
	switch v.Kind() {
	case reflect.Array:
		l := int(self.int32())
		for i := 0; i < l; i++ {
			self.value(v.Index(i))
		}

	case reflect.Struct:
		t := v.Type()
		l := v.NumField()
		for i := 0; i < l; i++ {
			// Note: Calling v.CanSet() below is an optimization.
			// It would be sufficient to check the field name,
			// but creating the StructField info for each field is
			// costly (run "go test -bench=ReadStruct" and compare
			// results when making changes to this code).
			if vv := v.Field(i); v.CanSet() || t.Field(i).Name != "_" {
				self.value(vv)
			} else {
				self.skip(vv)
			}
		}
	case reflect.String:
		v.SetString(string(self.bytes()))

	case reflect.Slice:

		if v.Type().Elem().Kind() == reflect.Uint8 {

			v.SetBytes(self.bytes())

		} else {
			l := int(self.int32())
			slice := reflect.MakeSlice(v.Type(), l, l)

			for i := 0; i < l; i++ {

				sliceValue := reflect.New(slice.Type().Elem()).Elem()

				self.value(sliceValue)
				slice.Index(i).Set(sliceValue)
			}

			v.Set(slice)
		}

	case reflect.Bool:
		v.SetBool(self.bool())

	case reflect.Int8:
		v.SetInt(int64(self.int8()))
	case reflect.Int16:
		v.SetInt(int64(self.int16()))
	case reflect.Int32:
		v.SetInt(int64(self.int32()))
	case reflect.Int64:
		v.SetInt(self.int64())

	case reflect.Uint8:
		v.SetUint(uint64(self.uint8()))
	case reflect.Uint16:
		v.SetUint(uint64(self.uint16()))
	case reflect.Uint32:
		v.SetUint(uint64(self.uint32()))
	case reflect.Uint64:
		v.SetUint(self.uint64())

	case reflect.Float32:
		v.SetFloat(float64(math.Float32frombits(self.uint32())))
	case reflect.Float64:
		v.SetFloat(math.Float64frombits(self.uint64()))
	case reflect.Interface:

	//case reflect.Ptr:
	//
	//	valuePtr := reflect.New(v.Type().Elem())
	//
	//	self.value(valuePtr.Elem())
	//
	//	v.Set(valuePtr)

	default:
		panic("encode: unsupport kind: " + v.Kind().String())
	}
}

func (self *encoder) value(v reflect.Value) {
	switch v.Kind() {
	case reflect.Array:
		l := v.Len()
		self.int32(int32(l))
		for i := 0; i < l; i++ {
			self.value(v.Index(i))
		}

	case reflect.Struct:
		t := v.Type()
		l := v.NumField()
		for i := 0; i < l; i++ {

			v := v.Field(i)

			tt := t.Field(i)

			tag := tt.Tag.Get("binary")

			if (v.CanSet() || tt.Name != "_") && tag != "-" {
				self.value(v)
			} else {
				self.skip(v)
			}
		}

	case reflect.Slice:

		if v.Type().Elem().Kind() == reflect.Uint8 {

			self.bytes(v.Bytes())

		} else {
			l := v.Len()
			self.int32(int32(l))
			for i := 0; i < l; i++ {
				self.value(v.Index(i))
			}
		}

	case reflect.String:
		self.bytes([]byte(v.String()))

	case reflect.Bool:
		self.bool(v.Bool())

	case reflect.Int, reflect.Int8, reflect.Int16, reflect.Int32, reflect.Int64:
		switch v.Type().Kind() {
		case reflect.Int8:
			self.int8(int8(v.Int()))
		case reflect.Int16:
			self.int16(int16(v.Int()))
		case reflect.Int32:
			self.int32(int32(v.Int()))
		case reflect.Int64:
			self.int64(v.Int())
		}

	case reflect.Uint, reflect.Uint8, reflect.Uint16, reflect.Uint32, reflect.Uint64, reflect.Uintptr:
		switch v.Type().Kind() {
		case reflect.Uint8:
			self.uint8(uint8(v.Uint()))
		case reflect.Uint16:
			self.uint16(uint16(v.Uint()))
		case reflect.Uint32:
			self.uint32(uint32(v.Uint()))
		case reflect.Uint64:
			self.uint64(v.Uint())
		}

	case reflect.Float32, reflect.Float64:
		switch v.Type().Kind() {
		case reflect.Float32:
			self.uint32(math.Float32bits(float32(v.Float())))
		case reflect.Float64:
			self.uint64(math.Float64bits(v.Float()))
		}
	//case reflect.Ptr:
	//	self.value(v.Elem())
	//case reflect.Invalid:

	default:
		panic("encode: unsupport kind: " + v.Kind().String())
	}
}

func (self *decoder) skip(v reflect.Value) {
	self.buf = self.buf[dataSize(v, nil):]
}

func (self *encoder) skip(v reflect.Value) {
	n := dataSize(v, nil)
	for i := range self.buf[0:n] {
		self.buf[i] = 0
	}
	self.buf = self.buf[n:]
}
