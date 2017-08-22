package meta

import "errors"

func parseEnumField(p *sprotoParser, d *Descriptor) {

	fd := newFieldDescriptor(d)

	nameToken := p.RawToken()

	// 字段名
	fd.Name = p.Expect(Token_Identifier).Value()

	if _, ok := d.FieldByName[fd.Name]; ok {
		panic(errors.New("Duplicate field name: " + d.Name))
	}

	// 有等号
	if p.TokenID() == Token_Assign {
		p.NextToken()

		// tag
		fd.Tag = p.Expect(Token_Numeral).ToInt()

	} else {

		if len(d.Fields) == 0 {
			fd.AutoTag = 0
		} else {
			fd.AutoTag = d.MaxTag() + 1
		}

	}

	fd.Type = FieldType_Int32

	fd.CommentGroup = p.CommentGroupByLine(nameToken.Line())

	checkField(d, fd)

	d.addField(fd)

	return
}
