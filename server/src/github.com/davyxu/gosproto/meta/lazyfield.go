package meta

import (
	"errors"
	"fmt"
	"github.com/davyxu/golexer"
)

type lazyField struct {
	typeName      string
	mainIndexName string

	fd *FieldDescriptor

	d *Descriptor

	tp golexer.TokenPos

	miss bool
}

func newLazyField(typeName string, fd *FieldDescriptor, d *Descriptor, tp golexer.TokenPos) *lazyField {
	return &lazyField{typeName: typeName,
		fd: fd,
		d:  d,
		tp: tp}
}

func (self *lazyField) resolve(pass int) (bool, error) {

	self.fd.Type, self.fd.Complex = self.fd.parseType(self.typeName)

	if self.fd.Type == FieldType_None {
		if pass > 1 {

			fmt.Println(self.tp.String())

			return true, errors.New("type not found: " + self.typeName)
		} else {

			self.miss = true
			return true, nil
		}
	}

	if self.mainIndexName != "" {
		if indexFd, ok := self.fd.Complex.FieldByName[self.mainIndexName]; ok {
			self.fd.MainIndex = indexFd
		} else {
			if pass > 1 {
				return true, errors.New("Main index not found:" + self.mainIndexName)
			} else {
				return true, nil
			}

		}
	}

	return false, nil
}
