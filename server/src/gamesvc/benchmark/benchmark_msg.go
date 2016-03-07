package benchmark

import (
	"proto/gamedef"
	"sync"

	"time"

	"github.com/davyxu/cellnet"
	"github.com/davyxu/cellnet/router"
	"github.com/davyxu/golog"
)

var log *golog.Logger = golog.New("benchmark")

func Start(pipe cellnet.EventPipe) {

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

	router.RegisterSessionMessage("gamedef.EnterGameREQ", func(content interface{}, routerSes cellnet.Session, clientid int64) {

		//msg := content.(*gamedef.EnterGameREQ)

		qpsGuard.Lock()

		qps++

		qpsGuard.Unlock()

		router.SendToClient(routerSes, clientid, &gamedef.EnterGameACK{})

	})

}
