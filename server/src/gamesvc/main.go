package main

import (
	"backend"
	"gamesvc/charinit"
	"gamesvc/gameuser"
	"gamesvc/verify"
	"table"

	"github.com/davyxu/cellnet"
	"github.com/davyxu/cellnet/socket"
	"github.com/davyxu/golog"
)

var log *golog.Logger = golog.New("main")

var msgLogFilter = make(map[uint32]*cellnet.MessageMeta)

func addMsgLogBlock(msgName string) {
	meta := cellnet.MessageMetaByName(msgName)

	if meta == nil {
		log.Errorf("msg log block not found: %s", msgName)
		return
	}

	msgLogFilter[meta.ID] = meta

}

func main() {

	table.LoadServiceTable()

	addMsgLogBlock("gamedef.UpstreamACK")
	addMsgLogBlock("gamedef.DownstreamACK")

	// 屏蔽socket层消息日志, 避免重复
	socket.SetMessageLogHook(func(info *socket.MessageLogInfo) bool {

		// 找到
		if _, ok := msgLogFilter[info.ID]; ok {
			return false
		}

		return true

	})

	pipe := cellnet.NewEventPipe()

	// DB消息投递
	evq := pipe.AddQueue()

	backend.StartBackendConnector(pipe, table.GetPeerAddressList("svc->agent"), "svc->agent", "game")

	// 组消息初始化
	gameuser.Start()
	verify.Start(evq)
	charinit.Start(evq)

	pipe.Start()

	pipe.Wait()
}
