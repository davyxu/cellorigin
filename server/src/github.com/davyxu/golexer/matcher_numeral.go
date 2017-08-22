package golexer

import (
	"errors"
	"reflect"
	"unicode"
)

// 整形，浮点数
type NumeralMatcher struct {
	baseMatcher
	includeNagtive bool
}

func (self *NumeralMatcher) String() string {
	return reflect.TypeOf(self).Elem().Name()
}

func (self *NumeralMatcher) Match(tz *Tokenizer) (Token, error) {

	if !(unicode.IsDigit(tz.Current()) || (self.includeNagtive && tz.Current() == '-')) {

		return EmptyToken, nil
	}

	begin := tz.Index()

	var maybeFloat bool

	for {

		tz.ConsumeOne()

		if !unicode.IsDigit(tz.Current()) {

			if tz.Current() == '.' {
				maybeFloat = true
			}

			break
		}

	}

	if maybeFloat {
		for i := 0; ; i++ {

			tz.ConsumeOne()

			if !unicode.IsDigit(tz.Current()) {

				// .之后的第一个字符居然不是数字
				if i == 0 {
					return EmptyToken, errors.New("invalid numeral")
				}

				break

			}

		}
	}

	return NewToken(self, tz, tz.StringRange(begin, tz.Index()), ""), nil
}

func NewNumeralMatcher(id int) TokenMatcher {
	return &NumeralMatcher{
		baseMatcher:    baseMatcher{id},
		includeNagtive: true,
	}
}

func NewPositiveNumeralMatcher(id int) TokenMatcher {
	return &NumeralMatcher{
		baseMatcher:    baseMatcher{id},
		includeNagtive: false,
	}
}
