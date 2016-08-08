package gameuser

// 角色信息
type CharData struct {
	CharName     string
	Coin         int32
	LastLoginUTC int64 // 上次登录的时间戳
	Diamond      int32

	// 这里会有角色内的所有model
}
