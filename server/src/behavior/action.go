package behavior

import (
	"proto/gamedef"
)

var actionByType map[gamedef.ActionType]func(def *gamedef.BehaviorDefine, args ...interface{})

func RegisterAction(t gamedef.ActionType, callback func(def *gamedef.BehaviorDefine, args ...interface{})) {

	if _, ok := actionByType[t]; ok {
		log.Errorln("duplicate action: %v", t)
		return
	}

	actionByType[t] = callback
}

func InvokeAction(def *gamedef.BehaviorDefine, args ...interface{}) {

	if a, ok := actionByType[def.Action]; ok {

		a(def, args...)
	}

}
