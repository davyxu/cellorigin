package main

import (
	"github.com/davyxu/cellnet/socket"
	"svcmod/login/platformverify"
	"github.com/davyxu/golog"
	"svc"
)

var log *golog.Logger = golog.New("main")

func main() {

	svc.Init("login")

	peer := socket.NewAcceptor(svc.Queue)

	platformverify.Start(peer)

	peer.Start("127.0.0.1:8001")

	svc.Run()
}
