package golexer

type Tokenizer struct {
	src   []rune
	index int
	lex   *Lexer
}

func (self *Tokenizer) Current() rune {

	if self.EOF() {
		return 0
	}

	return self.src[self.index]
}

func (self *Tokenizer) Index() int {
	return self.index
}

func (self *Tokenizer) Count() int {
	return len(self.src)
}

func (self *Tokenizer) Line() int {
	return self.lex.pos.Line
}

func (self *Tokenizer) Peek(offset int) rune {

	if self.index+offset >= len(self.src) {
		return 0
	}

	return self.src[self.index+offset]
}

func (self *Tokenizer) ConsumeOne() {

	self.index++
	self.lex.pos.Col++
}

func (self *Tokenizer) ConsumeMulti(count int) {

	self.index += count
	self.lex.pos.Col += count
}

func (self *Tokenizer) EOF() bool {
	return self.index >= len(self.src)
}

func (self *Tokenizer) increaseLine() {
	self.lex.pos.Line++
	self.lex.pos.Col = 1
}

func (self *Tokenizer) StringRange(begin, end int) string {

	if begin < 0 {
		begin = 0
	}

	if end > len(self.src) {
		end = len(self.src)
	}

	return string(self.src[begin:end])
}

func NewTokenizer(s string, l *Lexer) *Tokenizer {

	return &Tokenizer{
		src: []rune(s),
		lex: l,
	}
}
