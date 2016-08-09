package backend

import (
	"proto/gamedef"

	"github.com/davyxu/cellnet"
	"github.com/davyxu/cellnet/socket"
)

// Backend的各种服务器端使用以下代码

var routerConnArray []cellnet.Peer

type relayEvent struct {
	*socket.SessionEvent

	ClientID int64
}

const defaultReconnectSec = 2

// 后台服务器到router的连接
func StartBackendConnector(pipe cellnet.EventPipe, addressList []string, peerName string, svcID string) {

	routerConnArray = make([]cellnet.Peer, len(addressList))

	if len(addressList) == 0 {
		log.Warnf("empty router address list")
		return
	}

	for index, addr := range addressList {

		peer := socket.NewConnector(pipe)
		peer.SetName(peerName)

		peer.(cellnet.Connector).SetAutoReconnectSec(defaultReconnectSec)

		peer.Start(addr)

		routerConnArray[index] = peer

		// 连上网关时, 发送自己的服务器名字进行注册
		socket.RegisterSessionMessage(peer, "gamedef.SessionConnected", func(content interface{}, ses cellnet.Session) {

			ses.Send(&gamedef.RegisterRouterBackendACK{
				SvcID: svcID,
			})

		})

		// 广播
		socket.RegisterSessionMessage(peer, "gamedef.UpstreamACK", func(content interface{}, ses cellnet.Session) {
			msg := content.(*gamedef.UpstreamACK)

			// 生成派发的消息

			// TODO 用PostData防止多重嵌套?
			// 调用已注册的回调
			peer.CallData(&relayEvent{
				SessionEvent: socket.NewSessionEvent(msg.MsgID, ses, msg.Data),
				ClientID:     msg.ClientID,
			})

		})

	}

}
