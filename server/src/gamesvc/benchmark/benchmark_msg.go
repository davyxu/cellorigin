package benchmark

import (
	"proto/gamedef"

	"time"

	"github.com/davyxu/cellnet"
	"github.com/davyxu/cellnet/gate"
	"github.com/davyxu/golog"
)

var log *golog.Logger = golog.New("main")

func Start(pipe cellnet.EventPipe) {

	timeEvq := pipe.AddQueue()

	var qps int

	cellnet.NewTimer(timeEvq, time.Second, func(t *cellnet.Timer) {

		if qps > 0 {
			log.Debugf("QPS: %d", qps)
		}

		qps = 0
	})

	gate.RegisterSessionMessage("gamedef.EnterGameREQ", func(content interface{}, gateSes cellnet.Session, clientid int64) {

		//msg := content.(*gamedef.EnterGameREQ)

		qps++

		gate.SendToClient(gateSes, clientid, &gamedef.EnterGameACK{})

	})

}
