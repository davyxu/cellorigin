package usermgr

import (
	"proto/gamedef"
	"user"

	"github.com/davyxu/cellnet"
	"github.com/davyxu/cellnet/router"
)

var Manager = user.NewManager()

func Start() {

	// 进入验证
	router.RegisterSessionMessage("gamedef.EnterGameREQ", func(content interface{}, routerSes cellnet.Session, clientid int64) {
		//msg := content.(*gamedef.EnterGameREQ)

		// 已经有用户了
		if Manager.GetByID(clientid) != nil {
			return
		}

		// TODO 处理GM进入
		// TODO 处理人满
		// TODO 验证token

		u := user.NewUser(routerSes, clientid)
		Manager.Add(u)

		// TODO 加载DB档案

		// 通知User数据已经准备好了
		u.Event.Invoke("OnDataReady")

		u.Send(&gamedef.EnterGameACK{
			Result: gamedef.EnterGameResult_OK,
		})

	})

	router.RegisterSessionMessage("coredef.SessionClosed", func(content interface{}, routerSes cellnet.Session, clientid int64) {
		Manager.Remove(clientid)
	})

}
