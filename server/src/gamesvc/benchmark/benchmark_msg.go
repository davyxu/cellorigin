package benchmark

import (
	"proto/gamedef"
	"sync"

	"time"

	"github.com/davyxu/cellnet"
	"github.com/davyxu/cellnet/gate"
	"github.com/davyxu/golog"
)

var log *golog.Logger = golog.New("benchmark")

func Start(pipe cellnet.EventPipe) {

	golog.SetLevelByString("socket", "info")

	timeEvq := pipe.AddQueue()

	var qpsGuard sync.Mutex
	var qps int

	cellnet.NewTimer(timeEvq, time.Second, func(t *cellnet.Timer) {

		qpsGuard.Lock()

		defer qpsGuard.Unlock()

		if qps > 0 {
			log.Infof("QPS: %d", qps)
		}

		qps = 0
	})

	gate.RegisterSessionMessage("gamedef.EnterGameREQ", func(content interface{}, gateSes cellnet.Session, clientid int64) {

		//msg := content.(*gamedef.EnterGameREQ)

		qpsGuard.Lock()

		qps++

		qpsGuard.Unlock()

		gate.SendToClient(gateSes, clientid, &gamedef.EnterGameACK{})

	})

}
