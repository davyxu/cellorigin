package main

import (
	"gamesvc/benchmark"
	"gamesvc/usermgr"
	"table"

	"github.com/davyxu/cellnet"
	"github.com/davyxu/cellnet/router"
	"github.com/davyxu/golog"
)

var log *golog.Logger = golog.New("main")

func main() {

	table.LoadServiceTable()

	pipe := cellnet.NewEventPipe()

	router.StartBackendConnector(pipe, table.GetPeerAddressList("svc->agent"), "svc->agent", "game")

	// 组消息初始化
	benchmark.Start(pipe)
	usermgr.Start()

	pipe.Start()

	pipe.Wait()
}
