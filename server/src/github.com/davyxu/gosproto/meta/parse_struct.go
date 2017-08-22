package meta

import (
	"errors"
)

func parseStruct(p *sprotoParser, fileD *FileDescriptor, srcName string) {

	dotToken := p.RawToken()

	p.NextToken()

	d := newDescriptor(fileD)
	d.Type = DescriptorType_Struct

	// 名字
	d.Name = p.Expect(Token_Identifier).Value()

	d.CommentGroup = p.CommentGroupByLine(dotToken.Line())

	// {
	p.Expect(Token_CurlyBraceL)

	for p.TokenID() != Token_CurlyBraceR {

		// 字段
		parseStructField(p, d)

	}

	p.Expect(Token_CurlyBraceR)

	// }

	// 名字重复检查

	if fileD.NameExists(d.Name) {
		panic(errors.New("Duplicate name: " + d.Name))
	}

	fileD.addObject(d, srcName)

}
