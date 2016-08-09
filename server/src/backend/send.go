package backend

import (
	"proto/gamedef"

	"github.com/davyxu/cellnet"
)

// 将消息发送到客户端
func SendToClient(routerSes cellnet.Session, clientid int64, data interface{}) {

	if routerSes == nil {
		return
	}

	msgContent := data.(interface {
		String() string
	}).String()

	userpkt, _ := cellnet.BuildPacket(data)

	log.Debugf("backend->router clientid: %d %s(%d) size: %d |%s", clientid, getMsgName(userpkt.MsgID), userpkt.MsgID, len(userpkt.Data), msgContent)

	routerSes.Send(&gamedef.DownstreamACK{
		Data:     userpkt.Data,
		MsgID:    userpkt.MsgID,
		ClientID: []int64{clientid},
	})
}

// 通知网关关闭客户端连接
func CloseClient(routerSes cellnet.Session, clientid int64) {

	if routerSes == nil {
		return
	}

	log.Debugf("backend->router clientid: %d CloseClient", clientid)

	// 通知关闭
	routerSes.Send(&gamedef.CloseClientACK{
		ClientID: clientid,
	})
}

// 广播所有的客户端
func CloseAllClient() {

	log.Debugf("backend->router CloseAllClient")

	ack := &gamedef.CloseClientACK{}

	for _, conn := range routerConnArray {
		ses := conn.(connSesManager).DefaultSession()
		if ses == nil {
			continue
		}

		ses.Send(ack)
	}
}

type connSesManager interface {
	DefaultSession() cellnet.Session
}

// 发送给所有router的所有客户端
func BroadcastToClient(data interface{}) {

	msgContent := data.(interface {
		String() string
	}).String()

	pkt, _ := cellnet.BuildPacket(data)

	ack := &gamedef.DownstreamACK{
		Data:  pkt.Data,
		MsgID: pkt.MsgID,
	}

	log.Debugf("backend->router BroadcastToClient %s(%d) size: %d|%s", getMsgName(pkt.MsgID), pkt.MsgID, len(pkt.Data), msgContent)

	for _, conn := range routerConnArray {
		ses := conn.(connSesManager).DefaultSession()
		if ses == nil {
			continue
		}

		ses.Send(ack)
	}
}

// 客户端列表
type ClientList map[cellnet.Session][]int64

func (self ClientList) Add(routerSes cellnet.Session, clientid int64) {

	// 事件
	list, ok := self[routerSes]

	// 新建
	if !ok {

		list = make([]int64, 0)
	}

	list = append(list, clientid)

	self[routerSes] = list
}

func (self ClientList) Get(ses cellnet.Session) []int64 {
	if v, ok := self[ses]; ok {
		return v
	}

	return nil
}

func NewClientList() ClientList {
	return make(map[cellnet.Session][]int64)
}

// 发送给指定客户端列表的客户端
func BroadcastToClientList(data interface{}, list ClientList) {

	msgContent := data.(interface {
		String() string
	}).String()

	pkt, _ := cellnet.BuildPacket(data)

	log.Debugf("backend->router BroadcastToClientList %s(%d) size: %d|%s", getMsgName(pkt.MsgID), pkt.MsgID, len(pkt.Data), msgContent)

	for ses, clientlist := range list {

		ack := &gamedef.DownstreamACK{
			Data:  pkt.Data,
			MsgID: pkt.MsgID,
		}

		ack.ClientID = clientlist

		ses.Send(ack)
	}

}
