package meta

import (
	"fmt"
	"testing"
)

func TestParser(t *testing.T) {

	fileD, err := ParseFile("../example/addressbook.sp")

	if err != nil {
		t.Log(err)
		t.FailNow()
	}

	v, _ := fileD.StructByName["PhoneNumber"]
	//f, _ := v.FieldByName["number"]

	tag, _ := v.MatchTag("agent")
	fmt.Println("tag: ", tag)

	fmt.Println(fileD.String())
}
