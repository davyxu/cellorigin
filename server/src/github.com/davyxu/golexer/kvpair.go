package golexer

import (
	"bytes"
	"sort"
	"strconv"
)

type KVPair struct {
	values map[string]interface{}
}

func (self *KVPair) Raw() map[string]interface{} {
	return self.values
}

func (self *KVPair) getSortedKey() []string {

	sortedKeys := make([]string, len(self.values))

	var index int
	for k, _ := range self.values {
		sortedKeys[index] = k
		index++
	}

	sort.Strings(sortedKeys)
	return sortedKeys
}

func (self *KVPair) String() string {

	sortedKeys := self.getSortedKey()

	var buf bytes.Buffer
	var index int
	for _, k := range sortedKeys {

		v := self.values[k]

		if index > 0 {
			buf.WriteString(" ")
		}

		buf.WriteString(k)
		buf.WriteString(": ")

		switch vv := v.(type) {
		case string:
			buf.WriteString(vv)
		case []string:
			buf.WriteString("[")
			for sk, sv := range vv {

				if sk > 0 {
					buf.WriteString(",")
				}

				buf.WriteString(sv)
			}

			buf.WriteString("]")
		}

		index++

	}

	return buf.String()

}

func (self *KVPair) GetBool(key string) bool {

	if v, ok := self.values[key]; ok {

		if b, err := strconv.ParseBool(v.(string)); err != nil {
			return false
		} else {
			return b
		}
	}

	return false
}

func (self *KVPair) SetString(key, value string) {
	self.values[key] = value
}

func (self *KVPair) GetString(key string) string {

	if v, ok := self.values[key]; ok {

		return v.(string)
	}

	return ""
}

func (self *KVPair) ContainKey(key string) bool {
	_, ok := self.values[key]

	return ok
}

func (self *KVPair) ContainValue(key string, tgt string) bool {

	if value, ok := self.values[key]; ok {

		switch vv := value.(type) {
		case string:
			return tgt == value
		case []string:
			for _, v := range vv {
				if v == tgt {
					return true
				}
			}
		}
	}

	return false
}

func (self *KVPair) Parse(str string) error {
	return ParseKV(str, func(k string, v interface{}) bool {

		self.values[k] = v

		return true
	})
}

func NewKVPair() *KVPair {

	self := &KVPair{
		values: make(map[string]interface{}),
	}

	return self
}
