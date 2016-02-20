package table

import (
	"flag"
	"proto/gamedef"
	"share"
)

// 所有的端配置
var PeerMap = make(map[string]*gamedef.ServiceDefine)

var ServiceConfig *gamedef.ServiceDefine

func GetPeerDefineList(peerName string) []*gamedef.ServiceDefine {

	deflist := make([]*gamedef.ServiceDefine, 0)

	for name, def := range PeerMap {

		if name == peerName {
			deflist = append(deflist, def)
		}
	}

	return deflist
}

func makeServiceConfig(name string, svcindex int32) {

	var serviceFile gamedef.ServiceFile

	log.Infoln("load service table...")

	if share.LoadTable("Service", &serviceFile) != nil {
		return
	}

	for _, def := range serviceFile.Service {

		if def.Name != name {
			continue
		}

		// 记录主配置
		if def.MainConfig {
			ServiceConfig = def
		}

		// peer名重名
		if _, ok := PeerMap[def.PeerName]; ok {
			log.Warnln("duplicate peer name", def.PeerName)
			continue
		}

		PeerMap[def.PeerName] = def
	}
}

// 通过命令行提供服务索引
var paramSvcIndex = flag.Int("index", 0, "service index")

func LoadServiceTable() {

	var localFile gamedef.LocalFile

	// 从Local中读取配置
	if share.LoadTable("Local", &localFile) != nil {
		log.Errorln("local file not found")
		return
	}

	// 根据配置名, 在Service表中找到对应名字的, 提出来
	makeServiceConfig(localFile.ServiceConfig, int32(*paramSvcIndex))

}
