package meta

import "testing"

func TestCommentParser(t *testing.T) {

    v, err := parseComment("[agent] client -> battle # comment")
    if err != nil{
        t.Error(err)
        t.FailNow()
    }

    t.Log(v)
}
