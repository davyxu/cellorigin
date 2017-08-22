package golexer

import "reflect"

// 未知字符
type UnknownMatcher struct {
	baseMatcher
}

func (self *UnknownMatcher) String() string {
	return reflect.TypeOf(self).Elem().Name()
}

func (self *UnknownMatcher) Match(tz *Tokenizer) (Token, error) {

	if tz.Current() == 0 {
		return EmptyToken, nil
	}

	begin := tz.Index()

	tz.ConsumeOne()

	return NewToken(self, tz, tz.StringRange(begin, tz.Index()), ""), nil
}

func NewUnknownMatcher(id int) TokenMatcher {
	return &UnknownMatcher{
		baseMatcher{id},
	}
}
