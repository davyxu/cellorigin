package benchmark

import (
	"proto/gamedef"
	"sync"
	"time"

	"github.com/davyxu/cellnet"
	"github.com/davyxu/cellnet/socket"
	"github.com/davyxu/golog"
)

var log *golog.Logger = golog.New("benchmark")

func Start(pipe cellnet.EventPipe, evq cellnet.EventQueue) {

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

	socket.RegisterSessionMessage(evq, "gamedef.EnterGameACK", func(content interface{}, ses cellnet.Session) {

		qpsGuard.Lock()

		qps++

		qpsGuard.Unlock()

		//ses.Send(&gamedef.EnterGameREQ{})
	})

	socket.RegisterSessionMessage(evq, "coredef.SessionConnected", func(content interface{}, ses cellnet.Session) {

		ses.Send(&gamedef.EnterGameREQ{})

	})

}
