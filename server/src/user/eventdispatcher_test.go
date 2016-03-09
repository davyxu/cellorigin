package user

import (
	"reflect"
	"testing"
)

func TestEventDispatcher(t *testing.T) {
	ev := NewEventDispatcher()

	var step int

	ev.Add("test", func(args ...interface{}) {
		t.Log("test1", args)
		step++
	})

	ev.Add("test", func(args ...interface{}) {
		t.Log("test2", args[0], args[1], reflect.TypeOf(args[2]))
		step++
	})

	ev.Invoke("test", 1, 2, "hello")

	if step != 2 {
		t.Fail()
	}

}
