package main

import (
	"table"

	"github.com/davyxu/cellnet"
	"github.com/davyxu/cellnet/gate"
	"github.com/davyxu/golog"
)

func main() {

	golog.SetLevelByString("socket", "info")

	table.LoadServiceTable()

	pipe := cellnet.NewEventPipe()

	gate.StartBackendAcceptor(pipe, table.GetPeerAddress("svc->gate"))

	gate.StartClientAcceptor(pipe, table.GetPeerAddress("client->gate"))

	pipe.Start()

	pipe.Wait()
}
