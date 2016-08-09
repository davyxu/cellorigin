package gameuser

import (
	"backend"
	"table"

	"github.com/davyxu/cellnet"
	"github.com/davyxu/cellnet/db"
	"gopkg.in/mgo.v2"
	"gopkg.in/mgo.v2/bson"
)

var (
	// 按连接号索引的用户
	UserByID = make(map[int64]*GameUser)

	// 角色名索引的用户
	UserByCharName = make(map[string]*GameUser)

	// 账户名索引的用户
	UserByAccountName = make(map[string]*GameUser)

	// DB原始数据
	RawDataByID = make(map[int64]*DBAccount)

	// 1个连接同时只有1个账户, 1个角色在线
)

func GetAccountData(evq cellnet.EventQueue, accountid string, callback func(error, *DBAccount)) {

	log.Debugln("GetAccountData:", accountid)

	mdb.Execute(func(ses *mgo.Session) {

		var dbdata DBAccount

		col := ses.DB("").C("account")

		err := col.Find(bson.M{"account.accountid": accountid}).One(&dbdata)

		// 没有时创建
		if err == mgo.ErrNotFound {

			log.Debugf("Create account: %s", accountid)

			// 初始账号信息
			dbdata.Account = &AccountData{
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

// 创建角色
func CreateChar(evq cellnet.EventQueue, acc *DBAccount, char *CharData, callback func(error)) {

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

// 注册用户消息, 封装用户回调
func RegisterMessage(msgName string, userHandler func(interface{}, *GameUser)) {

	backend.RegisterMessage(msgName, func(content interface{}, routerSes cellnet.Session, clientid int64) {

		if u, ok := UserByID[clientid]; ok {

			userHandler(content, u)
		}
	})

}

var mdb *db.MongoDriver

func Start() {

	if table.ServiceConfig == nil {
		panic("config not ready")
	}

	mdb = db.NewMongoDriver()

	err := mdb.Start(&db.Config{
		URL:       table.ServiceConfig.DSN,
		ConnCount: table.ServiceConfig.DBConnCount,
	})

	if err != nil {
		log.Errorln(table.ServiceConfig.DSN)
	}

}
