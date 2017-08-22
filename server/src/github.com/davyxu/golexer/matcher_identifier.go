package golexer

import (
	"reflect"
	"unicode"
)

// 标识符
type IdentifierMatcher struct {
	baseMatcher
}

func (self *IdentifierMatcher) String() string {
	return reflect.TypeOf(self).Elem().Name()
}

func (self *IdentifierMatcher) Match(tz *Tokenizer) (Token, error) {

	if !unicode.IsLetter(tz.Current()) && tz.Current() != '_' {
		return EmptyToken, nil
	}

	begin := tz.Index()

	for {

		tz.ConsumeOne()

		if !(unicode.IsLetter(tz.Current()) || unicode.IsDigit(tz.Current())) && tz.Current() != '_' {
			break
		}

	}

	return NewToken(self, tz, tz.StringRange(begin, tz.index), ""), nil
}

func NewIdentifierMatcher(id int) TokenMatcher {
	return &IdentifierMatcher{
		baseMatcher{id},
	}
}
