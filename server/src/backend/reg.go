package backend

import (
	"github.com/davyxu/cellnet"
)

// 注册从网关接收到的消息
func RegisterMessage(msgName string, userHandler func(interface{}, cellnet.Session, int64)) {

	msgMeta := cellnet.MessageMetaByName(msgName)

	if msgMeta == nil {
		log.Errorf("message register failed, %s", msgName)
		return
	}

	for _, conn := range routerConnArray {

		conn.RegisterCallback(msgMeta.ID, func(data interface{}) {

			if ev, ok := data.(*relayEvent); ok {

				rawMsg, err := cellnet.ParsePacket(ev.Packet, msgMeta.Type)

				if err != nil {
					log.Errorln("unmarshaling error:\n", err)
					return
				}

				msgContent := rawMsg.(interface {
					String() string
				}).String()

				log.Debugf("router->backend clientid: %d %s(%d) size: %d|%s", ev.ClientID, getMsgName(ev.MsgID), ev.MsgID, len(ev.Packet.Data), msgContent)

				userHandler(rawMsg, ev.Ses, ev.ClientID)

			}

		})
	}

}
