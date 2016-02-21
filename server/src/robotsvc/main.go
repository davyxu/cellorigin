package main

import (
	"proto/gamedef"
	"table"

	"github.com/davyxu/cellnet"
	"github.com/davyxu/cellnet/socket"
	"github.com/davyxu/golog"
)

var log *golog.Logger = golog.New("main")

func main() {

	//golog.SetLevelByString("socket", "info")

	table.LoadServiceTable()

	pipe := cellnet.NewEventPipe()

	conn := socket.NewConnector(pipe)

	evq := conn.Start(table.GetPeerAddress("client->gate"))

	socket.RegisterSessionMessage(evq, "gamedef.EnterGameACK", func(content interface{}, ses cellnet.Session) {

		ses.Send(&gamedef.EnterGameREQ{})
	})

	socket.RegisterSessionMessage(evq, "coredef.SessionConnected", func(content interface{}, ses cellnet.Session) {

		ses.Send(&gamedef.EnterGameREQ{})

	})

	pipe.Start()

	pipe.Wait()
}
