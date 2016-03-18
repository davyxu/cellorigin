package user

import (
	"fmt"

	"github.com/davyxu/cellnet"
	"github.com/davyxu/cellnet/router"
)

// 网关通信用的用户
type RouterUser struct {

	// 网关信息
	routerSes cellnet.Session
	clientid  int64
}

func (self *RouterUser) ID() int64 {
	return self.clientid
}

func (self *RouterUser) String() string {
	return fmt.Sprintf("user id: %d router: %d", self.clientid, self.routerSes.ID())
}

func (self *RouterUser) Send(data interface{}) {

	router.SendToClient(self.routerSes, self.clientid, data)
}

func (self *RouterUser) Close() {

	router.CloseClient(self.routerSes, self.clientid)
}

func NewRouterUser(routerSes cellnet.Session, clientid int64) *RouterUser {
	return &RouterUser{
		routerSes: routerSes,
		clientid:  clientid,
	}
}
