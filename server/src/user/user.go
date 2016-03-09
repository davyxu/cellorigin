package user

import (
	"fmt"

	"github.com/davyxu/cellnet"
	"github.com/davyxu/cellnet/router"
)

type User struct {
	routerSes cellnet.Session
	clientid  int64
	Event     *EventDispatcher
}

func (self *User) ID() int64 {
	return self.clientid
}

func (self *User) String() string {
	return fmt.Sprintf("user id: %d router: %d", self.clientid, self.routerSes.ID())
}

func (self *User) Send(data interface{}) {

	router.SendToClient(self.routerSes, self.clientid, data)
}

func NewUser(routerSes cellnet.Session, clientid int64) *User {
	return &User{
		routerSes: routerSes,
		clientid:  clientid,
		Event:     NewEventDispatcher(),
	}
}
