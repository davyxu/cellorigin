package meta

import "github.com/davyxu/golexer"

// 自定义的token id
const (
	CommentToken_EOF = iota
	CommentToken_LeftBrace
	CommentToken_RightBrace
	CommentToken_WhiteSpace
	CommentToken_LineEnd
	CommentToken_UnixStyleComment
	CommentToken_Identifier
	CommentToken_Unknown
)

type CommentParser struct {
	*golexer.Parser
}

func parseComment(src string) (ret TaggedComment, retErr error) {

	p := NewCommentParser(src)

	//	defer golexer.ErrorCatcher(func(err error) {

	//		fmt.Printf("%s %s\n", p.PreTokenPos().String(), err.Error())

	//		retErr = err

	//	})

	p.Lexer().Start(src)

	p.NextToken()

	for p.TokenID() != CommentToken_EOF {

		//log.Debugln("#", self.TokenID(), self.TokenValue())

		if p.TokenID() == CommentToken_WhiteSpace {
			p.NextToken()
			continue
		}

		// 读取标头
		if p.TokenID() == CommentToken_LeftBrace {

			p.NextToken()

			tagNameToken := p.Expect(CommentToken_Identifier)

			p.Expect(CommentToken_RightBrace)

			ret.Name = tagNameToken.Value()

			for {

				ret.Value += p.TokenValue()

				p.NextToken()

				if p.TokenID() == CommentToken_LineEnd || p.TokenID() == CommentToken_EOF {
					break
				}
			}

		}

		p.NextToken()

	}

	return
}

func NewCommentParser(src string) *CommentParser {

	l := golexer.NewLexer()

	// 匹配顺序从高到低

	l.AddMatcher(golexer.NewSignMatcher(CommentToken_LeftBrace, "["))
	l.AddMatcher(golexer.NewSignMatcher(CommentToken_RightBrace, "]"))

	l.AddMatcher(golexer.NewWhiteSpaceMatcher(CommentToken_WhiteSpace))
	l.AddIgnoreMatcher(golexer.NewLineEndMatcher(CommentToken_LineEnd))
	l.AddIgnoreMatcher(golexer.NewUnixStyleCommentMatcher(CommentToken_UnixStyleComment))

	l.AddMatcher(golexer.NewIdentifierMatcher(CommentToken_Identifier))

	l.AddMatcher(golexer.NewUnknownMatcher(CommentToken_Unknown))

	return &CommentParser{
		Parser: golexer.NewParser(l, src),
	}
}
