package sproto_test

import (
	"bytes"
	"testing"

	"github.com/davyxu/gosproto"
)

type PackTestCase struct {
	Name     string
	Unpacked []byte
	Packed   []byte
}

var packTestCases []*PackTestCase = []*PackTestCase{
	&PackTestCase{
		Name:     "SimplePack",
		Unpacked: []byte{0x08, 0x00, 0x00, 0x00, 0x03, 0x00, 0x02, 0x00, 0x19, 0x00, 0x00, 0x00, 0xaa, 0x01, 0x00, 0x00},
		Packed:   []byte{0x51, 0x08, 0x03, 0x02, 0x31, 0x19, 0xaa, 0x01},
	},
	&PackTestCase{
		Name: "FFPack",
		Unpacked: bytes.Join([][]byte{
			bytes.Repeat([]byte{0x8a}, 30),
			[]byte{0x00, 0x00},
		}, nil),
		Packed: bytes.Join([][]byte{
			[]byte{0xff, 0x03},
			bytes.Repeat([]byte{0x8a}, 30),
			[]byte{0x00, 0x00},
		}, nil),
	},
}

func TestPack(t *testing.T) {
	var allUnpacked, allPacked []byte
	for _, tc := range packTestCases {
		packed := sproto.Pack(tc.Unpacked)
		if !bytes.Equal(packed, tc.Packed) {
			t.Log("packed:", packed)
			t.Log("expected:", tc.Packed)
			t.Fatalf("test case *%s* failed", tc.Name)
		}
		allUnpacked = sproto.Append(allUnpacked, tc.Unpacked)
		allPacked = sproto.Append(allPacked, packed)
	}

	packed := sproto.Pack(allUnpacked)
	if !bytes.Equal(packed, allPacked) {
		t.Log("packed:", packed)
		t.Log("expected:", allPacked)
		t.Fatal("test case *total* failed")
	}
}

func TestUnpack(t *testing.T) {
	var allUnpacked, allPacked []byte
	for _, tc := range packTestCases {
		unpacked, err := sproto.Unpack(tc.Packed)
		if err != nil {
			t.Fatalf("test case *%s* failed with error:%s", tc.Name, err)
		}
		if !bytes.Equal(unpacked, tc.Unpacked) {
			t.Log("unpacked:", unpacked)
			t.Log("expected:", tc.Unpacked)
			t.Fatalf("test case *%s* failed", tc.Name)
		}
		allUnpacked = sproto.Append(allUnpacked, tc.Unpacked)
		allPacked = sproto.Append(allPacked, tc.Packed)
	}
	unpacked, err := sproto.Unpack(allPacked)
	if err != nil {
		t.Fatalf("test case *total* failed with error:%s", err)
	}
	if !bytes.Equal(unpacked, allUnpacked) {
		t.Log("unpacked:", unpacked)
		t.Log("expected:", allUnpacked)
		t.Fatal("test case *total* failed")
	}
}
