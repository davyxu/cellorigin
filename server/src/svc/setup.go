package svc

import "github.com/davyxu/cellnet"

func Init(svcName string) {

	Queue = cellnet.NewEventQueue()

}

func Run() {

	Queue.StartLoop()

	Queue.Wait()
}
