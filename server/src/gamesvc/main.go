package main

import (
	"fmt"
	"gamesvc/benchmark"
	"table"

	"github.com/davyxu/cellnet"
	"github.com/davyxu/cellnet/gate"
	"github.com/davyxu/golog"
)

var log *golog.Logger = golog.New("main")

func main() {

	table.LoadServiceTable()

	gate.DebugMode = true

	pipe := cellnet.NewEventPipe()

	// 准备gate的连接地址
	gateAddrList := make([]string, 0)

	for _, def := range table.GetPeerDefineList("game->gate") {
		gateAddrList = append(gateAddrList, fmt.Sprintf("%s:%d", def.IP, def.Port))
	}

	gate.StartGateConnector(pipe, gateAddrList)

	// 组消息初始化
	benchmark.Start(pipe)

	pipe.Start()

	pipe.Wait()
}
