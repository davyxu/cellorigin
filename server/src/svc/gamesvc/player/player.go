package player

import (
	"user"

	"github.com/davyxu/cellnet"
)

// 逻辑中使用到的对象
type Player struct {
	*user.RouterUser `model:"-"`

	*MemChar
	*MemAccount
}

// 映射账号数据
func (self *Player) InitAccount(a *DBAccount) {
	self.MemAccount = &MemAccount{
		DBAccount: a,
	}
}

// 映射角色数据
func (self *Player) InitChar(c *DBChar) {
	self.MemChar = &MemChar{
		DBChar: c,
	}
}

func UserExists(clientid int64) bool {
	_, ok := playerByID[clientid]
	return ok
}

func AddPlayer(routerSes cellnet.Session, clientid int64) *Player {

	u := &Player{RouterUser: user.NewRouterUser(routerSes, clientid)}

	playerByID[clientid] = u

	return u
}

func RemovePlayer(clientid int64) {
	delete(playerByID, clientid)
}

var (
	// 按连接号索引的用户
	playerByID = make(map[int64]*Player)

	// 角色名索引的用户
	//userByCharName = make(map[string]*GameUser)

	// 账户名索引的用户
	//userByAccountName = make(map[string]*GameUser)

	// 1个连接同时只有1个账户, 1个角色在线
)

//func UserByAccount(name string) *GameUser {
//	if v, ok := userByAccountName[name]; ok {
//		return v
//	}

//	return nil
//}

//func UserByCharName(name string) *GameUser {
//	if v, ok := userByCharName[name]; ok {
//		return v
//	}

//	return nil
//}
