package backend

import (
	"github.com/davyxu/golog"

	"github.com/davyxu/cellnet"
)

var log *golog.Logger = golog.New("backend")

func getMsgName(msgid uint32) string {

	if meta := cellnet.MessageMetaByID(msgid); meta != nil {
		return meta.Name
	}

	return ""
}
