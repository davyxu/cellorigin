package player

import (
	"github.com/davyxu/cellnet"
	"gopkg.in/mgo.v2"
	"gopkg.in/mgo.v2/bson"
)

// 最多创建的角色数量
const MaxCharCount = 2

// 存db的完整信息
type DBProfile struct {
	Account *DBAccount

	Char []*DBChar // 所有角色的id
}

// 根据角色名找到角色
func (self *DBProfile) GetChar(charname string) *DBChar {
	for _, c := range self.Char {
		if c.CharName == charname {
			return c
		}
	}

	return nil
}

// DB原始数据
var profileByID = make(map[int64]*DBProfile)

// DB原始数据注册
func AddProfile(id int64, profile *DBProfile) {
	profileByID[id] = profile
}

// DB原始数据解除
func RemoveProfile(id int64) {
	delete(profileByID, id)
}

// 从DB加载一份完整的数据
func LoadProfile(evq cellnet.EventQueue, accountid string, callback func(error, *DBProfile)) {

	log.Debugln("Load profile:", accountid)

	mdb.Execute(func(ses *mgo.Session) {

		var dbdata DBProfile

		col := ses.DB("").C("profile")

		err := col.Find(bson.M{"account.accountid": accountid}).One(&dbdata)

		// 没有时创建
		if err == mgo.ErrNotFound {

			log.Debugf("Create profile: %s", accountid)

			// 初始账号信息
			dbdata.Account = &DBAccount{
				AccountID: accountid,
			}

			err = col.Insert(&dbdata)
		}

		if err != nil {
			log.Errorln(err)
		}

		evq.PostData(func() {
			callback(err, &dbdata)
		})

	})
}
