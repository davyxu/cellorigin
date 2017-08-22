package golexer

import (
	"bytes"
	"fmt"
	"testing"
)

// 自定义的token id
const (
	Token_EOF = iota
	Token_Unknown
	Token_Numeral
	Token_String
	Token_WhiteSpace
	Token_LineEnd
	Token_UnixStyleComment
	Token_Identifier
	Token_Dot
	Token_Go
	Token_XX
	Token_Every
	Token_Week
	Token_Semicolon
	Token_BackTicker
)

type CustomParser struct {
	*Parser
}

func NewCustomParser() *CustomParser {

	l := NewLexer()

	// 匹配顺序从高到低

	l.AddMatcher(NewNumeralMatcher(Token_Numeral))
	l.AddMatcher(NewStringMatcher(Token_String))

	l.AddIgnoreMatcher(NewWhiteSpaceMatcher(Token_WhiteSpace))
	l.AddIgnoreMatcher(NewLineEndMatcher(Token_LineEnd))
	l.AddIgnoreMatcher(NewUnixStyleCommentMatcher(Token_UnixStyleComment))

	l.AddMatcher(NewSignMatcher(Token_Semicolon, ";"))
	l.AddMatcher(NewSignMatcher(Token_Dot, "."))
	l.AddMatcher(NewKeywordMatcher(Token_Go, "go"))
	l.AddMatcher(NewKeywordMatcher(Token_XX, "xx"))
	l.AddMatcher(NewKeywordMatcher(Token_Every, "等"))
	l.AddMatcher(NewKeywordMatcher(Token_Week, "秒"))
	l.AddMatcher(NewBackTicksMatcher(Token_BackTicker))

	l.AddMatcher(NewIdentifierMatcher(Token_Identifier))

	l.AddMatcher(NewUnknownMatcher(Token_Unknown))

	return &CustomParser{
		Parser: NewParser(l, "custom"),
	}
}

// 反引号内容
func TestBackTicks(t *testing.T) {

	p := NewCustomParser()

	defer ErrorCatcher(func(err error) {

		t.Error(err.Error())

	})

	p.Lexer().Start("go . `xxx` ")

	p.NextToken()

	for p.TokenID() != 0 {

		t.Log(fmt.Sprintf("MatcherName: '%s' Value: '%s'\n", p.MatcherName(), p.TokenValue()))

		p.NextToken()

	}

}

func TestParser(t *testing.T) {

	p := NewCustomParser()

	defer ErrorCatcher(func(err error) {

		t.Error(err.Error())

	})

	p.Lexer().Start(`"a"
		123.3;
		-1
		Base64Text
		gonew.xx
		_id # comment
		等2秒
		"\'\""
		""
		;
		'b'

		`)

	p.NextToken()

	rightAnswer := `===
MatcherName: 'StringMatcher' Value: 'a'
MatcherName: 'NumeralMatcher' Value: '123.3'
MatcherName: 'SignMatcher' Value: ';'
MatcherName: 'NumeralMatcher' Value: '-1'
MatcherName: 'IdentifierMatcher' Value: 'Base64Text'
MatcherName: 'KeywordMatcher' Value: 'go'
MatcherName: 'IdentifierMatcher' Value: 'new'
MatcherName: 'SignMatcher' Value: '.'
MatcherName: 'KeywordMatcher' Value: 'xx'
MatcherName: 'IdentifierMatcher' Value: '_id'
MatcherName: 'KeywordMatcher' Value: '等'
MatcherName: 'NumeralMatcher' Value: '2'
MatcherName: 'KeywordMatcher' Value: '秒'
MatcherName: 'StringMatcher' Value: ''"'
MatcherName: 'StringMatcher' Value: ''
MatcherName: 'SignMatcher' Value: ';'
MatcherName: 'StringMatcher' Value: 'b'
===
`

	var b bytes.Buffer

	b.WriteString("===\n")

	for p.TokenID() != 0 {

		b.WriteString(fmt.Sprintf("MatcherName: '%s' Value: '%s'\n", p.MatcherName(), p.TokenValue()))

		p.NextToken()

	}

	b.WriteString("===\n")

	fmt.Println(b.String())

	if b.String() != rightAnswer {
		t.FailNow()
	}

}
