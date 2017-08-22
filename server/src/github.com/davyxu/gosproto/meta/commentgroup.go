package meta

import (
	"fmt"

	"github.com/davyxu/golexer"
)

type TaggedComment struct {
	Name  string
	Value string
}

type CommentGroup struct {
	Pos golexer.TokenPos

	Leading  string
	Trailing string

	taggedComments []TaggedComment
}

func (self *CommentGroup) addLineComment(text string) {

	if text == "" {
		return
	}

	if v, err := parseComment(text); err == nil && v.Name != "" {
		self.taggedComments = append(self.taggedComments, v)
	}
}

func (self *CommentGroup) MatchTag(tag string) (string, bool) {

	for _, c := range self.taggedComments {
		if c.Name == tag {
			return c.Value, true
		}
	}

	return "", false

}

func (self *CommentGroup) String() string {
	return fmt.Sprintf("Leading: %s Trailing: %s", self.Leading, self.Trailing)
}

func newCommentGroup() *CommentGroup {
	return &CommentGroup{}
}
