package golexer

import "reflect"

// #开头的行注释
type UnixStyleCommentMatcher struct {
	baseMatcher
}

func (self *UnixStyleCommentMatcher) String() string {
	return reflect.TypeOf(self).Elem().Name()
}

func (self *UnixStyleCommentMatcher) Match(tz *Tokenizer) (Token, error) {
	if tz.Current() != '#' {
		return EmptyToken, nil
	}

	tz.ConsumeOne()

	begin := tz.Index()

	for {

		tz.ConsumeOne()

		if tz.Current() == '\n' || tz.Current() == 0 {
			break
		}

	}

	return NewToken(self, tz, tz.StringRange(begin, tz.index), ""), nil
}

func NewUnixStyleCommentMatcher(id int) TokenMatcher {
	return &UnixStyleCommentMatcher{
		baseMatcher{id},
	}
}
