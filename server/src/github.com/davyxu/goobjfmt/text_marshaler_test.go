package goobjfmt

import (
	"math"
	"testing"
)

type MyCar int32

const (
	MyCar_Monkey MyCar = 1
	MyCar_Monk   MyCar = 2
	MyCar_Pig    MyCar = 3
)

type MyData struct {
	Stream []byte

	Name string

	Type MyCar

	Int32 int32

	Uint32 uint32

	Int64 int64

	Uint64 uint64
}

type PhoneNumber struct {
	Number string

	Type int32
}

type Person struct {
	Name string

	Id int32

	Email string

	Phone []*PhoneNumber
}

type AddressBook struct {
	Person []*Person

	PersonByName map[string]*Person
}

func TestNumber(t *testing.T) {

	input := &MyData{
		Stream: []byte{1, 2, 3, 4},
		Name:   "genji",
		Type:   MyCar_Pig,
		Uint32: math.MaxUint32,
		Int64:  math.MaxInt64,
		Uint64: math.MaxUint64,
	}

	t.Log(CompactTextString(input))
}

func TestString(t *testing.T) {
	t.Log(CompactTextString(&MyData{Name: "源氏"}))
}

func TestPhone(t *testing.T) {
	input := &AddressBook{
		Person: []*Person{
			{
				Name: "Alice",
				Id:   int32(10000),
				Phone: []*PhoneNumber{
					{
						Number: "123456789",
						Type:   1,
					},
					{
						Number: "87654321",
						Type:   2,
					},
				},
			},
			{
				Name: "Bob",
				Id:   int32(20000),
				Phone: []*PhoneNumber{
					{
						Number: "01234567890",
						Type:   int32(3),
					},
				},
			},
		},
	}

	input.PersonByName = make(map[string]*Person)

	for _, v := range input.Person {

		input.PersonByName[v.Name] = v
	}

	t.Log(CompactTextString(input))

	t.Log(MarshalTextString(input))

}
