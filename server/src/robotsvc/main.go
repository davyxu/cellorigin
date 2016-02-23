package main

import (
	"robotsvc/benchmark"
	"table"

	"flag"

	"github.com/davyxu/cellnet"
	"github.com/davyxu/cellnet/socket"
	"github.com/davyxu/golog"
)

var log *golog.Logger = golog.New("main")

func main() {

	flag.Parse()

	table.LoadServiceTable()

	pipe := cellnet.NewEventPipe()

	conn := socket.NewConnector(pipe)
	evq := conn.Start(table.GetPeerAddress("client->gate"))
	benchmark.Start(pipe, evq)

	pipe.Start()

	pipe.Wait()
}
