package main

import (
	"flag"
	"table"

	"github.com/davyxu/cellnet"
	"github.com/davyxu/cellnet/router"
	"github.com/davyxu/golog"
)

var log *golog.Logger = golog.New("main")

func main() {

	flag.Parse()

	table.LoadServiceTable()

	router.DebugMode = true

	// 后台与前台在两个线程
	backendPipe := cellnet.NewEventPipe()
	frontendPipe := cellnet.NewEventPipe()

	router.SetRelayMethod(router.RelayMethod_WhiteList)

	// 根据自动生成列表来制作这个

	router.RelayMessage("game", "gamedef.VerifyGameREQ")
	router.RelayMessage("game", "gamedef.CharListREQ")
	router.RelayMessage("game", "gamedef.CreateCharREQ")
	router.RelayMessage("game", "gamedef.EnterGameREQ")

	router.StartBackendAcceptor(backendPipe, table.GetPeerAddress("svc->agent"), "svc->agent")

	router.StartFrontendAcceptor(frontendPipe, table.GetPeerAddress("client->agent"), "client->agent")

	backendPipe.Start()

	frontendPipe.Start()

	backendPipe.Wait()
	frontendPipe.Wait()
}
