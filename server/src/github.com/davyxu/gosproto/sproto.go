package sproto

import (
	"errors"
)

var (
	ErrNonPtr    = errors.New("sproto: called with Non-Ptr type")
	ErrNonStruct = errors.New("sproto: Encode called with Non-Ptr")
	ErrNil       = errors.New("sproto: Encode called with nil")
	ErrDecode    = errors.New("sproto: Decode msg failed")
	ErrUnpack    = errors.New("sproto: Unpack data failed")
)

func Append(dst, src []byte) []byte {
	l := len(dst)
	if l+len(src) > cap(dst) {
		// allocate double what's needed, for future growth
		buf := make([]byte, (l+len(src))*2)
		copy(buf, dst)
		dst = buf
	}
	dst = dst[0 : l+len(src)]
	copy(dst[l:], src)
	return dst
}

// encode && pack
func EncodePacked(sp interface{}) ([]byte, error) {
	unpacked, err := Encode(sp)
	if err != nil {
		return nil, err
	}
	return Pack(unpacked), nil
}

// unpack && decode
func DecodePacked(data []byte, sp interface{}) error {
	unpacked, err := Unpack(data)
	if err != nil {
		return err
	}
	_, err = Decode(unpacked, sp)
	return err
}
