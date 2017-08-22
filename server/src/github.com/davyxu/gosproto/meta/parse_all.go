package meta

import (
	"errors"
	"fmt"
	"github.com/davyxu/golexer"
	"io/ioutil"
)

func ParseFile(fileName string) (*FileDescriptorSet, error) {

	fileset := NewFileDescriptorSet()

	fileD := NewFileDescriptor()
	fileD.FileName = fileName

	err := rawPaseFile(fileD, fileName)
	if err != nil {
		return nil, err
	}

	fileset.addFile(fileD)

	return fileset, fileset.resolveAll()
}

func ParseFileList(fileset *FileDescriptorSet, filelist []string) (string, error) {

	for _, filename := range filelist {

		fileD := NewFileDescriptor()
		fileD.FileName = filename
		fileset.addFile(fileD)

		if err := rawPaseFile(fileD, filename); err != nil {
			return filename, err
		}

	}

	return "", fileset.resolveAll()

}

// 从文件解析
func rawPaseFile(fileD *FileDescriptor, fileName string) error {

	data, err := ioutil.ReadFile(fileName)
	if err != nil {
		return err
	}

	return rawParse(fileD, string(data), fileName)
}

// 解析字符串
func rawParse(fileD *FileDescriptor, data string, srcName string) (retErr error) {

	p := newSProtoParser(srcName)

	defer golexer.ErrorCatcher(func(err error) {

		retErr = fmt.Errorf("%s %s", p.PreTokenPos().String(), err.Error())

	})

	p.Lexer().Start(data)

	p.NextToken()

	for p.TokenID() != Token_EOF {

		switch p.TokenID() {
		case Token_Dot, Token_Message:
			parseStruct(p, fileD, srcName)
		case Token_Enum:
			parseEnum(p, fileD, srcName)
		case Token_FileTag:
			parseFileTag(p, fileD, srcName)
		default:
			panic(errors.New("Unknown token: " + p.TokenValue()))
		}

	}

	return nil
}
