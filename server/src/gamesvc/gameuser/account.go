package gameuser

// 最多创建的角色数量
const MaxCharCount = 2

// 账号信息
type DBAccount struct {
	Account *AccountData

	Char []*CharData // 所有角色的id
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
