package behavior

import (
	"proto/gamedef"
	"table"

	"github.com/davyxu/cellnet"
	"github.com/davyxu/cellnet/router"
)

var behaviorByMapperID = make(map[int64][]*gamedef.BehaviorDefine)

// 根据类型和一些meta数据取出对应的行为列表
func getBehaviorList(t gamedef.TriggerType, msgid uint32) []*gamedef.BehaviorDefine {

	mapperID := int64(msgid)<<32 | int64(t)

	if v, ok := behaviorByMapperID[mapperID]; ok {
		return v
	}

	return nil
}

func buildBehaviorMapper() {

	for _, def := range table.BehaviorByID {

		if !def.Enable {
			continue
		}

		// id在低位
		mapperID := int64(def.Trigger)

		// 接收消息类型的映射
		if def.Trigger == gamedef.TriggerType_RecvMessage {

			meta := cellnet.MessageMetaByName(def.MessageName)
			if meta != nil {
				log.Errorln("message name not found: %s", def.MessageName)
				continue
			}

			// 消息号在高位
			mapperID = int64(meta.ID)<<32 | mapperID

			// 注册网关事件
			router.RegisterMessage(def.MessageName, func(content interface{}, routerSes cellnet.Session, clientid int64) {

				Trigger(nil, gamedef.TriggerType_RecvMessage, meta.ID, content)
			})
		}

		// 映射到list
		list, ok := behaviorByMapperID[mapperID]
		if ok {
			list = append(list, def)

		} else {
			list = make([]*gamedef.BehaviorDefine, 0)
		}

		behaviorByMapperID[mapperID] = list

	}

}

func init() {
	buildBehaviorMapper()
}
