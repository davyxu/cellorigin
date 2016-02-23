package table

import (
	"flag"
	"fmt"
	"proto/gamedef"
	"share"
)

// 所有的端配置
var PeerMap = make(map[string]*gamedef.ServiceDefine)

var ServiceConfig *gamedef.ServiceDefine

// 获取给定peerName的所有可用地址列表
func GetPeerAddressList(peerName string) []string {

	addrlist := make([]string, 0)

	for _, def := range PeerMap {

		if def.PeerName == peerName {
			addrlist = append(addrlist, fmt.Sprintf("%s:%d", def.IP, def.Port))
		}
	}

	return addrlist
}

// 根据peerName和当前进程的index获取到地址
func GetPeerAddress(peerName string) string {

	if def, ok := PeerMap[makeMapKey(peerName, GetSvcIndex())]; ok {
		return fmt.Sprintf("%s:%d", def.IP, def.Port)
	}

	return ""
}

func makeMapKey(name string, index int32) string {
	return fmt.Sprintf("%s#%d", name, index)
}

func makeServiceConfig(name string, svcindex int32) {

	var serviceFile gamedef.ServiceFile

	log.Infoln("load service table...")

	if share.LoadTable("Service", &serviceFile) != nil {
		return
	}

	log.Infoln("service table loaded")

	for _, def := range serviceFile.Service {

		if def.Name != name {
			continue
		}

		// 记录主配置
		if def.MainConfig {
			ServiceConfig = def
		}

		thisKey := makeMapKey(def.PeerName, def.PeerIndex)

		// peer名重名
		if _, ok := PeerMap[thisKey]; ok {
			log.Warnln("duplicate peer name", def.PeerName)
			continue
		}

		PeerMap[thisKey] = def
	}
}

// 通过命令行提供服务索引
var paramSvcIndex = flag.Int("index", 0, "service index")

func GetSvcIndex() int32 {
	return int32(*paramSvcIndex)
}

func LoadServiceTable() {

	log.Infof("svc index: %d", *paramSvcIndex)

	var localFile gamedef.LocalFile

	// 从Local中读取配置
	if share.LoadTable("Local", &localFile) != nil {
		log.Errorln("local file not found")
		return
	}

	// 根据配置名, 在Service表中找到对应名字的, 提出来
	makeServiceConfig(localFile.ServiceConfig, GetSvcIndex())

}
