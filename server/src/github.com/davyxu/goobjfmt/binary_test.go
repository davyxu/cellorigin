package goobjfmt

import (
	"fmt"
	"testing"
)

type P struct {
	X, Y, Z int32

	Name string
}

//func TestPtr(t *testing.T) {
//
//	var v int32 = 100
//
//	data, err := BinaryWrite(&struct {
//		Ignore *int32
//	}{
//		&v,
//	})
//
//	if err != nil {
//		fmt.Println(err)
//	} else {
//		fmt.Println(data)
//	}
//
//	var out struct {
//		Ignore *int32
//	}
//
//	err = BinaryRead(data, &out)
//
//	if err != nil {
//		fmt.Println(err)
//	} else {
//		fmt.Println(*out.Ignore)
//	}
//
//	if *out.Ignore != v {
//		t.FailNow()
//	}
//
//}

func TestBinaryIgnore(t *testing.T) {

	data, err := BinaryWrite(&struct {
		Ignore int32 `binary:"-"`
	}{
		100,
	})

	if err != nil {
		fmt.Println(err)
	} else {
		fmt.Println(data)
	}
}

func TestWrite(t *testing.T) {

	var input P
	input.Name = "hello"
	input.X = 4

	data, err := BinaryWrite(&input)

	if err != nil {
		fmt.Println(err)
	} else {
		fmt.Println(data)
	}

	var p2 P
	err = BinaryRead(data, &p2)

	if err != nil {
		fmt.Println(err)
	} else {
		fmt.Println(p2)
	}
}

type RemoteCallACK struct {
	MsgID  uint32
	Data   []byte
	CallID int32
}

func TestWrite2(t *testing.T) {

	input := &RemoteCallACK{123, []byte{1, 34}, 34}

	data, err := BinaryWrite(input)

	if err != nil {
		fmt.Println(err)
	} else {
		fmt.Println(data)
	}

	inputsize := BinarySize(input)

	if len(data) != inputsize {
		t.Failed()
	}

	var p2 RemoteCallACK
	err = BinaryRead(data, &p2)

	if err != nil {
		fmt.Println(err)
	} else {
		fmt.Println(p2)
	}
}
