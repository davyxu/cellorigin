package main

import (
	"github.com/davyxu/cellnet"
	"github.com/davyxu/cellnet/gate"
)

func main() {

	pipe := cellnet.NewEventPipe()

	gate.StartBackendAcceptor(pipe, gatecfg.InternalAddress)

	gate.StartClientAcceptor(pipe, gatecfg.ExternalAddress)

	pipe.Start()

	pipe.Wait()
}
