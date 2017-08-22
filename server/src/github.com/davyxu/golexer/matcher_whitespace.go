package golexer

import "reflect"

// 空白字符
type WhiteSpaceMatcher struct {
	baseMatcher
}

func (self *WhiteSpaceMatcher) String() string {
	return reflect.TypeOf(self).Elem().Name()
}

func isWhiteSpace(c rune) bool {
	return c == ' ' || c == '\t'
}

func (self *WhiteSpaceMatcher) Match(tz *Tokenizer) (Token, error) {

	var count int

	var ret rune
	for {

		c := tz.Peek(count)

		if !isWhiteSpace(c) {
			break
		}

		count++
		ret += c
	}

	if count == 0 {
		return EmptyToken, nil
	}

	tz.ConsumeMulti(count)

	return NewToken(self, tz, string(ret), ""), nil
}

func NewWhiteSpaceMatcher(id int) TokenMatcher {
	return &WhiteSpaceMatcher{
		baseMatcher{id},
	}
}
