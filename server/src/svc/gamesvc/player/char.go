package player

import (
	"github.com/davyxu/cellnet"
	"gopkg.in/mgo.v2"
	"gopkg.in/mgo.v2/bson"
)

// 存入DB的角色信息
type DBChar struct {
	// Model需要同步的
	CharName string `model:"Char.CharName"`
	Coin     int32  `model:"Char.Coin"`
	Diamond  int32  `model:"Char.Diamond"`

	// 不需要同步的变量,但需要存盘

	LastLoginUTC int64 // 上次登录的时间戳
}

// 内存里的角色信息
type MemChar struct {
	*DBChar

	// 不需要存盘的变量
}

func (self *MemChar) ChangeValue(key string, value interface{}) {

}

// 创建角色
func CreateChar(evq cellnet.EventQueue, acc *DBProfile, char *DBChar, callback func(error)) {

	acc.Char = append(acc.Char, char)

	mdb.Execute(func(ses *mgo.Session) {

		col := ses.DB("").C("account")

		err := col.Update(bson.M{"account.accountid": acc.Account.AccountID}, &acc)
		if err != nil {
			log.Errorln(err)
		}

		evq.PostData(func() {
			callback(err)
		})

	})

}
