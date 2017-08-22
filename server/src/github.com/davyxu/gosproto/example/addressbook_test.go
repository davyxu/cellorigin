package example

import (
	"math"
	"reflect"
	"testing"

	"github.com/davyxu/gosproto"
)

var abData []byte = []byte{
	1, 0, 0, 0, 122, 0, 0, 0,
	68, 0, 0, 0, 4, 0, 0,
	0, 34, 78, 1, 0, 0, 0,
	5, 0, 0, 0, 65, 108, 105,
	99, 101, 45, 0, 0, 0, 19,
	0, 0, 0, 2, 0, 0, 0,
	4, 0, 9, 0, 0, 0, 49,
	50, 51, 52, 53, 54, 55, 56,
	57, 18, 0, 0, 0, 2, 0,
	0, 0, 6, 0, 8, 0, 0,
	0, 56, 55, 54, 53, 52, 51,
	50, 49, 46, 0, 0, 0, 4,
	0, 0, 0, 66, 156, 1, 0,
	0, 0, 3, 0, 0, 0, 66,
	111, 98, 25, 0, 0, 0, 21,
	0, 0, 0, 2, 0, 0, 0,
	8, 0, 11, 0, 0, 0, 48,
	49, 50, 51, 52, 53, 54, 55,
	56, 57, 48,
}

func TestMyProfile(t *testing.T) {

	input := &MyData{
		Name:   "genji",
		Type:   MyCar_Pig,
		Uint32: math.MaxUint32,
		Int64:  math.MaxInt64,
		Uint64: math.MaxUint64,
		Stream: []byte{1, 2, 3, 4},
	}

	input.SetFloat32(3.14159265)
	input.SetFloat64(3.14159265)

	var my MyData

	encodeDecodeCompare(t, input, &my)

	t.Log(input.Type.String(), input.Float32(), input.Float64())

	assert(t, input.Name == "genji")
	assert(t, input.Type == MyCar_Pig)
	assert(t, input.Uint32 == math.MaxUint32)
	assert(t, input.Int64 == math.MaxInt64)
	assert(t, input.Uint64 == math.MaxUint64)

	t.Log(input.String())
}

func assert(t *testing.T, condition bool) {
	if !condition {
		t.FailNow()
	}
}

func TestAddressBook(t *testing.T) {

	for _, tp := range SProtoStructs {
		t.Log(tp.Name())
	}

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

	data := encodeDecodeCompare(t, input, new(AddressBook))

	if !reflect.DeepEqual(abData, data) {
		t.FailNow()
	}
}

func encodeDecodeCompare(t *testing.T, input, sample interface{}) []byte {
	data, err := sproto.Encode(input)
	//t.Log(data)

	if err != nil {
		t.Log(err)
		t.FailNow()
	}

	_, err = sproto.Decode(data, sample)

	if err != nil {
		t.Log(err)
		t.FailNow()
	}

	if !reflect.DeepEqual(sample, input) {
		t.Log("deep equal failed", input)
		t.FailNow()
	}

	return data
}
