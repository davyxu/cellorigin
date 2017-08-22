package golexer

import (
	"bytes"
	"reflect"
)

// 字符串
type BackTicksMatcher struct {
	baseMatcher
	builder bytes.Buffer
}

func (self *BackTicksMatcher) String() string {
	return reflect.TypeOf(self).Elem().Name()
}

func (self *BackTicksMatcher) Match(tz *Tokenizer) (Token, error) {

	if tz.Current() != '`' {
		return EmptyToken, nil
	}

	tz.ConsumeOne()

	begin := tz.Index()

	for {

		if tz.Current() == '`' {
			tz.ConsumeOne()
			break
		}

		if tz.Current() == 0 {
			break
		}

		tz.ConsumeOne()

	}

	return NewToken(self, tz, tz.StringRange(begin, tz.index-1), ""), nil
}

func NewBackTicksMatcher(id int) TokenMatcher {
	return &BackTicksMatcher{
		baseMatcher: baseMatcher{id},
	}
}
