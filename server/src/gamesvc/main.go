package main

import (
	"gamesvc/charinit"
	"gamesvc/gameuser"
	"gamesvc/verify"

	"table"

	"github.com/davyxu/cellnet"
	"github.com/davyxu/cellnet/router"
	"github.com/davyxu/golog"
)

var log *golog.Logger = golog.New("main")

func main() {

	table.LoadServiceTable()

	pipe := cellnet.NewEventPipe()

	// DB消息投递
	evq := pipe.AddQueue()

	router.StartBackendConnector(pipe, table.GetPeerAddressList("svc->agent"), "svc->agent", "game")

	// 组消息初始化
	gameuser.Start()
	verify.Start(evq)
	charinit.Start(evq)

	pipe.Start()

	pipe.Wait()
}
