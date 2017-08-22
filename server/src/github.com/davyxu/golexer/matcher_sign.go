package golexer

import (
	"fmt"
	"reflect"
	"unicode"
)

// 操作符，分隔符
type SignMatcher struct {
	baseMatcher
	word []rune
}

func isSign(r rune) bool {
	return !unicode.IsLetter(r) &&
		!unicode.IsDigit(r) &&
		r != ' ' &&
		r != '\r' &&
		r != '\n'
}
func (self *SignMatcher) String() string {
	return fmt.Sprintf("%s('%s')", reflect.TypeOf(self).Elem().Name(), string(self.word))
}

func (self *SignMatcher) Match(tz *Tokenizer) (Token, error) {

	if (tz.Count() - tz.Index()) < len(self.word) {
		return EmptyToken, nil
	}

	for i, c := range self.word {

		if !isSign(c) {
			return EmptyToken, nil
		}

		if tz.Peek(i) != c {
			return EmptyToken, nil
		}

	}

	tz.ConsumeMulti(len(self.word))

	return NewToken(self, tz, string(self.word), ""), nil
}

func NewSignMatcher(id int, word string) TokenMatcher {
	self := &SignMatcher{
		baseMatcher: baseMatcher{id},
		word:        []rune(word),
	}

	for _, c := range self.word {
		if !isSign(c) {
			panic("not sign")
		}
	}

	return self
}
