package main

import (
	"gamesvc/benchmark"
	"table"

	"github.com/davyxu/cellnet"
	"github.com/davyxu/cellnet/gate"
	"github.com/davyxu/golog"
)

var log *golog.Logger = golog.New("main")

func main() {

	table.LoadServiceTable()

	pipe := cellnet.NewEventPipe()

	log.Debugln("start gate connector")

	gate.StartGateConnector(pipe, table.GetPeerAddressList("svc->gate"))

	log.Debugln("benchmark.Start")
	// 组消息初始化
	benchmark.Start(pipe)

	log.Debugln("pipe start")

	pipe.Start()

	log.Debugln("done")
	pipe.Wait()
}
