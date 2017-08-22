package platformverify

import (
	"proto"

	"github.com/davyxu/cellnet"
	"github.com/davyxu/golog"
)

var log *golog.Logger = golog.New("platformverify")

func Start(peer cellnet.Peer) {

	cellnet.RegisterMessage(peer, "proto.LoginREQ", func(ev *cellnet.Event) {

		msg := ev.Msg.(*proto.LoginREQ)

		log.Debugln("platform token:", msg.PlatformToken)

		ev.Send(&proto.LoginACK{
			Token: "pass",
		})

	})

}
