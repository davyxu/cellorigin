package main

import (
	//	"gamesvc/benchmark"
	"table"

	"github.com/davyxu/cellnet"
	"github.com/davyxu/cellnet/gate"
	"github.com/davyxu/golog"
)

var log *golog.Logger = golog.New("main")

func main() {

	table.LoadServiceTable()

	//gate.DebugMode = true

	pipe := cellnet.NewEventPipe()

	gate.StartGateConnector(pipe, table.GetPeerAddressList("svc->gate"))

	// 组消息初始化
	Start(pipe)

	pipe.Start()

	pipe.Wait()
}
