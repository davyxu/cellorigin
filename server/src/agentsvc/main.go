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

	router.StartBackendAcceptor(backendPipe, table.GetPeerAddress("svc->agent"), "svc->agent")

	router.StartFrontendAcceptor(frontendPipe, table.GetPeerAddress("client->agent"), "client->agent")

	backendPipe.Start()

	frontendPipe.Start()

	backendPipe.Wait()
	frontendPipe.Wait()
}
