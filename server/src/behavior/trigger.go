package behavior

import (
	"proto/gamedef"
)

type BehaviorImplement interface {
	CheckCondition(def *gamedef.BehaviorDefine, args ...interface{}) bool
}

func Trigger(bi BehaviorImplement, t gamedef.TriggerType, msgid uint32, args ...interface{}) {

	list := getBehaviorList(t, msgid)
	if list == nil {
		return
	}

	for _, def := range list {

		if bi.CheckCondition(def, args...) {
			InvokeAction(def, args...)
		}
	}
}
