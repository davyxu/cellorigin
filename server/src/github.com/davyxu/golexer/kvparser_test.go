package golexer

import (
	"testing"
)

func TestKVParser(t *testing.T) {

	ParseKV(`TableName: "Service"  Package: "gamedef" OutputTag:[".go", ".json"]"`, func(key string, value interface{}) bool {

		switch v := value.(type) {
		case string:
			t.Logf("'%s' = '%s'", key, value)
		case []string:
			t.Logf("'%s' = '%s' len: %d", key, value, len(v))
		}

		return true
	})

	kvp := NewKVPair()
	if err := kvp.Parse(`protobuf:"varint,1,opt,name=AutoID,json=autoID" json:"AutoID,omitempty" my:[1, 2]`); err != nil {
		t.Error(err)
	}

	t.Log(kvp.GetString("json"))

	t.Log(kvp.String())

}
