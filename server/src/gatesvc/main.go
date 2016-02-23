package main

import (
	"flag"
	"table"

	"github.com/davyxu/cellnet"
	"github.com/davyxu/cellnet/gate"
	"github.com/davyxu/golog"
)

var log *golog.Logger = golog.New("benchmark")

var paramShowLog = flag.Bool("showlog", false, "showlog")

func main() {

	flag.Parse()

	if *paramShowLog == false {
		golog.SetLevelByString("socket", "info")
	}

	table.LoadServiceTable()

	// 后台与前台在两个线程
	backendPipe := cellnet.NewEventPipe()
	frontendPipe := cellnet.NewEventPipe()

	log.Infoln("StartBackendAcceptor")
	gate.StartBackendAcceptor(backendPipe, table.GetPeerAddress("svc->gate"))

	log.Infoln("StartClientAcceptor")
	gate.StartClientAcceptor(frontendPipe, table.GetPeerAddress("client->gate"))

	log.Infoln("backendPipe.Start")
	backendPipe.Start()

	log.Infoln("frontendPipe.Start")
	frontendPipe.Start()

	log.Infoln("done")
	backendPipe.Wait()
	frontendPipe.Wait()
}
