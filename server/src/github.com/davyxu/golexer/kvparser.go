package golexer

import (
	"errors"
)

// 自定义的token id
const (
	pbtToken_EOF = iota
	pbtToken_WhiteSpace
	pbtToken_Identifier
	pbtToken_Numeral
	pbtToken_String
	pbtToken_Comma
	pbtToken_Colon
	pbtToken_LBrace
	pbtToken_RBrace
	pbtToken_Unknown
)

// k:v 与pbt格式互换,详见kvparser_test.go

func ParseKV(str string, callback func(string, interface{}) bool) (errRet error) {

	l := NewLexer()

	l.AddMatcher(NewNumeralMatcher(pbtToken_Numeral))
	l.AddMatcher(NewStringMatcher(pbtToken_String))

	l.AddIgnoreMatcher(NewWhiteSpaceMatcher(pbtToken_WhiteSpace))
	l.AddMatcher(NewSignMatcher(pbtToken_Comma, ","))
	l.AddMatcher(NewSignMatcher(pbtToken_Colon, ":"))
	l.AddMatcher(NewSignMatcher(pbtToken_LBrace, "["))
	l.AddMatcher(NewSignMatcher(pbtToken_RBrace, "]"))
	l.AddMatcher(NewIdentifierMatcher(pbtToken_Identifier))
	l.AddMatcher(NewUnknownMatcher(pbtToken_Unknown))

	l.Start(str)

	p := NewParser(l, str)

	defer ErrorCatcher(func(err error) {

		errRet = err

	})

	p.NextToken()

	for p.TokenID() != pbtToken_EOF {

		if p.TokenID() != pbtToken_Identifier {
			panic(errors.New("expect identifier: " + p.TokenValue()))
		}

		key := p.TokenValue()

		p.NextToken()

		if p.TokenID() != pbtToken_Colon {
			panic(errors.New("expect comma: " + p.TokenValue()))
		}

		p.NextToken()

		var value interface{}

		if p.TokenID() == pbtToken_LBrace {
			p.NextToken()

			strArray := make([]string, 0)

			for p.TokenID() != pbtToken_RBrace &&
				p.TokenID() != pbtToken_EOF {

				value := p.TokenValue()

				strArray = append(strArray, value)

				p.NextToken()

				// 逗号分割值
				if p.TokenID() == pbtToken_Comma {
					p.NextToken()
				}

			}

			value = strArray

		} else {
			value = p.TokenValue()
		}

		if !callback(key, value) {
			return nil
		}

		p.NextToken()

	}

	return nil
}
