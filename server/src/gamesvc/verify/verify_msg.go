package verify

import (
	"gamesvc/gameuser"
	"proto/gamedef"
	"user"

	"github.com/davyxu/cellnet"
	"github.com/davyxu/cellnet/router"
)

func Start(evq cellnet.EventQueue) {

	router.RegisterMessage("gamedef.VerifyGameREQ", func(content interface{}, routerSes cellnet.Session, clientid int64) {
		msg := content.(*gamedef.VerifyGameREQ)

		// 已经有用户了
		if _, ok := gameuser.UserByID[clientid]; ok {

			// TODO 踢掉之前的用户, 让这个用户进来
			return
		}

		// 创建新用户
		u := &gameuser.GameUser{RouterUser: user.NewRouterUser(routerSes, clientid)}
		gameuser.UserByID[clientid] = u

		// 加载玩家数据
		gameuser.LoadAccountData(evq, msg.Token, func(acc *gameuser.DBAccount) {

			// 没数据
			if acc.Account == nil {
				u.Send(&gamedef.VerifyGameACK{
					Result: gamedef.VerifyGameResult_InvalidToken,
				})
				return
			}

			u.AccountData = acc.Account

			// 保存原始数据, 方便后面处理
			gameuser.RawDataByID[clientid] = acc

			// 角色数据在选择角色后挂接

			u.Send(&gamedef.VerifyGameACK{
				Result: gamedef.VerifyGameResult_VerifyOK,
			})
		})
	})

	// 客户端断开
	router.RegisterMessage("coredef.SessionClosed", func(content interface{}, routerSes cellnet.Session, clientid int64) {

		// 这个组件关联的, 这个组件删除
		delete(gameuser.RawDataByID, clientid)
		delete(gameuser.UserByID, clientid)
	})

}
