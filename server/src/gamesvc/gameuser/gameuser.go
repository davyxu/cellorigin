package gameuser

import (
	"user"
)

// 最多创建的角色数量
const MaxCharCount = 2

// 账号信息
type DBAccount struct {
	Account *AccountData

	Char []*CharData // 多角色
}

// 根据角色名找到角色
func (self *DBAccount) GetChar(charname string) *CharData {
	for _, c := range self.Char {
		if c.CharName == charname {
			return c
		}
	}

	return nil
}

type AccountData struct {
	AccountID string // 账号标示号

}

// 角色信息
type CharData struct {
	CharName     string
	Coin         int32
	LastLoginUTC int64 // 上次登录的时间戳
	Diamond      int32

	// 这里会有角色内的所有model
}

// 逻辑中使用到的对象
type GameUser struct {
	*user.RouterUser

	*AccountData
	*CharData

	OnDataReady user.Action
}
