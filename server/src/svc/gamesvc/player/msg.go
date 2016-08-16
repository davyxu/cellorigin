package player

import (
	"backend"

	"github.com/davyxu/cellnet"
)

// 注册用户消息, 封装用户回调
func RegisterMessage(msgName string, userHandler func(interface{}, *Player)) {

	backend.RegisterMessage(msgName, func(content interface{}, routerSes cellnet.Session, clientid int64) {

		if u, ok := playerByID[clientid]; ok {

			userHandler(content, u)
		}
	})

}

func RegisterMessageWithProfile(msgName string, userHandler func(interface{}, *Player, *DBProfile)) {

	backend.RegisterMessage(msgName, func(content interface{}, routerSes cellnet.Session, clientid int64) {

		u, ok := playerByID[clientid]
		if !ok {
			return
		}

		profile, ok := profileByID[clientid]
		if !ok {
			return
		}

		userHandler(content, u, profile)
	})

}
