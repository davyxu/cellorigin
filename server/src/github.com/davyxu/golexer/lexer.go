package golexer

type Lexer struct {
	matchers []matcherMeta

	running bool

	tz *Tokenizer

	pos TokenPos
}

type matcherMeta struct {
	m      TokenMatcher
	ignore bool
}

var eofToken = NewToken(nil, nil, "EOF", "")

func (self *Lexer) VisitMatcher(callback func(TokenMatcher) bool) {

	for _, m := range self.matchers {
		if !callback(m.m) {
			return
		}
	}
}

func (self *Lexer) MatcherString(id int) string {
	m := self.MatcherByID(id)
	if m != nil {
		return m.String()
	}

	return ""
}

func (self *Lexer) MatcherByID(id int) TokenMatcher {
	for _, m := range self.matchers {
		if m.m.ID() == id {
			return m.m
		}
	}

	return nil
}

// 添加一个匹配器，如果结果匹配，返回token
func (self *Lexer) AddMatcher(m TokenMatcher) {
	self.matchers = append(self.matchers, matcherMeta{
		m:      m,
		ignore: false,
	})
}

// 添加一个匹配器，如果结果匹配，直接忽略匹配内容
func (self *Lexer) AddIgnoreMatcher(m TokenMatcher) {
	self.matchers = append(self.matchers, matcherMeta{
		m:      m,
		ignore: true,
	})
}

func (self *Lexer) Start(src string) {

	self.running = true

	self.tz = NewTokenizer(src, self)
}

func (self *Lexer) Read() (Token, error) {

	if !self.running {
		return eofToken, nil
	}

	tk, err := self.readToken()

	if err != nil || tk.MatcherID() == 0 {
		self.running = false
	}

	return tk, err
}

func (self *Lexer) readToken() (Token, error) {

	if len(self.matchers) == 0 {
		return eofToken, nil
	}

	for !self.tz.EOF() {

		for _, mm := range self.matchers {

			token, err := mm.m.Match(self.tz)

			if err != nil {
				return NewToken(nil, self.tz, err.Error(), ""), err
			}

			if token == EmptyToken {
				continue
			}

			if mm.ignore {
				break
			}

			return token, nil
		}
	}

	return eofToken, nil
}

func NewLexer() *Lexer {

	return &Lexer{
		pos: DefaultTokenPos,
	}

}
