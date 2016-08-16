package player

import (
	"table"

	"github.com/davyxu/cellnet/db"
)

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
