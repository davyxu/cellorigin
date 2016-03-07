package share

import (
	"io/ioutil"
	"path/filepath"

	"github.com/golang/protobuf/proto"
)

func LoadPBTFile(filename string, msg proto.Message) error {

	content, err := ioutil.ReadFile(filename)

	if err != nil {
		return err
	}

	return proto.UnmarshalText(string(content), msg)
}

const dataPath = "../cfg"

func LoadTable(name string, m proto.Message) error {

	var ext = ".pbt"

	final := filepath.Join(dataPath, name) + ext

	if err := LoadPBTFile(final, m); err != nil {

		log.Errorln(err)
		return err
	}

	return nil
}
