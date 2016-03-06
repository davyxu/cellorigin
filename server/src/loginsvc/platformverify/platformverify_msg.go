package platformverify

import (
	"fmt"
	"proto/gamedef"
	"strings"
	"table"

	"github.com/davyxu/cellnet"
	"github.com/davyxu/cellnet/socket"
	"github.com/davyxu/golog"
)

var log *golog.Logger = golog.New("benchmark")

// 从配置表里构建下发客户端的列表
func getServerInfoFromTable() []*gamedef.ServerInfo {
	svinfoList := make([]*gamedef.ServerInfo, 0)

	for _, def := range table.PeerMap {

		if def.PeerName == "client->agent" {
			svinfoList = append(svinfoList, &gamedef.ServerInfo{
				Name:        def.Name,
				DisplayName: def.DisplayName,
				Address:     fmt.Sprintf("%s:%d", def.IP, def.Port),
			})

		}
	}

	return svinfoList
}

func Start(evq cellnet.EventQueue) {

	// TODO 从GM服务器直接获取配置
	svinfoList := getServerInfoFromTable()

	socket.RegisterSessionMessage(evq, "gamedef.LoginREQ", func(content interface{}, ses cellnet.Session) {

		msg := content.(*gamedef.LoginREQ)

		// TODO 从GM服务器直接获取配置
		if table.ServiceConfig.Version != "" &&
			table.ServiceConfig.Version != strings.TrimSpace(msg.ClientVersion) {

			ses.Send(&gamedef.LoginACK{Result: "error client version"})
			return
		}

		ses.Send(&gamedef.LoginACK{
			ServerList: svinfoList,
		})

	})

}
