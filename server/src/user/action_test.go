package user

import (
	"reflect"
	"testing"
)

func TestAction(t *testing.T) {

	var evtTest Action

	var step int

	evtTest.Add(func(args ...interface{}) {
		t.Log("test1", args[0], args[1], reflect.TypeOf(args[2]))
		step++
	})

	t.Log(evtTest)

	evtTest.Add(func(args ...interface{}) {
		t.Log("test2", args[0], args[1], reflect.TypeOf(args[2]))
		step++
	})

	evtTest.Invoke(1, 2, "hello")

	evtTest.Clear()

	evtTest.Invoke(3, 4, "hello")

	if step != 2 {
		t.Fail()
	}

}
