# golexer

可自定义的词法解析器

C#版参考https://github.com/davyxu/SharpLexer

# 特性

* 支持数值，字符串，注释，标识符等的内建匹配

* 可自定义匹配器来拾取需要的token

* 高性能并发匹配

```golang

// 自定义的token id
const (
	Token_Unknown = iota
	Token_Numeral
	Token_String
	Token_WhiteSpace
	Token_LineEnd
	Token_UnixStyleComment
	Token_Identifier
	Token_Go
	Token_Semicolon
)

// 使用lexer

	l := NewLexer()
	l.AddMatcher(NewNumeralMatcher(Token_Numeral))
	l.AddMatcher(NewStringMatcher(Token_String))

	l.AddIgnoreMatcher(NewWhiteSpaceMatcher(Token_WhiteSpace))
	l.AddIgnoreMatcher(NewLineEndMatcher(Token_LineEnd))
	l.AddIgnoreMatcher(NewUnixStyleCommentMatcher(Token_UnixStyleComment))

	l.AddMatcher(NewSignMatcher(Token_Semicolon, ";"))
	l.AddMatcher(NewSignMatcher(Token_Go, "go"))

	l.AddMatcher(NewIdentifierMatcher(Token_Identifier))

	l.AddMatcher(NewUnknownMatcher(Token_Unknown))

// 解析原文
	l.Start(`"a" 
	123.3;
	go
	_id # comment
	;
	'b'
	
	
	`)

	// 解析过程
	for {

		tk, err := l.Read()

		if err != nil {
			t.Error(err)
			break
		}

		if tk == nil {
			break
		}

		t.Log(tk.String())
	}
```


# 备注

感觉不错请star, 谢谢!

博客: http://www.cppblog.com/sunicdavy

知乎: http://www.zhihu.com/people/sunicdavy

邮箱: sunicdavy@qq.com
