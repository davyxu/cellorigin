package player

import (
	//"reflect"
	"model"
	"testing"

	//"proto/gamedef"

	//"github.com/golang/protobuf/proto"
)

func TestSer(t *testing.T) {

	p := &Player{
		MemChar: &MemChar{
			DBChar: &DBChar{
				Coin: 101,
			},
		},
	}

	ms := model.NewModelSync(p)

	ms.SetModel("Char.CharName", "hello")

	ms.GetModel("Char.Coin")

	//log.Debugln(p.CharName, m, ok)

	//	var login gamedef.LoginModel

	//	vlogin := reflect.ValueOf(&login)
	//	vlogin.Elem().FieldByName("Account").SetString("hello")

	//	rawdata, err := proto.Marshal(&login)

	//	if err != nil {
	//		log.Errorln(err)
	//	}

	//	log.Debugln(rawdata)

}
