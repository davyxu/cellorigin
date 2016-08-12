package charinit

import (
	"gamesvc/player"
	"proto/gamedef"
	"timeutil"

	"github.com/davyxu/cellnet"
)

func Start(evq cellnet.EventQueue) {

	player.RegisterMessageWithProfile("gamedef.CharListREQ", func(content interface{}, u *player.Player, profile *player.DBProfile) {

		ack := gamedef.ModelACK{}

		for _, c := range profile.Char {

			ack.CharList = append(ack.CharList, &gamedef.SimpleCharInfo{
				CharName:     c.CharName,
				LastLoginUTC: c.LastLoginUTC,

				// TODO ID
			})
		}

		u.Send(&ack)

	})

	player.RegisterMessageWithProfile("gamedef.CreateCharREQ", func(content interface{}, u *player.Player, profile *player.DBProfile) {
		msg := content.(*gamedef.CreateCharREQ)

		// TODO 名字检查
		// TODO 角色范围检查

		c := &player.DBChar{
			CharName:     msg.CharName,
			LastLoginUTC: timeutil.GetUTCSecondNow(),
		}

		player.CreateChar(evq, profile, c, func(err error) {

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

	})

	// 进入验证
	player.RegisterMessageWithProfile("gamedef.EnterGameREQ", func(content interface{}, u *player.Player, profile *player.DBProfile) {
		msg := content.(*gamedef.EnterGameREQ)

		// TODO 处理GM进入
		// TODO 处理人满

		if c := profile.GetChar(msg.CharName); c != nil {

			// 选中这个角色, 挂接到逻辑对象

			u.InitChar(c)

			u.Send(&gamedef.EnterGameACK{
				Result: gamedef.EnterGameResult_EnterGameOK,
			})

		} else {
			log.Debugf("select char not exist, account: %s char: %s", u.ID(), msg.CharName)
		}

	})

}
