package charinit

import (
	"gamesvc/gameuser"
	"proto/gamedef"
	"timeutil"

	"github.com/davyxu/cellnet"
)

func Start(evq cellnet.EventQueue) {

	gameuser.RegisterMessage("gamedef.CharListREQ", func(content interface{}, u *gameuser.GameUser) {
		//msg := content.(*gamedef.CharListREQ)

		if acc, ok := gameuser.RawDataByID[u.ID()]; ok {

			ack := gamedef.CharListACK{}

			for _, c := range acc.Char {

				ack.CharInfo = append(ack.CharInfo, &gamedef.SimpleCharInfo{
					CharName:     c.CharName,
					LastLoginUTC: c.LastLoginUTC,

					// TODO ID
				})
			}

			u.Send(&ack)

		} else {
			log.Errorln("out of raw account data, id: ", u.ID())
		}

	})

	gameuser.RegisterMessage("gamedef.CreateCharREQ", func(content interface{}, u *gameuser.GameUser) {
		msg := content.(*gamedef.CreateCharREQ)

		if acc, ok := gameuser.RawDataByID[u.ID()]; ok {

			// TODO 名字检查
			// TODO 角色范围检查

			c := &gameuser.CharData{
				CharName:     msg.CharName,
				LastLoginUTC: timeutil.GetUTCSecondNow(),
			}

			gameuser.CreateChar(evq, acc, c, func(err error) {

				if err == nil {

					u.Send(&gamedef.CreateCharACK{
						CandidateID: msg.CandidateID,
						Result:      gamedef.CreateCharResult_CreateCharOK,
					})

				} else {
					u.Send(&gamedef.CreateCharACK{
						Result: gamedef.CreateCharResult_CreateCharError,
					})
				}

			})

		}

	})

	// 进入验证
	gameuser.RegisterMessage("gamedef.EnterGameREQ", func(content interface{}, u *gameuser.GameUser) {
		msg := content.(*gamedef.EnterGameREQ)

		// TODO 处理GM进入
		// TODO 处理人满

		if acc, ok := gameuser.RawDataByID[u.ID()]; ok {

			if c := acc.GetChar(msg.CharName); c != nil {

				// 选中这个角色, 挂接到逻辑对象
				u.CharData = c

				u.Send(&gamedef.EnterGameACK{
					Result: gamedef.EnterGameResult_EnterGameOK,
				})

			} else {
				log.Debugf("select char not exist, account: %s char: %s", u.ID(), msg.CharName)
			}

		}

	})

}
