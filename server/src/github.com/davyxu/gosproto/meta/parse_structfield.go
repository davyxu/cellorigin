package meta

import (
	"errors"
	"fmt"
)

func parseStructField(p *sprotoParser, d *Descriptor) {

	fd := newFieldDescriptor(d)

	nameToken := p.RawToken()
	// 字段名
	fd.Name = p.Expect(Token_Identifier).Value()

	if _, ok := d.FieldByName[fd.Name]; ok {
		panic(errors.New("Duplicate field name: " + d.Name))
	}

	if p.TokenID() == Token_Numeral {
		// tag
		fd.Tag = p.Expect(Token_Numeral).ToInt()
	} else { // 没写就自动生成

		if len(d.Fields) == 0 {
			fd.AutoTag = 0
		} else {
			fd.AutoTag = d.MaxTag() + 1
		}

	}

	if p.TokenID() == Token_Colon {
		p.NextToken()
	}

	tp := p.TokenPos()

	var typeName string

	switch p.TokenID() {
	// 数组
	case Token_Star:
		p.NextToken()

		fd.Repeatd = true

		typeName = p.Expect(Token_Identifier).Value()
	case Token_BracketL:
		p.NextToken()
		p.Expect(Token_BracketR)
		fd.Repeatd = true

		typeName = p.Expect(Token_Identifier).Value()

	case Token_Identifier:
		// 普通字段
		typeName = p.TokenValue()
		p.NextToken()
		break
	default:
	}

	// 根据类型名查找类型及结构体类型

	pf := newLazyField(typeName, fd, d, tp)

	// map的索引解析 (
	if p.TokenID() == Token_ParenL {
		p.NextToken()

		// 索引的字段
		pf.mainIndexName = p.Expect(Token_Identifier).Value()

		p.Expect(Token_ParenR)

	}
	// )

	fd.CommentGroup = p.CommentGroupByLine(nameToken.Line())

	// 尝试首次解析
	if need2Pass, _ := pf.resolve(1); need2Pass {
		d.File.FileSet.unknownFields = append(d.File.FileSet.unknownFields, pf)
	}

	checkField(d, fd)

	d.addField(fd)

	return
}

func checkField(d *Descriptor, fd *FieldDescriptor) {

	if _, ok := d.FieldByName[fd.Name]; ok {
		panic(errors.New(fmt.Sprintf("Duplicate field name: %s in %s", fd.Name, d.Name)))
	}

	if _, ok := d.FieldByTag[fd.TagNumber()]; ok {
		panic(errors.New(fmt.Sprintf("Duplicate field tag: %d in %s", fd.TagNumber(), d.Name)))
	}
}
