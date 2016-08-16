package verify

import (
	"backend"
	"proto/gamedef"
	"svc/gamesvc/player"

	"github.com/davyxu/cellnet"
)

func Start(evq cellnet.EventQueue) {

	backend.RegisterMessage("gamedef.VerifyGameREQ", func(content interface{}, routerSes cellnet.Session, clientid int64) {
		msg := content.(*gamedef.VerifyGameREQ)

		// 已经有用户了
		if player.UserExists(clientid) {

			// TODO 踢掉之前的用户, 让这个用户进来
			return
		}

		// 创建新玩家
		u := player.AddPlayer(routerSes, clientid)

		// 加载玩家数据
		player.LoadProfile(evq, msg.Token, func(err error, profile *player.DBProfile) {

			var ack gamedef.VerifyGameACK

			if err == nil {

				// 保存原始数据, 方便后面处理
				player.AddProfile(clientid, profile)

				u.InitAccount(profile.Account)

				// 角色数据在选择角色后挂接
				ack.Result = gamedef.VerifyGameResult_VerifyOK

			} else {

				ack.Result = gamedef.VerifyGameResult_DataException
			}

			u.Send(&ack)

		})
	})

	// 客户端断开
	backend.RegisterMessage("gamedef.SessionClosed", func(content interface{}, routerSes cellnet.Session, clientid int64) {

		// 这个组件关联的, 这个组件删除
		player.RemoveProfile(clientid)
		player.RemovePlayer(clientid)
	})

}
