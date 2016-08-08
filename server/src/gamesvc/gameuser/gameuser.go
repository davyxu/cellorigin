package gameuser

import (
	"user"
)

// 逻辑中使用到的对象
type GameUser struct {
	*user.RouterUser

	*CharData
}
