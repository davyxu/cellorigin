package golexer

import "errors"

type Parser struct {
	lexer *Lexer

	curr Token

	errFunc func(error)

	prePos TokenPos
}

func (self *Parser) Lexer() *Lexer {
	return self.lexer
}

func (self *Parser) TokenPos() TokenPos {
	return self.lexer.pos
}
func (self *Parser) PreTokenPos() TokenPos {
	return self.prePos
}

func (self *Parser) Expect(id int) Token {

	if self.TokenID() != id {
		panic(errors.New("Expect " + self.lexer.MatcherString(id)))
	}

	t := self.curr

	self.NextToken()

	return t
}

func (self *Parser) NextToken() {

	self.prePos = self.lexer.pos

	token, err := self.lexer.Read()

	if err != nil {
		panic(err)
	}

	self.curr = token
}

func (self *Parser) RawToken() Token {
	return self.curr
}

func (self *Parser) TokenID() int {
	return self.curr.MatcherID()
}

func (self *Parser) TokenValue() string {
	return self.curr.Value()
}

func (self *Parser) MatcherName() string {
	return self.curr.MatcherName()
}

func (self *Parser) MatcherString() string {
	return self.curr.MatcherString()
}

func (self *Parser) TokenRaw() string {

	return self.curr.Raw()
}

func NewParser(l *Lexer, srcName string) *Parser {

	self := &Parser{
		lexer: l,
	}

	self.lexer.pos.SourceName = srcName

	return self

}
