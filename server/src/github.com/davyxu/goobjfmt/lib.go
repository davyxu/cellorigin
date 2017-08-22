package goobjfmt

import (
	"fmt"
	"reflect"
	"sort"
)

type mapKeySorter struct {
	vs   []reflect.Value
	less func(a, b reflect.Value) bool
}

func (s mapKeySorter) Len() int      { return len(s.vs) }
func (s mapKeySorter) Swap(i, j int) { s.vs[i], s.vs[j] = s.vs[j], s.vs[i] }
func (s mapKeySorter) Less(i, j int) bool {
	return s.less(s.vs[i], s.vs[j])
}

// Map fields may have key types of non-float scalars, strings and enums.
// The easiest way to sort them in some deterministic order is to use fmt.
// If this turns out to be inefficient we can always consider other options,
// such as doing a Schwartzian transform.

func mapKeys(vs []reflect.Value) sort.Interface {
	s := mapKeySorter{
		vs: vs,
		// default Less function: textual comparison
		less: func(a, b reflect.Value) bool {
			return fmt.Sprint(a.Interface()) < fmt.Sprint(b.Interface())
		},
	}

	// Type specialization per https://developers.google.com/protocol-buffers/docs/proto#maps;
	// numeric keys are sorted numerically.
	if len(vs) == 0 {
		return s
	}
	switch vs[0].Kind() {
	case reflect.Int32, reflect.Int64:
		s.less = func(a, b reflect.Value) bool { return a.Int() < b.Int() }
	case reflect.Uint32, reflect.Uint64:
		s.less = func(a, b reflect.Value) bool { return a.Uint() < b.Uint() }
	}

	return s
}
