package main

import (
	"svc/loginsvc/platformverify"
	"table"

	"github.com/davyxu/cellnet"
	"github.com/davyxu/cellnet/socket"
	"github.com/davyxu/golog"
)

var log *golog.Logger = golog.New("main")

func main() {

	table.LoadServiceTable()

	pipe := cellnet.NewEventPipe()

	evq := socket.NewAcceptor(pipe)
	evq.SetName("client->login")
	evq.Start(table.GetPeerAddress("client->login"))

	platformverify.Start(evq)

	pipe.Start()

	pipe.Wait()
}
