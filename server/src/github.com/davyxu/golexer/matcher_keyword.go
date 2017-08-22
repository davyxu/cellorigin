package golexer

import (
	"fmt"
	"reflect"
	"unicode"
)

// 下划线和字母(中文)+数字=关键字
type KeywordMatcher struct {
	baseMatcher
	word []rune
}

func isKeyword(r rune, index int) bool {
	basic := unicode.IsLetter(r) || r == '_'
	if index == 0 {
		return basic
	}

	return basic || unicode.IsDigit(r)
}

func (self *KeywordMatcher) String() string {
	return fmt.Sprintf("%s('%s')", reflect.TypeOf(self).Elem().Name(), string(self.word))
}

func (self *KeywordMatcher) Match(tz *Tokenizer) (Token, error) {

	if (tz.Count() - tz.Index()) < len(self.word) {
		return EmptyToken, nil
	}

	var index int

	for _, c := range self.word {

		if !isKeyword(c, index) {
			return EmptyToken, nil
		}

		if tz.Peek(index) != c {
			return EmptyToken, nil
		}

		index++

	}

	tz.ConsumeMulti(len(self.word))

	return NewToken(self, tz, string(self.word), ""), nil
}

func NewKeywordMatcher(id int, word string) TokenMatcher {

	if len(word) == 0 {
		panic("empty string")
	}

	self := &KeywordMatcher{
		baseMatcher: baseMatcher{id},
		word:        []rune(word),
	}

	for i, c := range self.word {
		if !isKeyword(c, i) {
			panic("not keyword")
		}
	}

	return self
}
